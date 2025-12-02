using UnityEngine;
using Mirror;
using StarterAssets;
using TMPro;
using System.Collections;

[RequireComponent(typeof(PlayerRaycast))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(BasicRigidBodyPush))]
[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(NetworkTransformUnreliable))]
[RequireComponent(typeof(AnimationControler))]

public class Player : NetworkBehaviour
{
    [SyncVar] public int Health = 100;

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private Transform _gunHandler;
    [SerializeField] private Transform _knifeHandler;
    [SerializeField] private LayerMask _itemsLayer;
    [SerializeField] private GameObject _dieEffect;
    [SerializeField] private GameObject[] _playerSkinDetails;
    private float _dropForce;
    private float _throwForce;
    public Waepon _currentWeapon; 
    private PlayerRaycast _raycast;
    private AnimationControler _anim;
    private Transform _camera;

    private void Start()
    {
        if (ConfigContainer.Movement == null) {Invoke(nameof(Start), 0.001f); return;}

        _dropForce = ConfigContainer.Movement.DropWeaponForce;
        _throwForce = ConfigContainer.Movement.TrowWeaponForce;
        _raycast = GetComponent<PlayerRaycast>();
        _camera = Camera.main.transform;
        if (isLocalPlayer) NameText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            NameText.text = PlayerData.Instance.PlayerName;
            NameText.transform.LookAt(_camera);
        }
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeOffWeapon(_dropForce);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeOffWeapon(_throwForce);
        }
        if (_raycast.CastedObj == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab(_raycast.CastedObj);
        }
        if (Input.GetMouseButton(0))
        {
            if (_currentWeapon)
                _raycast.RpcCastBullet(_currentWeapon);
        }

        if (_currentWeapon) _currentWeapon.PlayAnim(Input.GetMouseButton(0));
    }

    [ClientRpc]
    public void Damage(int damage)
    {
        Health -= damage;
    }
    [ClientRpc]
    public void Die()
    {
        NetworkStartPosition[] Spawns = FindObjectsByType<NetworkStartPosition>(FindObjectsSortMode.None); // Search Spawn Points

        StartCoroutine(Respawn(Spawns[Random.Range(0, Spawns.Length)].transform));
    }
    private IEnumerator Respawn(Transform Spawn)
    {
        UIManager.Instance.DiePanel();
        _anim.DieAnim();
        _dieEffect.SetActive(true);
        foreach (var m in _playerSkinDetails) m.SetActive(false);
        yield return new WaitForSeconds(3);
        _dieEffect.SetActive(false);
        foreach (var m in _playerSkinDetails) m.SetActive(true);
        transform.position = Spawn.position;
        _anim.Spawn();
    }
    [ClientRpc]
    private void Grab(GameObject Object)
    {
        if (!Object) return;
        var grableObj = Object.GetComponent<GrableItem>();
        Waepon Item = null;

        foreach (var w in ConfigContainer.Weapon.WeaponSetts)
        {
            if (w.Type == grableObj.Type) Item = w.HandedVersion.GetComponent<Waepon>();
        }
        
        GameObject NewWeapon = Instantiate(Item, _gunHandler).gameObject;
        NetworkServer.Spawn(NewWeapon);
        NetworkServer.Destroy(Object);
        _currentWeapon = NewWeapon.GetComponent<Waepon>();
    }
    [ClientRpc]
    private void TakeOffWeapon(float _takeOffForce)
    {
        if (_currentWeapon == null) { Debug.LogWarning("NO WEAPON"); return; }
        GameObject PhysicsVersionOfWeapon = null;
        foreach (var w in ConfigContainer.Weapon.WeaponSetts)
        {
            if (w.Type == _currentWeapon.Type) PhysicsVersionOfWeapon = w.PhysicVersion;
        }
        Rigidbody TakedWeapon = Instantiate(PhysicsVersionOfWeapon, _camera.position, _camera.rotation, parent: null).gameObject.GetComponent<Rigidbody>();
        NetworkServer.Spawn(TakedWeapon.gameObject);
        NetworkServer.Destroy(_currentWeapon.gameObject);
        TakedWeapon.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        TakedWeapon.AddForce(_camera.forward * _takeOffForce, ForceMode.Impulse);
        TakedWeapon.AddRelativeTorque(Random.Range(1, 15), Random.Range(-200, 200),0);
    }
}
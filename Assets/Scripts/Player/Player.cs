using UnityEngine;
using Mirror;
using StarterAssets;
using Mirror.Examples.Common.Controllers.Tank;
using TMPro;

[RequireComponent(typeof(PlayerRaycast))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(BasicRigidBodyPush))]
[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(NetworkTransformUnreliable))]

public class Player : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private Transform _gunHandler;
    [SerializeField] private Transform _knifeHandler;
    [SerializeField] private Transform _shieldHandler;
    [SerializeField] private LayerMask _itemsLayer;
    private float _dropForce;
    private float _throwForce;
    public Waepon _currentWeapon; 
    [SyncVar] public int Health = 100;

    private PlayerRaycast _raycast;
    private Transform _camera;

    private void Start()
    {
        if (ConfigContainer.Movement == null) {Invoke(nameof(Start), 0.001f); return;}

        _dropForce = ConfigContainer.Movement.DropWeaponForce;
        _throwForce = ConfigContainer.Movement.TrowWeaponForce;
        _raycast = GetComponent<PlayerRaycast>();
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        HealthText.text = Health.ToString();
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
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot");
            _raycast.CastBullet(_currentWeapon.Type);
        }
    }
    [ClientRpc]
    public void Damage(int damage)
    {
        Health -= damage;
    }

    private void Grab(GameObject Object)
    {
        var grableObj = Object.GetComponent<GrableItem>();
        Waepon Item = null;

        foreach (var w in ConfigContainer.Weapon.WeaponSetts)
        {
            if (w.Type == grableObj.Type) Item = w.HandedVersion.GetComponent<Waepon>();
        }
        
        GameObject NewWeapon = Instantiate(Item, _gunHandler).gameObject;
        _currentWeapon = NewWeapon.GetComponent<Waepon>();
        NetworkServer.Spawn(NewWeapon);
        NetworkServer.Destroy(Object);
    }

    private void TakeOffWeapon(float _takeOffForce)
    {
        if (_currentWeapon == null) { Debug.LogWarning("NO WEAPON"); return; }

        Rigidbody TakedWeapon = Instantiate(_currentWeapon.PhysicsVersion, _camera.position, _camera.rotation, parent: null).gameObject.GetComponent<Rigidbody>();
        TakedWeapon.AddForce(_camera.forward * _takeOffForce, ForceMode.Impulse);
        NetworkServer.Spawn(TakedWeapon.gameObject);
        NetworkServer.Destroy(_currentWeapon.gameObject);
    }
}
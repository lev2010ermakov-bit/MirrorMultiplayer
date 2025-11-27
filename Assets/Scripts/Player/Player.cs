using UnityEngine;
using Mirror;
using StarterAssets;

[RequireComponent(typeof(PlayerRaycast))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(BasicRigidBodyPush))]
[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(NetworkTransformUnreliable))]

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform _gunHandler;
    [SerializeField] private Transform _knifeHandler;
    [SerializeField] private Transform _shieldHandler;
    [SerializeField] private LayerMask _itemsLayer;
    [SerializeField] private float _takeOffForce;
    public Waepon _currentWeapon; 

    private PlayerRaycast _raycast;
    private Transform _camera;

    private void Start()
    {
        _raycast = GetComponent<PlayerRaycast>();
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeOffWeapon();
        }

        if (_raycast.CastedObj == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab(_raycast.CastedObj);
        }
    }

    private void Grab(GameObject Object)
    {
        var grableObj = Object.GetComponent<GrableItem>();
        var Item = grableObj.HandItemVersion.GetComponent<MonoBehaviour>();
        switch (Item)
        {
            case Waepon:
            GameObject NewWeapon = Instantiate(Item, _gunHandler).gameObject;
            _currentWeapon = NewWeapon.GetComponent<Waepon>();
            NetworkServer.Spawn(NewWeapon);
                break;
        }
        NetworkServer.Destroy(Object);
    }

    private void TakeOffWeapon()
    {
        if (_currentWeapon == null) { Debug.LogWarning("NO WEAPON"); return; }

        Rigidbody TakedWeapon = Instantiate(_currentWeapon.PhysicsVersion, _camera.position, _camera.rotation, parent: null).gameObject.GetComponent<Rigidbody>();
        TakedWeapon.AddForce(_camera.forward * _takeOffForce, ForceMode.Impulse);
        NetworkServer.Spawn(TakedWeapon.gameObject);
        NetworkServer.Destroy(_currentWeapon.gameObject);
    }
}
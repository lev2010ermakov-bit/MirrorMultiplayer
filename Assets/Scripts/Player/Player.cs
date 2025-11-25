using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform _gunHandler;
    [SerializeField] private Transform _knifeHandler;
    [SerializeField] private Transform _shieldHandler;
    [SerializeField] private LayerMask _itemsLayer;
    [SerializeField] private float _takeOffForce;
    public Waepon _currentWeapon; 

    private void Update()
    {
        if (!isLocalPlayer) return;

        RaycastHit hit;

        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _itemsLayer);

        if (hit.collider == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab(hit.transform.gameObject);
        }
        if (Input.GetKey(KeyCode.G))
        {
            TakeOffWeapon();
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
           // _currentWeapon = NewWeapon.GetComponent<Waepon>();
            NetworkServer.Spawn(NewWeapon, gameObject);
                break;
            case Shield:
                break;
        }
        NetworkServer.Spawn(Object);
    }

    private void TakeOffWeapon()
    {
        Rigidbody TakedWeapon = Instantiate(_currentWeapon.PhysicsVersion, transform.position, transform.rotation, parent: null).gameObject.GetComponent<Rigidbody>();
        TakedWeapon.AddForce(Camera.main.transform.forward * _takeOffForce, ForceMode.Impulse);
        NetworkServer.Spawn(TakedWeapon.gameObject);
        NetworkServer.Destroy(_currentWeapon.gameObject);
    }
}
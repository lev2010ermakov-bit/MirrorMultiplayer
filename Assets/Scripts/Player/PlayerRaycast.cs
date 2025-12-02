using Mirror;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class PlayerRaycast : NetworkBehaviour
{
    public GameObject CastedObj;

    [Header("Settings")]

    [SerializeField] private float _maxCastDistance;
    [SerializeField] private LayerMask _usebleObjectsMask;
    [SerializeField] private LayerMask _damagebleObjectsMask;
    [SerializeField] private AudioSource _aud;



    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        rpcCastUsebleObjects();
    }
    private void rpcCastUsebleObjects()
    {
        RaycastHit hit;

        Physics.Raycast(_camera.position, _camera.forward, out hit, _maxCastDistance, _usebleObjectsMask);

        if (hit.transform == null) return;

        CastedObj = hit.transform.gameObject;
    }
    
    [ClientRpc]
    public void RpcCastBullet(Waepon Weapon)
    {
        if (Weapon.CanShoot)
        {
            RaycastHit hit;
            Physics.Raycast(_camera.position, _camera.forward, out hit, 200, _damagebleObjectsMask);
            int damage = 0;
            foreach (var w in ConfigContainer.Weapon.WeaponSetts)
            {
                if (w.Type == Weapon.Type) { damage = w.DamagePerBullet;}
            }

            if (hit.transform == null) return;

            hit.transform.GetComponent<Player>().Damage(damage);
        }
    }
}

using System.Collections.Generic;
using System.Collections;
using Mirror;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerInventory : NetworkBehaviour
{
    [SerializeField] private Transform _fpcGunHandler;
    [SerializeField] private Transform _skinGunHandler;
    [SyncVar] public List<Waepon> _weapons = new List<Waepon>();
    [SyncVar] public List<Waepon> _buyedWeaponsFPC = new List<Waepon>();
    [SyncVar] private List<bool> _weaponsActive = new List<bool>();

    private List<SkinWeapon> _skinWeapons = new List<SkinWeapon>();
    private List<SkinWeapon> _buyedWeaponsSkin = new List<SkinWeapon>();
    [SyncVar] public int _currentWeaponIndex = 0;
    private bool Sniped;

    private void Start()
    {
        if (!isLocalPlayer) return;
        _currentWeaponIndex = 0;
        if (!ConfigContainer.Weapon){Invoke(nameof(Start), 0.01f); return;}
        cmdSpawnWeapons();
        BuyWeapon(WaeponType.KNIFE);
    }

    [Command(requiresAuthority = false)]
    private void cmdSpawnWeapons()
    {
        rpcSpawnWeapons();
    }
    [ClientRpc]
    private void rpcSpawnWeapons()
    {
        if (!isServer)
        for (int i = 0; i < ConfigContainer.Weapon.WeaponSetts.Count; i++)
        {
            GameObject NewFPCWeapon = Instantiate(ConfigContainer.Weapon.WeaponSetts[i].FPCPrefab, _fpcGunHandler);
            Spawn(NewFPCWeapon);
            _weapons.Add(NewFPCWeapon.GetComponent<Waepon>());


            GameObject NewSkinWeapon = Instantiate(ConfigContainer.Weapon.WeaponSetts[i].SkinPrefab, _skinGunHandler);
            Spawn(NewSkinWeapon);
            _skinWeapons.Add(NewSkinWeapon.GetComponent<SkinWeapon>());

            _weaponsActive.Add(false);
        }
    }
    [Command(requiresAuthority = false)]
    private void Spawn(GameObject g)
    {
        NetworkServer.Spawn(g);
    }
    public void BuyWeapon(WaeponType type)
    {
        foreach (var w in _buyedWeaponsFPC) 
        {
            if (w.Type == type) 
            { 
                Debug.Log("Weapon already buyed"); 
                return; 
            }
        }

        bool WeaponFinded = false;

        foreach (var w in _weapons) 
        {
            if (w.Type == type) 
            {
                _buyedWeaponsFPC.Add(w);
                WeaponFinded = true;
            }
        }

        if (!WeaponFinded) StartCoroutine(ReBuyWeapon(type));

        foreach (var w in _skinWeapons) if (w.Type == type) _buyedWeaponsSkin.Add(w);

        SelectWeapon(0);
    }

    private IEnumerator ReBuyWeapon(WaeponType type)
    {
        yield return new WaitForSeconds(0.01f);
        BuyWeapon(type);
    }

    [Command(requiresAuthority = false)]
    public void SelectWeapon(int DeltaIndex)
    {
        if (DeltaIndex != 0)
        {
            _currentWeaponIndex += DeltaIndex;

            if (_currentWeaponIndex > _buyedWeaponsFPC.Count - 1) _currentWeaponIndex -= _buyedWeaponsFPC.Count - 1;

            if (_currentWeaponIndex < 0) _currentWeaponIndex = _buyedWeaponsFPC.Count + _currentWeaponIndex;
        }

        for (int i = 0; i < _buyedWeaponsFPC.Count; i++)
        {
            _weaponsActive[i] = i == _currentWeaponIndex;
        }
    }

    private void Update()
    {
        for(int i = 0; i < _weapons.Count - 1; i++)
        {
            _weapons[i].gameObject.SetActive(_weaponsActive[i]);
            _skinWeapons[i].gameObject.SetActive(_weaponsActive[i]);
        }
        if (!isLocalPlayer) return;
        if (_buyedWeaponsFPC.Count == 0) return;

        if (Input.GetMouseButton(0)) _buyedWeaponsFPC[_currentWeaponIndex].Shoot();

        if (Input.GetMouseButtonDown(1)) 
        {
            _buyedWeaponsFPC[_currentWeaponIndex].SniperAim(Sniped);
            Sniped = !Sniped;
        }
    }
}
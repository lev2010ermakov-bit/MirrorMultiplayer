using System;
using Mirror;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] private Transform _fpcGunHandler;
        [SerializeField] private Transform _skinGunHandler;
    
        [SyncVar(hook = nameof(cmdSetWeaponVisual))] public WeaponType currentWeapon;
        private GameObject[] _currentWeapons = new GameObject[2];
        private Animator[] _currentWeaponsAnims = new Animator[2];

        private void Start()
        {
            cmdSetWeaponVisual(WeaponType.EMPTY, currentWeapon);
        }

        [Command(requiresAuthority = false)]
        public void cmdPlayGunAnimations(WeaponAnimationType type)
        {
            rpcPlayGunAnimations(type);
        }

        [ClientRpc]
        private void rpcPlayGunAnimations(WeaponAnimationType t)
        {
            foreach (var a in _currentWeaponsAnims)
            {
                switch (t)
                {
                    case WeaponAnimationType.Shoot:
                        a.SetTrigger("Shoot");
                        break;
                }
            }
        }

        [Command(requiresAuthority = false)]
        public void ChoiseWeapon(WeaponType weapon)
        {
            currentWeapon = weapon;
        }

        [Command(requiresAuthority = false)] 
        private void cmdSetWeaponVisual(WeaponType o, WeaponType n) => rpcSetWeaponVisual(o, n);

        [ClientRpc] 
        private void rpcSetWeaponVisual(WeaponType oldValue, WeaponType newValue)
        {
            if (_currentWeapons[0]) Destroy(_currentWeapons[0]);
            if (_currentWeapons[1]) Destroy(_currentWeapons[1]);

            if (newValue == WeaponType.EMPTY) return;   

            GameObject newFPCWeapon = null;
            GameObject newSkinWeapon = null;
            foreach (var w in ConfigContainer.Weapon.WeaponSetts) 
                if (w.Type == newValue)
                    {
                        newFPCWeapon = Instantiate(w.FPCPrefab, _fpcGunHandler);
                        newSkinWeapon = Instantiate(w.SkinPrefab, _skinGunHandler);
                    }
        
            _currentWeapons[0] = newFPCWeapon;
            _currentWeapons[1] = newSkinWeapon;

            _currentWeaponsAnims[0] = newFPCWeapon.GetComponent<Animator>();
            _currentWeaponsAnims[1] = newSkinWeapon.GetComponent<Animator>();

            if (isLocalPlayer)
            {
                newSkinWeapon.SetActive(false);
            }
            else
            {
                newFPCWeapon.SetActive(false);
            }
        }
    }
}

[Serializable]
public enum WeaponAnimationType
{
    Shoot
}
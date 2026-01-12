using UnityEngine;
using Mirror;
using System;

namespace PlayerScripts
{
    public class PlayerWeaponTool : NetworkBehaviour
    {
        [SerializeField] private LayerMask Players;
        [SerializeField] private AudioSource aud;
        private Player player;
        private Transform cam;
        private void Start()
        {
            cam = Camera.main.transform;
            player = GetComponent<Player>();
        } 
        public void Shoot(WeaponType weapon)
        {
            if (weapon == WeaponType.EMPTY) return;

            Ray ray = new Ray(cam.position, cam.forward);
            RaycastHit hit;
            Player hitedPlayer = null;
        
            if (weapon != WeaponType.KNIFE)
            {
                Physics.Raycast(ray, out hit, ConfigContainer.Weapon.GunRayDistance, Players);
            }
            else
            {
                Physics.SphereCast(ray, ConfigContainer.Weapon.KnifeRayThick, out hit, ConfigContainer.Weapon.KnifeRayDistance);
            }

            if (hit.transform != null)
            {
                hitedPlayer = hit.transform.gameObject.GetComponent<Player>();
                Debug.Log(player.PlayerName + " hits " + hitedPlayer.PlayerName);
            }

            foreach (var w in ConfigContainer.Weapon.WeaponSetts)
            {
                if (w.Type == weapon)
                {
                    aud.PlayOneShot(w.ShootSound);
                    player.inventory.cmdPlayGunAnimations(WeaponAnimationType.Shoot);
                    if (hitedPlayer) HitPlayer(hitedPlayer, w.DamagePerBullet);
                }
            }
        }

        private void HitPlayer(Player player,int damage)
        {
            player.cmdDamage(damage);
        } 
    }

    [Serializable]
    public enum WeaponType
    {
        EMPTY,
        AK,
        M4A1,
        GLOCK,
        USP,
        AWP,
        KNIFE
    }
}
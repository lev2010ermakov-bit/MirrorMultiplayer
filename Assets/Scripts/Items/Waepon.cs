using System;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Waepon : NetworkBehaviour
{
    public WaeponType Type;
    public bool CanShoot = true;
    private NetworkAnimator _anim;
    private AudioClip _sound;
    private AudioSource _aud;
    void Start()
    {
        foreach (var w in ConfigContainer.Weapon.WeaponSetts){ if (w.Type == Type) _sound = w.ShootSound; }
        _aud = GetComponentInChildren<AudioSource>();
        _aud.clip = _sound;
        _anim = GetComponent<NetworkAnimator>();
    }

    [ClientRpc]
    public void PlayAnim(bool value)
    {
        if (CanShoot)
        {
            _anim.animator.SetBool("Shoot", value);
        }
    }

    [ClientRpc] 
    public void PlaySound()
    {
        _aud.Play();
    }
    [ClientRpc] private void SetCanShoot() => CanShoot = !CanShoot;
}
[Serializable]
public enum WaeponType
{
    AK, // Yes
    M4A1, // Yes
    USP, // 
    GLOCK, // Yes
    AWP, // Yes
}
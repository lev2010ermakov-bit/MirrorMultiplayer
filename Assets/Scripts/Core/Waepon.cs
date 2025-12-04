using System;
using Mirror;
using UnityEngine;

public abstract class Waepon : NetworkBehaviour
{
    [SerializeField] protected LayerMask PlayerLayerMask;
    [HideInInspector] public WaeponType Type;
    protected bool CanShoot = true;
    protected NetworkAnimator _anim;
    private AudioClip _shootSound;
    private AudioClip _aimSound;
    private AudioSource _aud;
    protected int _damage;
    protected virtual void Start()
    {
        foreach (var w in ConfigContainer.Weapon.WeaponSetts)
        { 
            if (w.Type == Type) 
            {
                _shootSound = w.ShootSound;
                _aimSound = w.AimSound ? w.AimSound : null;
                _damage = w.DamagePerBullet;
            }
        }
        _aud = GetComponentInParent<AudioSource>();
        _anim = GetComponent<NetworkAnimator>();
    }
    [Command(requiresAuthority = false)] public void cmdPlayAnim(string o) => rpcPlayAnim(o);
    [ClientRpc] protected virtual void rpcPlayAnim(string Options) {}
    public void SniperAim(bool v) { UIManager.Instance.SnipeSet(v);} 
    protected void PlaySound(SoundType s) 
    { 
        switch (s)
        {
            case SoundType.SHOOT :
                _aud.PlayOneShot(_shootSound); 
                break;
            case SoundType.SNIPER_AIM :
                _aud.PlayOneShot(_aimSound);
                break;
        }
    }
    public virtual void Shoot() {}
    public void MakeAbleToShoot() => CanShoot = true;
}
[Serializable]
public enum WaeponType
{
    AK, 
    M4A1, 
    USP, 
    GLOCK, 
    AWP, 
    KNIFE
}
[Serializable] public enum SoundType
{
    SHOOT,
    SNIPER_AIM,
}
using Mirror;
using UnityEngine;

public class Knife : Waepon
{
    protected override void Start()
    {
        CanShoot = true;
        Type = WaeponType.KNIFE;
        base.Start();
    }
    [ClientRpc] protected override void rpcPlayAnim(string o)
    {
        switch (o)
        {
            case "Shoot":
                PlaySound(SoundType.SHOOT);
                CanShoot = false;
                break;
            default: return;
        }
        _anim.animator.SetTrigger(o);
    }

    public override void Shoot()
    {
        if (!CanShoot) return;

        cmdPlayAnim("Shoot");

        RaycastHit hit;
        Ray ray;
        ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        Physics.SphereCast(ray, 1, out hit, 2, PlayerLayerMask);
        CanShoot = false;

        if (!hit.transform) return;

        hit.transform.GetComponent<Player>().serDamage(_damage);
    }
}

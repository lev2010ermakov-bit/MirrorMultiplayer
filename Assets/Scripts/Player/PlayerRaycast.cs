using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public GameObject CastedObj;

    [Header("Settings")]

    [SerializeField] private float _maxCastDistance;
    [SerializeField] private LayerMask _usebleObjectsMask;
    private AudioSource _aud;



    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
        _aud = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CastUsebleObjects();
    }

    private void CastUsebleObjects()
    {
        RaycastHit hit;

        Physics.Raycast(_camera.position, _camera.forward, out hit, _maxCastDistance, _usebleObjectsMask);

        if (hit.transform == null) return;

        CastedObj = hit.transform.gameObject;
    }
    
    public void CastBullet(WaeponType type)
    {
        RaycastHit hit;
        Physics.Raycast(_camera.position, _camera.forward, out hit, 200, layerMask: 6);
        if (hit.transform == null) return;
        int damage = 0;
        AudioClip Sound = null;
        foreach (var w in ConfigContainer.Weapon.WeaponSetts)
        {
            if (w.Type == type) { damage = w.DamagePerBullet; Sound = w.ShootSound; }
        }
        hit.transform.GetComponent<Player>().Damage(damage);
        _aud.PlayOneShot(Sound);
        Debug.Log(Sound.name + damage.ToString());
    }
}

using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public GameObject CastedObj;

    [Header("Settings")]

    [SerializeField] private float _maxCastDistance;
    [SerializeField] private LayerMask _usebleObjectsMask;



    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
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
}

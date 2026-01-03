using Mirror;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using PlayerScripts;

public class FindPartOfPlayerSystem : MonoBehaviour
{
    [SerializeField] private UICanvasControllerInput _canvasInput;
    [SerializeField] private UIManager _manager;
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private int _maxFindTrys;
    private int _trys;
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkBehaviour>().isLocalPlayer)
            {
                _canvasInput.starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
                _camera.Target.TrackingTarget = player.GetComponent<FirstPersonController>().CinemachineCameraTarget.transform;
                _manager.player = player.GetComponent<Player>();
            }
        }
        
        Invoke(nameof(Start), 0.001f);
    }
}

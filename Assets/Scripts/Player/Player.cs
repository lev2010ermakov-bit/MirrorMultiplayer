using UnityEngine;
using Mirror;
using StarterAssets;
using TMPro;
using System.Collections;

[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(BasicRigidBodyPush))]
[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(NetworkTransformUnreliable))]
[RequireComponent(typeof(AnimationControler))]

public class Player : NetworkBehaviour
{
    [SyncVar] public int Health = 100;

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private RectTransform Canvas;
    [SerializeField] private GameObject _dieEffect;
    [SerializeField] private GameObject[] _playerSkinDetails;
    private AnimationControler _anim;
    private Transform _camera;

    private void Start()
    {
        if (ConfigContainer.Movement == null) {Invoke(nameof(Start), 0.001f); return;}
        _camera = Camera.main.transform;
        if (isLocalPlayer) NameText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            Canvas.LookAt(_camera);
            Canvas.eulerAngles = Canvas.eulerAngles + new Vector3(0, 180, 0);
        
            NameText.text = PlayerData.Instance.PlayerName;
        
            HealthText.text = Health.ToString();
        }
    }

    [Command(requiresAuthority = false)] private void cmdDie() => rpcDie();
    [Server] public void serDamage(int d) => Damage(d);


    private void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            cmdDie();
        }
    }
    [ClientRpc]
    private void rpcDie()
    {
        NetworkStartPosition[] Spawns = FindObjectsByType<NetworkStartPosition>(FindObjectsSortMode.None); // Search Spawn Points

        StartCoroutine(Respawn(Spawns[Random.Range(0, Spawns.Length)].transform));
    }    
    
    private IEnumerator Respawn(Transform Spawn)
    {
        UIManager.Instance.DiePanel();
        _anim.cmdDieAnim();
        _dieEffect.SetActive(true);
        foreach (var m in _playerSkinDetails) m.SetActive(false);
        yield return new WaitForSeconds(3);
        _dieEffect.SetActive(false);
        foreach (var m in _playerSkinDetails) m.SetActive(true);
        transform.position = Spawn.position;
        _anim.cmdSpawn();
    }
}
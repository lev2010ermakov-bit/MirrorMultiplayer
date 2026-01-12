using UnityEngine;
using Mirror;
using StarterAssets;
using TMPro;
using System.Collections;
using NUnit.Framework;
using Mirror.Examples.Chat;


namespace PlayerScripts
{   
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
        [SyncVar(hook = nameof(SetHealthText))] public int Health = 100;

        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI HealthText;
        [SerializeField] private RectTransform Canvas;
        [SerializeField] private GameObject _dieEffect;
        [SerializeField] private GameObject[] _playerSkinDetails;
        public AnimationControler anim;
        public PlayerInventory inventory;
        public PlayerWeaponTool weaponTool;
        private Transform _camera;
        [SyncVar(hook = nameof(OnNameChanged))] public string PlayerName;

        private void Start()
        {

            anim = GetComponent<AnimationControler>();
            inventory = GetComponent<PlayerInventory>();
            weaponTool = GetComponent<PlayerWeaponTool>();

            _camera = Camera.main.transform;
            if (isLocalPlayer)
            {
                ChatUI.localPlayerName = PlayerData.PlayerName;
                PlayerName = PlayerData.PlayerName;
            }
            NameText.text = PlayerName;

            NameText.gameObject.SetActive(!isLocalPlayer);
            HealthText.gameObject.SetActive(!isLocalPlayer);
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                Canvas.LookAt(_camera);
                Canvas.eulerAngles = Canvas.eulerAngles + new Vector3(0, 180, 0);
            }
            if (isLocalPlayer)
                if (Input.GetMouseButtonDown(0))
                {
                    weaponTool.Shoot(inventory.currentWeapon);
                    Debug.Log("Shoot");
                }
        }

        [Command(requiresAuthority = false)] 
        public void cmdDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        private void SetHealthText(int oldValue, int newValue) => HealthText.text = newValue.ToString();
        private void OnNameChanged(string o, string n) => NameText.text = PlayerName;

        private void Die()
        {
            
        }

        public void BuyWeapon(WeaponType w)
        {
            inventory.ChoiseWeapon(w);
        }
    }
}

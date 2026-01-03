using UnityEngine;
using Mirror;
using StarterAssets;

namespace PlayerScripts
{
    public class AnimationControler : NetworkBehaviour
    {
        [SerializeField] private GameObject Capsule;
        [SerializeField] private GameObject Skin;
        [SerializeField] private NetworkAnimator _SkinAnimator;
        [SerializeField] private NetworkAnimator _FPCAnimator;
        [SerializeField] private StarterAssetsInputs _inputs;
        [SerializeField] private MeshRenderer[] _hideNeedMeshs;
        [SerializeField] private AudioClip[] _stepSounds;
        private AudioSource _aud;
        private CharacterController _character;
        [SyncVar] public float WalkSpeed;
        [SyncVar] public bool isWalk;

        private void Start()
        {
            _aud = GetComponentInChildren<AudioSource>();
            _character = GetComponent<CharacterController>();
            if (!isLocalPlayer) return;
            Skin.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            foreach (var m in _hideNeedMeshs) m.enabled = false;
        }

        private void Update()
        {
            SetAnimFrame();
        } 
        private void SetAnimFrame()
        {
            SetInput();
            SetWalkAnimations();
            Jump();
        }

        [Command(requiresAuthority = false)] public void cmdPlayStepSound() => rpcPlayStepSound();
        [Command(requiresAuthority = false)] public void cmdGroundHit() => rpcGroundHit();
        [Command(requiresAuthority = false)] public void cmdSpawn() => rpcSpawn();
        [Command(requiresAuthority = false)] public void cmdDieAnim() => rpcDieAnim();


        [ClientRpc] private void rpcPlayStepSound() { if (_character.isGrounded) _aud.PlayOneShot(_stepSounds[Random.Range(0, _stepSounds.Length)]); }
        [ClientRpc] private void rpcGroundHit() => _SkinAnimator.animator.SetTrigger("GroundHit");
        [ClientRpc] private void rpcDieAnim() => _SkinAnimator.animator.SetTrigger("Die");
        [ClientRpc] private void rpcSpawn() => _SkinAnimator.animator.SetTrigger("Spawn");


        private void SetInput()
        {
            isWalk = _inputs.move.magnitude > 0.1f;
            WalkSpeed = _inputs.sprint ? 1.6f : 1f;
        }
        private void SetWalkAnimations()
        {
            if (!isLocalPlayer)
            {
                _SkinAnimator.animator.SetBool("Walk", isWalk);
                _SkinAnimator.animator.SetFloat("WalkSpeed", WalkSpeed);
            }
            else
            {
                _FPCAnimator.animator.SetBool("Walk", isWalk);
                _FPCAnimator.animator.SetFloat("WalkSpeed", WalkSpeed);
            }
        }
        private void Jump() { if (_inputs.jump) _SkinAnimator.animator.SetTrigger("Jump"); }
    }

}
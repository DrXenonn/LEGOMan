using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [FormerlySerializedAs("_sprite")] [SerializeField] private SpriteRenderer sprite;
        [FormerlySerializedAs("_landParticles")] [SerializeField] private ParticleSystem landParticles;
        [SerializeField] private AudioClip[] _footsteps;

        private AudioSource _source;
        private IPlayerController _player;
        private ParticleSystem.MinMaxGradient _currentGradient;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _player = GetComponentInParent<IPlayerController>();
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;
        }

        private void Update()
        {
            if (_player == null) return;

            HandleSpriteFlip();

            HandleRunAnimation();
        }

        private void HandleRunAnimation()
        {
            anim.SetBool(IsMoving, _player.FrameInput.x != 0);
        }

        private void HandleSpriteFlip()
        {
            if (_player.FrameInput.x != 0) sprite.flipX = _player.FrameInput.x < 0;
        }

        private void OnJumped()
        {
            anim.SetTrigger(JumpKey);
            anim.ResetTrigger(GroundedKey);
        }

        private void OnGroundedChanged(bool grounded, float impact)
        {
            if (grounded)
            {
                DetectGroundColor();
                SetColor(landParticles);

                anim.SetTrigger(GroundedKey);
                _source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);

                landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, impact);
                landParticles.Play();
            }
        }


        private void DetectGroundColor()
        {
            var hit = Physics2D.Raycast(transform.position, Vector3.down, 1.25f);
            
            if (!hit || hit.collider.isTrigger || !hit.transform.TryGetComponent(out ColorSelection r)) return;
            var color = r.GetColor();
            _currentGradient = new ParticleSystem.MinMaxGradient(color * 0.9f, color * 1.2f);
        }

        private void SetColor(ParticleSystem ps)
        {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    }

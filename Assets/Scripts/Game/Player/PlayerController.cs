using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Player.Character;
using Assets.Scripts.Game.Player.Character.Skills;
using Assets.Scripts.Game.Player.Effects;
using Assets.Scripts.Game.Player.Physics;
using UnityEngine;

namespace Assets.Scripts.Game.Player
{
    [RequireComponent(typeof(PlayerPhysics), typeof(PlayerState))]
    public class PlayerController : MonoBehaviour, IRepositioner
    {
        /// <summary>
        /// Value by which the current vertical velocity will be multiplied when the player jumps and
        /// prematurely releases the jump button.
        /// </summary>
        [Header("Config values")]
        //--Private Variables Exposed to the Inspector.
        [SerializeField] private float _prematureJumpEndScale = 0.5f;

        [Header("Required references")] 
        [SerializeField] private PlayerMovementApplier _movementApplier;
        [SerializeField] private SkillsController _skillsController;
        [SerializeField] private CharacterRotator _characterRotator;
        [SerializeField] private PlayerEffectManager _effectManager;
        [SerializeField] private Transform _characterBody;


        //--Component References
        private Rigidbody2D rb;
        private CapsuleCollider2D cc;

        private PlayerState _playerState;
        private PlayerPhysics _playerPhysics;

        /// <summary>
        /// The velocity calculated from gravity, jump time and jump force.
        /// </summary>
        private float _jumpVelocity;
        /// <summary>
        /// Changes position of the player.
        /// </summary>
        /// <param name="newPosition">New position of the player.</param>
        public void ChangePosition(Vector2 newPosition)
        {
            transform.position = new Vector3(newPosition.x, newPosition.y, 0);
            rb.position = newPosition;
        }

        private void CalcJumpVelocity()
        {
            _jumpVelocity += _playerState.LocalGravityManager.GetBaseJumpStartVelocity();
        }


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cc = GetComponent<CapsuleCollider2D>();
            _playerPhysics = GetComponent<PlayerPhysics>();
            _playerState = GetComponent<PlayerState>();

            _playerState.RegisterDoubleJumpHandler(DoubleJump);

            InputReader.RegisterJumpHandler(NoteJumpRequest);//Order matters!
            InputReader.RegisterJumpHandler(Jump);//Order matters!
            InputReader.RegisterPrematureJumpEndHandler(PrematurelyEndJump);
            InputReader.RegisterGlideHandler(Glide);

            _playerState.ColliderSize = cc.size;

            CalcJumpVelocity();
        }

        private void Update()
        {
            CheckInput();
        }

        private void ChkWallSlide()
        {
            _skillsController.UseSkill(SkillType.WallSlide);
        }

        private void FixedUpdate()
        {
            RotatePlayer();
            _playerPhysics.CheckCollisions();

            if (_playerState.IsJumpRequestRemembered)
            {
                Jump(); //The player wanted to jump, there's a chance that the floor will be nearby soon.
            }

            ApplyMovement();
        }

        private void CheckInput()
        {
            var rawInput = InputReader.InputRaw;
            _playerState.xInput = rawInput.X;
            _playerState.yInput = rawInput.Y;
        }

        private void RotatePlayer()
        {
            if (ValueComparator.IsEqual(_playerState.xInput, -_playerState.facingDirection))
            {
                _characterRotator.TurnCharacterHorizontally();
            }
        }
        /// <summary>
        /// Called when the player releases the space quickly.
        /// </summary>
        private void PrematurelyEndJump()
        {
            if (_playerState.isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * _prematureJumpEndScale);
            }
        }
        /// <summary>
        /// The player wanted to jump - keep that in mind for the time defined in the config (PlayerState) class.
        /// </summary>
        private void NoteJumpRequest()
        {
            _playerState.PlayerRequestedJump();
        }
        private void Jump()
        {
            if (_playerState.IsJumpRequestRemembered && _playerState.canJump || _playerState.IsStandingOnGroundRemembered)
            {//If the player is standing on the ground or was standing on it merely several frames ago (or any time setup in the config)

                _playerState.PlayerJumps();

                _playerState.NewVelocity = new Vector2(rb.velocity.x, _jumpVelocity);
                rb.velocity = new Vector2(rb.velocity.x, _jumpVelocity);
            }
            else if (_playerState.IsTouchingWall || _playerState.IsTouchingWallRemembered)
            {//...or if the player is or WAS holding the wall just a moment ago, do the wall jump.
                _skillsController.UseSkill(SkillType.WallJump);
            }
        }
        /// <summary>
        /// Performs double jump.
        /// </summary>
        private void DoubleJump()
        {
            //Double jump is blocked until regular/wall jump await time has passed. Thus, this needs to be called as timeout event.
            if (_playerState.CanDoubleJump)
            {
                _skillsController.UseSkill(SkillType.DoubleJump);
            }
        }
        private void Glide(GlideStages glideStage)
        {
            _playerState.GlideStage = glideStage;
            _skillsController.UseSkill(SkillType.Glide);
        }
    

        private void RepositionToGround()
        {
            var newPos = new Vector2(_characterBody.position.x, _characterBody.position.y - _playerState.DistanceToGround);
            _characterBody.position = newPos;
        }
        private void ApplyMovement()
        {
            RepositionToGround();   //Move the player closer to the ground, if applicable.
        
            _movementApplier.ApplyMovement(Time.deltaTime);

            ChkWallSlide();

            _effectManager.ApplyEffects(_playerState, rb);
        }
    }
}

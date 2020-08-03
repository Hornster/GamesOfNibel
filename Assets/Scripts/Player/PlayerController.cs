using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Character;
using Assets.Scripts.Player.Character.Skills;
using Assets.Scripts.Player.Effects;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
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

        InputReader.RegisterJumpHandler(Jump);
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
    private void Jump()
    {
        if (_playerState.canJump)
        {
            _playerState.canJump = false;
            _playerState.isJumping = true;
            _playerState.CharacterStoppedTouchingTheGround();

            _playerState.NewVelocity = new Vector2(rb.velocity.x, _jumpVelocity);
            rb.velocity = new Vector2(rb.velocity.x, _jumpVelocity);
        }
        else if (_playerState.IsTouchingWall)
        {
            _skillsController.UseSkill(SkillType.WallJump);
        }
        else if (_playerState.CanDoubleJump)
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

        _effectManager.ApplyEffects(rb);

        Debug.Log($"Velocity: {_playerState.NewVelocity.magnitude}");
    }
}

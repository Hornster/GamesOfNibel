﻿using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Character;
using Assets.Scripts.Player.Character.Skills;
using Assets.Scripts.Player.Effects;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{
    [Header("Config values")]
    //--Private Variables Exposed to the Inspector.
    [SerializeField]
    private float movementSpeed;
    /// <summary>
    /// Used when player is forcing the character to change horizontal direction while airborne.
    /// </summary>
    [SerializeField] private float _airborneAccelerationFactor = 0.4f;
    /// <summary>
    /// Used when there's no input from the player to force the character to gradually decrease
    /// their horizontal velocity while airborne.Scales the influence of velocity on deceleration.
    /// </summary>
    [SerializeField] private float _airborneDecelVelInfluenceFactor = 0.4f;
    /// <summary>
    /// The time it takes for the character to stop horizontally while airborne from
    /// their max horizontal (running) velocity.
    /// </summary>
    [SerializeField] private float _timeToStopAirborne = 0.7f;
    /// <summary>
    /// Value by which the current vertical velocity will be multiplied when the player jumps and
    /// prematurely releases the jump button.
    /// </summary>
    [SerializeField] private float _prematureJumpEndScale = 0.5f;

    [Header("Required references")]

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
    /// Defined by division of movementSpeed/_timeToStopAirborne.
    /// Used on character while they are airborne to make the gradually halt on the X axis.
    /// </summary>
    private float _airborneDeceleration;

    private void CalcJumpVelocity()
    {
        _jumpVelocity += _playerState.LocalGravityManager.GetBaseJumpStartVelocity();
    }

    private void CalcAirborneDeceleration()
    {
        _airborneDeceleration = movementSpeed / _timeToStopAirborne;
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
        CalcAirborneDeceleration();
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
    /// <summary>
    /// Check if the character's really close to ground.
    /// If yes - reset the Y velocity.
    /// </summary>
    /// <param name="velocity">Current velocity of the player.</param>
    private Vector2 ChkHowCloseToGround(Vector2 velocity)
    {
        if (_playerState.IsStandingOnGround)
        {
            velocity.y = 0.0f;
        }

        return velocity;
    }

    private void CorrectXVelocityWhileAirborne()
    {
        var currentVelocity = rb.velocity;
        if (ValueComparator.IsEqual(_playerState.xInput, 0.0f))
        {
            float absVel = Mathf.Abs(currentVelocity.x);
            float velSign = Mathf.Sign(currentVelocity.x);

            absVel -= _airborneDeceleration * Time.deltaTime + absVel * _airborneDecelVelInfluenceFactor;

            if (absVel < 0.0f)
            {
                absVel = 0.0f;
            }

            currentVelocity.x = absVel * velSign;
        }
        else
        {
            currentVelocity.x += movementSpeed * _playerState.xInput * _airborneAccelerationFactor;
        }


        if (Mathf.Abs(currentVelocity.x) > movementSpeed)
        {
            currentVelocity.x = Mathf.Sign(currentVelocity.x) * movementSpeed;
        }
        //In the air
        _playerState.NewVelocity = currentVelocity;
        rb.velocity = _playerState.NewVelocity;
    }

    private void RepositionToGround()
    {
        var newPos = new Vector2(_characterBody.position.x, _characterBody.position.y - _playerState.DistanceToGround);
        _characterBody.position = newPos;
    }
    private void ApplyMovement()
    {
        RepositionToGround();   //Move the player closer to the ground, if applicable.
        if (_playerState.isGrounded && !_playerState.isOnSlope && !_playerState.isJumping)
        {
            var newVelocity = ChkHowCloseToGround(new Vector2(movementSpeed * _playerState.xInput, _playerState.NewVelocity.y));
            //On flat ground
            _playerState.NewVelocity = newVelocity;
            rb.velocity = newVelocity;
        }
        else if (_playerState.isGrounded && _playerState.isOnSlope && !_playerState.isJumping && _playerState.canWalkOnSlope)
        {
            //On walkable slope
            //-xInput since the normal is rotated counterclockwise
            if (_playerState.IsStandingOnGround)
            {
                _playerState.NewVelocity = new Vector2(movementSpeed * _playerState.SlopeNormalPerp.x * -_playerState.xInput, movementSpeed * _playerState.SlopeNormalPerp.y * -_playerState.xInput);
                //_playerState.NewVelocity = new Vector2(_playerState.NewVelocity.x, _playerState.NewVelocity.y -_playerState.DistanceToGround);
                rb.velocity = _playerState.NewVelocity;
            }
        }
        else if (_playerState.isGrounded && _playerState.isOnSlope &&
                 !_playerState.canWalkOnSlope)
        {
            if (ValueComparator.IsEqual(_playerState.xInput, 0f) == false
            && ValueComparator.IsEqual(_playerState.SlopeHorizontalNormal.x, 0f) == false)
            {
                //On unwalkable slope (too steep)
                var xInputSign = Mathf.Sign(_playerState.xInput);
                var xSlopeDirection = Mathf.Sign(_playerState.SlopeHorizontalNormal.x);    //X part of character's velocity is directed accordingly to the slope.

                if (ValueComparator.IsEqual(xInputSign, xSlopeDirection))
                {
                    //Player wants to move away from the slope, that is acceptable.
                    _playerState.NewVelocity = new Vector2(rb.velocity.x + movementSpeed * _playerState.xInput, rb.velocity.y);
                    rb.velocity = _playerState.NewVelocity;
                }
            }
        }
        else if (!_playerState.isGrounded)
        {
            CorrectXVelocityWhileAirborne();
        }

        ChkWallSlide();

        _effectManager.ApplyEffects(rb);
    }
}

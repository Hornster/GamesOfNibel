﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Character.Skills;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{

    //--Private Variables Exposed to the Inspector.
    [SerializeField]
    private float movementSpeed;

    [SerializeField] private float _airborneAccelerationFactor = 0.4f;

    [SerializeField] private SkillsController _skillsController;


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
        _jumpVelocity += _playerState.GravityManager.GetBaseJumpStartVelocity();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        _playerPhysics = GetComponent<PlayerPhysics>();
        _playerState = GetComponent<PlayerState>();

        InputReader.RegisterJumpHandler(Jump);
        InputReader.RegisterGlideHandler(Glide);

        _playerState.colliderSize = cc.size;

        CalcJumpVelocity();
    }

    private void Update()
    {
        CheckInput();
        RotatePlayer();
    }

    private void ChkWallSlide()
    {
        _skillsController.UseSkill(SkillType.WallSlide);
    }

    private void FixedUpdate()
    {
        _playerPhysics.CheckCollisions();
        ApplyMovement();
    }
    
    private void CheckInput()
    {
        var rawInput = InputReader.InputRaw;
        _playerState.xInput = rawInput.X;
    }

    private void RotatePlayer()
    {
        if (ValueComparator.IsEqual(_playerState.xInput, -_playerState.facingDirection))
        {
            Flip();
        }
    }
    
    private void Jump()
    {
        if (_playerState.canJump)
        {
            _playerState.canJump = false;
            _playerState.isJumping = true;

            _playerState.newVelocity = new Vector2(rb.velocity.x, _jumpVelocity); 
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
        switch (glideStage)
        {
            case GlideStages.GlideBegin:
                _playerState.IsGliding = true;
                break;
            case GlideStages.GlideKeep:
                _skillsController.UseSkill(SkillType.Glide);
                break;
            case GlideStages.GlideStop:
                _playerState.IsGliding = false;
                break;
        }
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

    private void ApplyMovement()
    {
        //rb.velocity = new Vector2(0.0f, 0.0f);
        //return;
        //TODO Do it like this - make a switch. When isGround == false - switch = false.
        //TODO When IsDeeplyGrounded == true - switch = true. Player keeps falling UNTIL THIS SWITCH IS FREAKING TRUE.
        if (_playerState.isGrounded && !_playerState.isOnSlope && !_playerState.isJumping)
        {
            var newVelocity = ChkHowCloseToGround(new Vector2(movementSpeed * _playerState.xInput, _playerState.newVelocity.y));
            //On flat ground
            _playerState.newVelocity = newVelocity;
            rb.velocity = newVelocity;
        }
        else if (_playerState.isGrounded && _playerState.isOnSlope && !_playerState.isJumping && _playerState.canWalkOnSlope)
        {
            //On slope
            //-xInput since the normal is rotated counterclockwise
            if (_playerState.IsStandingOnGround)
            {
                _playerState.newVelocity = new Vector2(movementSpeed * _playerState.slopeNormalPerp.x * -_playerState.xInput, movementSpeed * _playerState.slopeNormalPerp.y * -_playerState.xInput);
                rb.velocity = _playerState.newVelocity;
            }
        }
        else if (!_playerState.isGrounded)
        {
            var currentVelocity = rb.velocity;
            currentVelocity.x += movementSpeed * _playerState.xInput * _airborneAccelerationFactor;

            if (Mathf.Abs(currentVelocity.x) > movementSpeed)
            {
                currentVelocity.x = Mathf.Sign(currentVelocity.x) * movementSpeed;
            }
            //In the air
            _playerState.newVelocity = currentVelocity;
            rb.velocity = _playerState.newVelocity;
        }

        ChkWallSlide();
    }

    private void Flip()
    {
        _playerState.facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }


}

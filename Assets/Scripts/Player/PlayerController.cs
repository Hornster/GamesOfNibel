using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private float jumpForce;

    [SerializeField] private float _airborneAccelerationFactor = 0.4f;

    [SerializeField] private SkillsController _skillsController;


    //--Component References
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private PlayerState _playerState;
    private PlayerPhysics _playerPhysics;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        _playerPhysics = GetComponent<PlayerPhysics>();
        _playerState = GetComponent<PlayerState>();

        InputReader.RegisterJumpHandler(Jump);

        _playerState.colliderSize = cc.size;
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

            _playerState.newVelocity = new Vector2(0.0f, 0.0f);
            rb.velocity = _playerState.newVelocity;
            _playerState.newForce = new Vector2(0.0f, jumpForce);
            rb.AddForce(_playerState.newForce, ForceMode2D.Impulse);
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
    
    private void ApplyMovement()
    {
        if (_playerState.isGrounded && !_playerState.isOnSlope && !_playerState.isJumping)
        {
            //On flat ground
            _playerState.newVelocity = new Vector2(movementSpeed * _playerState.xInput, 0.0f);
            rb.velocity = _playerState.newVelocity;
        }
        else if (_playerState.isGrounded && _playerState.isOnSlope && !_playerState.isJumping && _playerState.canWalkOnSlope)
        {
            //On slope
            //-xInput since the normal is rotated counterclockwise
            _playerState.newVelocity = new Vector2(movementSpeed * _playerState.slopeNormalPerp.x * -_playerState.xInput, movementSpeed * _playerState.slopeNormalPerp.y * -_playerState.xInput);
            rb.velocity = _playerState.newVelocity;
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

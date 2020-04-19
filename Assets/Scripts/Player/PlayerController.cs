using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{

    //--Private Variables Exposed to the Inspector.
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;


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
        if (_playerState.xInput == -_playerState.facingDirection)
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
            //In the air
            _playerState.newVelocity = new Vector2(movementSpeed * _playerState.xInput, rb.velocity.y);
            rb.velocity = _playerState.newVelocity;
        }
    }

    private void Flip()
    {
        _playerState.facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }


}

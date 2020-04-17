using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //--Private Variables Exposed to the Inspector.
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFritcion;

    //--Private Variables

    //Perpendicular

    //--Component References
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private readonly PlayerState _playerState = new PlayerState();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        _playerState.colliderSize = cc.size;
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
    }

    private void SlopeCheck()
    {
        float scaledColliderSize = (_playerState.colliderSize.y * transform.localScale.y);
        Vector2 checkPos = transform.position - new Vector3(0, scaledColliderSize / 2, 0);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);

    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        var slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        var slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            _playerState.isOnSlope = true;
            _playerState.slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            _playerState.isOnSlope = true;
            _playerState.slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            _playerState.slopeSideAngle = 0.0f;
            _playerState.isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        var hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);
        Debug.DrawRay(checkPos, Vector2.down * slopeCheckDistance, Color.black);
        if (hit)
        {
            _playerState.slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            _playerState.slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (_playerState.slopeDownAngle != _playerState.slopeDownAngleOld)
            {
                _playerState.isOnSlope = true;
            }

            _playerState.slopeDownAngleOld = _playerState.slopeDownAngle;

            Debug.DrawRay(hit.point, _playerState.slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
        }

        if (_playerState.slopeDownAngle > maxSlopeAngle || _playerState.slopeSideAngle > maxSlopeAngle)
        {
            _playerState.canWalkOnSlope = false;
        }
        else
        {
            _playerState.canWalkOnSlope = true;
        }

        if (_playerState.isOnSlope && _playerState.xInput == 0.0f && _playerState.canWalkOnSlope)
        {
            rb.sharedMaterial = fullFritcion;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }

    private void CheckInput()
    {
        _playerState.xInput = Input.GetAxisRaw("Horizontal");

        if (_playerState.xInput == -_playerState.facingDirection)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void CheckGround()
    {
        _playerState.isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rb.velocity.y <= 0.0f)
        {
            _playerState.isJumping = false;
        }
        if (_playerState.isGrounded && !_playerState.isJumping && _playerState.slopeDownAngle <= maxSlopeAngle)
        {
            _playerState.canJump = true;
        }
        else
        {
            _playerState.canJump = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerPhysics : MonoBehaviour
    {

        [SerializeField]
        private float slopeCheckDistance;
        [SerializeField]
        private float maxSlopeAngle;
        [SerializeField]
        private float groundCheckRadius;

        /// <summary>
        /// Defines where will the collision and slope checking rays be coming from.
        /// </summary>
        [SerializeField]
        private Transform groundCheck;

        /// <summary>
        /// What is treated by the character as collidable ground?
        /// </summary>
        [SerializeField]
        private LayerMask whatIsGround;

        //Component references
        [SerializeField]
        private PhysicsMaterial2D noFriction;
        [SerializeField]
        private PhysicsMaterial2D fullFritcion;
        private Rigidbody2D rb;
        private PlayerState _playerState;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerState = GetComponent<PlayerState>();
        }

        public void CheckCollisions()
        {
            CheckGround();
            SlopeCheck();
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
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

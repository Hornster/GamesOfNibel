﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player.Character;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerPhysics : MonoBehaviour
    {
        [SerializeField]
        private float _verticalSlopeCheckDistance;
        [SerializeField]
        private float _horizontalSlopeCheckDistance;
        [SerializeField]
        private float maxSlopeAngle;
        [SerializeField]
        private float groundCheckRadius;
        [SerializeField]
        private float wallCheckDistance;
        /// <summary>
        /// Max angle of the wall towards horizontal ground that the player can climb on (or perform a wall jump).
        /// </summary>
        [SerializeField]
        private float _maxClimbableAngle;

        [SerializeField] private Vector2 _closeGroundCheckSize;

        /// <summary>
        /// Defines where will the collision and slope checking rays be coming from.
        /// </summary>
        [SerializeField]
        private Transform groundCheck;
        /// <summary>
        /// Used to test how close to the ground the character is. If they are close enough,
        /// the Y velocity shall be set to 0.
        /// </summary>
        [SerializeField] private Transform _groundCloseCheck;
        /// <summary>
        /// Start position for additional ray that checks if the character touches a long enough part of wall for
        /// climbing/wall jumping.
        /// </summary>
        [SerializeField] private Transform wallCheck;

        

        //Component referencesddd
        [SerializeField]
        private PhysicsMaterial2D noFriction;
        [SerializeField]
        private PhysicsMaterial2D fullFritcion;

        [SerializeField] private CharacterRotator _characterRotator;
        [SerializeField] private CollisionMasksManager _collisionMaskManager;
        private Rigidbody2D rb;
        private PlayerState _playerState;
        /// <summary>
        /// Set to true when there's an unclimbable slope detected to the front of the character.
        /// </summary>
        private bool _unclimbableSlopeOnFront;
        /// <summary>
        /// Set to true when there's an unclimbable slope detected to the back of the character.
        /// </summary>
        private bool _unclimbableSlopeOnBack;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerState = GetComponent<PlayerState>();
        }

        

        public void CheckCollisions()
        {
            _unclimbableSlopeOnFront = false;
            _unclimbableSlopeOnBack = false;

            CheckGround();
            SlopeCheck();
            WallCheck();
            ChkEdgeCases();
        }

        private void CheckGround()
        {
            _playerState.isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, _collisionMaskManager.WhatIsGround);
            bool isCloseToGround = Physics2D.OverlapBox(_groundCloseCheck.position, _closeGroundCheckSize, 0.0f, _collisionMaskManager.WhatIsGround);

            if (_playerState.isGrounded == false)
            {
                _playerState.CharacterStoppedTouchingTheGround();
            }

            if (isCloseToGround)
            {
                _playerState.IsStandingOnGround = true;
            }

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
            float scaledColliderSize = (_playerState.ColliderSize.y * transform.localScale.y);
            Vector2 checkPos = transform.position - new Vector3(0, scaledColliderSize / 2, 0);

            SlopeCheckHorizontal(checkPos);
            SlopeCheckVertical(checkPos);

        }

        private void SlopeCheckHorizontal(Vector2 checkPos)
        {
            var slopeHitFront = Physics2D.Raycast(checkPos, transform.right, _horizontalSlopeCheckDistance, _collisionMaskManager.WhatIsGround);
            var slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, _horizontalSlopeCheckDistance, _collisionMaskManager.WhatIsGround);

            if (slopeHitBack)
            {
                _unclimbableSlopeOnBack = true;
            }

            if (slopeHitFront)
            {
                _unclimbableSlopeOnFront = true;
                _playerState.isOnSlope = true;
                _playerState.slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
                _playerState.SlopeHorizontalNormal = slopeHitFront.normal;
            }
            else if (slopeHitBack)
            {
                _playerState.isOnSlope = true;
                _playerState.slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
                _playerState.SlopeHorizontalNormal = slopeHitBack.normal;
            }
            else
            {
                _playerState.slopeSideAngle = 0.0f;
                _playerState.isOnSlope = false;
                _playerState.SlopeHorizontalNormal = Vector2.zero;
            }

        }
        /// <summary>
        /// Checks for the slope right beneath the player.
        /// </summary>
        /// <param name="checkPos">Position which the testing ray will be cast from.</param>
        private void SlopeCheckVertical(Vector2 checkPos)
        {
            var hit = Physics2D.Raycast(checkPos, Vector2.down, _verticalSlopeCheckDistance, _collisionMaskManager.WhatIsGround);
            Debug.DrawRay(checkPos, Vector2.down * _verticalSlopeCheckDistance, Color.black);
            if (hit)
            {
                _playerState.SlopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

                _playerState.slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (ValueComparator.IsEqual(_playerState.slopeDownAngle, _playerState.slopeDownAngleOld) == false)
                {
                    _playerState.isOnSlope = true;
                }

                _playerState.slopeDownAngleOld = _playerState.slopeDownAngle;

                Debug.DrawRay(hit.point, _playerState.SlopeNormalPerp, Color.red);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            }

            if (_playerState.slopeDownAngle > maxSlopeAngle || _playerState.slopeSideAngle > maxSlopeAngle)
            {
                _playerState.canWalkOnSlope = false;
            }
            else
            {
                _playerState.canWalkOnSlope = true;
                if (_playerState.IsStandingOnGround == false)
                {
                    _playerState.isJumping = false;
                }
            }

            if (_playerState.isOnSlope && ValueComparator.IsEqual(_playerState.xInput, 0.0f) && _playerState.canWalkOnSlope)
            {
                rb.sharedMaterial = fullFritcion;
            }
            else
            {
                rb.sharedMaterial = noFriction;
            }
        }
        /// <summary>
        /// Checks if the character is holding a wall.
        /// </summary>
        private void WallCheck()
        {
            //The character is rotating, so left side in the code would become right side.
            //Basically check for wall presence from both sides of the character...
            bool isWallCloseFromRightSide = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, _collisionMaskManager.WallCollisionLayers);
            bool isWallCloseFromLeftSide = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, _collisionMaskManager.WallCollisionLayers);
            //...and take a logic sum of both. If at least one side is close to wall - character's
            //touching a wall.
            _playerState.IsTouchingWall = isWallCloseFromRightSide || isWallCloseFromLeftSide;

            _characterRotator.TurnCharacterToWall(isWallCloseFromLeftSide, isWallCloseFromRightSide);

            if (_playerState.IsTouchingWall && _playerState.isGrounded == false && rb.velocity.y < 0)
            {
                _playerState.IsWallSliding = true;
            }
            else
            {
                _playerState.IsWallSliding = false;
            }
        }
        /// <summary>
        /// Checks for these pesky edge cases that happen rarely but when they do they break the gameplay, most likely.
        /// </summary>
        private void ChkEdgeCases()
        {
            //Check if the character is hanging in between two unclimbable slopes that are too short for a wall jump.
            if (_playerState.isGrounded && _unclimbableSlopeOnBack && _unclimbableSlopeOnFront)
            {
                _playerState.canJump = true;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawWireCube((Vector2)_groundCloseCheck.position, _closeGroundCheckSize);

            var wallLineDest = new Vector3(wallCheck.position.x + transform.right.x * wallCheckDistance, wallCheck.position.y, wallCheck.position.z);
            Gizmos.DrawLine(wallCheck.position, wallLineDest);
            
            Vector2 checkPos = transform.position;
            var slopeCheckDest = checkPos + (Vector2)transform.right * _horizontalSlopeCheckDistance;
            Gizmos.DrawLine(checkPos, slopeCheckDest);
        }
        
    }
}
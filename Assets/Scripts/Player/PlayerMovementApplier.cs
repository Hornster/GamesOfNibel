﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Helpers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovementApplier : MonoBehaviour
    {
        /// <summary>
        /// Time in seconds it takes the player to reach max velocity on grounds.
        /// </summary>
        [Header("Config values")]
        [SerializeField]
        private float _movementSpeed;
        [SerializeField] private float _accelerationTime = 0.5f;
        /// <summary>
        /// How long should it take for the player in the air to fully accelerate? In seconds.
        /// </summary>
        [SerializeField] private float _accelerationTimeAirborne = 0.4f;
        /// <summary>
        /// The time it takes for the character to stop horizontally while airborne from
        /// their max horizontal (running) velocity.
        /// </summary>
        [SerializeField] private float _decelerationTimeAirborne = 0.7f;
        /// <summary>
        /// The state of the player.
        /// </summary>
        [Header("Required references")]
        [SerializeField]
        private PlayerState _playerState;
        [SerializeField]
        private Rigidbody2D rb;

        /// <summary>
        /// The old direction which the player was moving in.
        /// </summary>
        private int _oldMovementDirection;
        /// <summary>
        /// Defined by division of _movementSpeed/_decelerationTimeAirborne.
        /// Used on character while they are airborne to make the gradually halt on the X axis.
        /// </summary>
        private float _airborneDeceleration;
        /// <summary>
        /// Defined by division of _movementSpeed/_accelerationTimeAirborne.
        /// Used on character while they are airborne to make the gradually halt on the X axis.
        /// </summary>
        private float _airborneAcceleration;
        /// <summary>
        /// The time which the player was running for.
        /// </summary>
        private float _runningTime = 0f;

        private void Start()
        {
            _oldMovementDirection = _playerState.facingDirection;
            CalcAirborneVelChangeValues();
        }
        private void CalcAirborneVelChangeValues()
        {
            _airborneDeceleration = _movementSpeed / _decelerationTimeAirborne;
            _airborneAcceleration = _movementSpeed / _accelerationTimeAirborne;
        }
        /// <summary>
        /// Returns the current player running velocity accordingly to the acceleration time.
        /// </summary>
        private float AccelerateOnGround(float lastFrameTime)
        {
            _runningTime = _runningTime * _accelerationTime + lastFrameTime;
            return Mathf.Lerp(0.0f, _movementSpeed, _runningTime / _accelerationTime);
        }
        /// <summary>
        /// Returns the current player running velocity accordingly to the acceleration time.
        /// </summary>
        private float DecelerateAirborne(float lastFrameTime)
        {
            _runningTime = _runningTime * _decelerationTimeAirborne - lastFrameTime;
            return Mathf.Lerp(0.0f, _movementSpeed, _runningTime / _decelerationTimeAirborne);
        }
        /// <summary>
        /// Returns the current player running velocity accordingly to the acceleration time.
        /// </summary>
        private float AccelerateAirborne(float lastFrameTime)
        {
            _runningTime = _runningTime * _accelerationTimeAirborne + lastFrameTime;
            return Mathf.Lerp(0.0f, _movementSpeed, _runningTime / _accelerationTimeAirborne);
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

        /// <summary>
        /// Influences horizontal velocity of the player when they are in the air.
        /// </summary>
        /// <param name="lastFrameTime"></param>
        private void CorrectXVelocityWhileAirborne(float lastFrameTime)
        {
            var currentVelocity = rb.velocity;
            var xInputSign = Mathf.Sign(_playerState.xInput);
            var velocitySign = Mathf.Sign(currentVelocity.x);

            if (ValueComparator.IsEqual(_playerState.xInput, 0.0f) ||
                (ValueComparator.IsEqual(xInputSign, velocitySign) == false &&
                 ValueComparator.IsEqual(currentVelocity.x, 0.0f) == false))
            {
                float velSign = Mathf.Sign(currentVelocity.x);
                currentVelocity.x = DecelerateAirborne(lastFrameTime) * velSign;
            }
            else
            {
                float newVelocityHorizontal = AccelerateAirborne(lastFrameTime);
                currentVelocity.x = newVelocityHorizontal * _playerState.xInput;
            }


            if (Mathf.Abs(currentVelocity.x) > _movementSpeed)
            {
                currentVelocity.x = Mathf.Sign(currentVelocity.x) * _movementSpeed;
            }
            //In the air
            _playerState.NewVelocity = currentVelocity;
            rb.velocity = _playerState.NewVelocity;
        }
        public void ApplyMovement(float lastFrameTime)
        {
            _runningTime = Mathf.Abs(rb.velocity.x / _movementSpeed);

            if (_playerState.isGrounded && !_playerState.isOnSlope && !_playerState.isJumping)
            {
                float horizontalVelocity = AccelerateOnGround(lastFrameTime);
                var newVelocity = ChkHowCloseToGround(new Vector2(horizontalVelocity * _playerState.xInput, _playerState.NewVelocity.y));
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
                    float horizontalVelocity = AccelerateOnGround(lastFrameTime);
                    _playerState.NewVelocity = new Vector2(horizontalVelocity * _playerState.SlopeNormalPerp.x * -_playerState.xInput, _movementSpeed * _playerState.SlopeNormalPerp.y * -_playerState.xInput);
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
                        float horizontalVelocity = AccelerateOnGround(lastFrameTime);
                        //Player wants to move away from the slope, that is acceptable.
                        _playerState.NewVelocity = new Vector2(rb.velocity.x + horizontalVelocity * _playerState.xInput, rb.velocity.y);
                        rb.velocity = _playerState.NewVelocity;
                    }
                }
            }
            else if (!_playerState.isGrounded)
            {
                CorrectXVelocityWhileAirborne(lastFrameTime);
            }
        }
    }
}

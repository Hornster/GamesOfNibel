using System;
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
        /// The time which the player was running for.
        /// </summary>
        [SerializeField] private float _runningTime = 0f;
        /// <summary>
        /// The time it takes for the character to stop horizontally while airborne from
        /// their max horizontal (running) velocity.
        /// </summary>
        [SerializeField] private float _timeToStopAirborne = 0.7f;
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
        /// Defined by division of _movementSpeed/_timeToStopAirborne.
        /// Used on character while they are airborne to make the gradually halt on the X axis.
        /// </summary>
        private float _airborneDeceleration;

        private void Start()
        {
            _oldMovementDirection = _playerState.facingDirection;
            CalcAirborneDeceleration();
        }
        private void CalcAirborneDeceleration()
        {
            _airborneDeceleration = _movementSpeed / _timeToStopAirborne;
        }
        /// <summary>
        /// Resets the ground acceleration time measurement.
        /// </summary>
        private void ResetGroundAcceleration()
        {
            _oldMovementDirection = _playerState.facingDirection;
            _runningTime = 0.0f;
        }
        /// <summary>
        /// Returns the current player running velocity accordingly to the acceleration time.
        /// </summary>
        private float AccelerateOnGround()
        {
            return Mathf.Lerp(0.0f, _movementSpeed, _runningTime/_accelerationTime);
        }
        /// <summary>
        /// Updates the ground acceleration.
        /// </summary>
        /// <param name="lastFrameTime">Time it took to complete last frame. Will be added to the acceleration time.</param>
        private void UpdateGroundAccelerationTime(float lastFrameTime)
        {
            if (_playerState.facingDirection != _oldMovementDirection || ValueComparator.IsEqual(_playerState.xInput, 0.0f))
            {
                ResetGroundAcceleration();
            }

            _runningTime += lastFrameTime;
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
                currentVelocity.x += _movementSpeed * _playerState.xInput * _airborneAccelerationFactor;
            }


            if (Mathf.Abs(currentVelocity.x) > _movementSpeed)
            {
                currentVelocity.x = Mathf.Sign(currentVelocity.x) * _movementSpeed;
            }
            //In the air
            _playerState.NewVelocity = currentVelocity;
            rb.velocity = _playerState.NewVelocity;
        }
        /// <summary>
        /// Applies movement to the player.
        /// </summary>
        /// <param name="lastFrameTime">Time of the last frame.</param>
        public void ApplyMovement(float lastFrameTime)
        {
            UpdateGroundAccelerationTime(lastFrameTime);

            if (_playerState.isGrounded && !_playerState.isOnSlope && !_playerState.isJumping)
            {
                float horizontalVelocity = AccelerateOnGround();
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
                    float horizontalVelocity = AccelerateOnGround();
                    _playerState.NewVelocity = new Vector2(horizontalVelocity * _playerState.SlopeNormalPerp.x * -_playerState.xInput, _movementSpeed * _playerState.SlopeNormalPerp.y * -_playerState.xInput);
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
                        float horizontalVelocity = AccelerateOnGround();
                        //Player wants to move away from the slope, that is acceptable.
                        _playerState.NewVelocity = new Vector2(rb.velocity.x + horizontalVelocity * _playerState.xInput, rb.velocity.y);
                        rb.velocity = _playerState.NewVelocity;
                    }
                }
            }
            else if (!_playerState.isGrounded)
            {
                CorrectXVelocityWhileAirborne();
            }
        }

    }
}

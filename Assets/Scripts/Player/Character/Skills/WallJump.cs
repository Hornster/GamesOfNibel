using Assets.Scripts.Common.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Character.Skills
{
    public class WallJump : MonoBehaviour, IBasicSkill
    {

        [SerializeField] private UnityEvent _wallJumpUnavailable;
        [SerializeField] private UnityEvent _wallJumpAvailable;

        private Vector2 _wallJumpDirection;
        private float _wallJumpVelocity;

        [Range(1, 90)]
        [SerializeField] private float _jumpAngle;
        [SerializeField] private float _jumpHeight;

        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        [SerializeField] //TODO remove serialization, debug feature.
        private Rigidbody2D _characterRigidbody;

        /// <summary>
        /// Uses this skill.
        /// </summary>
        [SerializeField] //TODO remove serialization, debug feature.
        private PlayerState _playerState;

        private void CalculateJumpStartVelocity()
        {
            float angleInRadians = Mathf.Deg2Rad * _jumpAngle;
            float angleSin = Mathf.Sin(angleInRadians);
            float jumpTime = _playerState.GravityManager.GetBaseJumpTime();
            _wallJumpDirection = new Vector2(Mathf.Cos(angleInRadians), angleSin);
            _wallJumpVelocity = _jumpHeight / jumpTime;
            _wallJumpVelocity += 0.5f* _playerState.GravityManager.GetRefGravityValue() * jumpTime;
            _wallJumpVelocity /= _wallJumpDirection.y;
        }
        private void Start()
        {
            _wallJumpDirection.Normalize();
            CalculateJumpStartVelocity();
        }
        //todo DEBUG
        private void ChkSkillAvailablilty()
        {
            if (_playerState.IsTouchingWall && (!_playerState.isGrounded || !_playerState.canWalkOnSlope))
            {
                _wallJumpAvailable?.Invoke();
            }
            else
            {
                _wallJumpUnavailable?.Invoke();
            }
        }
        private void Update()
        {
            ChkSkillAvailablilty();
        }

        public void UseSkill()
        {
            //wall hop
            if (_playerState.IsTouchingWall //If the player is holding a climbable wall...
                || (_playerState.isGrounded && _playerState.canWalkOnSlope == false)) //...or is on too steep slope to walk on.
            {
                _characterRigidbody.velocity = Vector2.zero;
                //Since the horizontal velocity is being assigned directly, we need to assign
                //it this way here, too.
                float newYVelocity = _wallJumpDirection.y * _wallJumpVelocity;
                float newXVelocity = _wallJumpDirection.x * _wallJumpVelocity;
                newXVelocity *= (-_playerState.facingDirection);

                _characterRigidbody.velocity = new Vector2(newXVelocity, newYVelocity);

                _playerState.isJumping = true;
            }
        }
    }
}
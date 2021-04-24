using Assets.Scripts.Game.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Player.Character.Skills
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
            var instance = GlobalGravityManager.Instance;
            float angleInRadians = Mathf.Deg2Rad * _jumpAngle;
            float angleSin = Mathf.Sin(angleInRadians);
            float jumpTime = instance.GetBaseJumpTime();
            _wallJumpDirection = new Vector2(Mathf.Cos(angleInRadians), angleSin);
            _wallJumpVelocity = _jumpHeight / jumpTime;
            _wallJumpVelocity += 0.5f* instance.GetBaseGravityValue() * jumpTime;
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
        /// <summary>
        /// Performs the jump.
        /// </summary>
        /// <param name="facingDirection">Which way is the character facing?</param>
        private void PerformWallJump(int facingDirection)
        {
            _characterRigidbody.velocity = Vector2.zero;
            //Since the horizontal velocity is being assigned directly, we need to assign
            //it this way here, too.
            float newYVelocity = _wallJumpDirection.y * _wallJumpVelocity;
            float newXVelocity = _wallJumpDirection.x * _wallJumpVelocity;
            newXVelocity *= facingDirection;

            _characterRigidbody.velocity = new Vector2(newXVelocity, newYVelocity);

            _playerState.PlayerWallJumps();
        }
        /// <summary>
        /// Sets the rigidbody to provided one.
        /// </summary>
        /// <param name="rigidbody"></param>
        public void SetRigidBody(Rigidbody2D rigidbody)
        {
            _characterRigidbody = rigidbody;
        }
        /// <summary>
        /// Sets the player state to provided one.
        /// </summary>
        /// <param name="playerState"></param>
        public void SetPlayerState(PlayerState playerState)
        {
            _playerState = playerState;
        }
        public void UseSkill()
        {
            //wall hop
            if (_playerState.IsTouchingWall //If the player is holding a climbable wall...
                || (_playerState.isGrounded && _playerState.canWalkOnSlope == false))//...or is on too steep slope to walk on...
            {
                PerformWallJump(-_playerState.facingDirection);
            }
            else if (_playerState.IsTouchingWallRemembered)//...or we were touching the wall just a moment ago. 
            {
                PerformWallJump(_playerState.facingDirection);
            }
        }
    }
}
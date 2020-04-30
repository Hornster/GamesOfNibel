using Assets.Scripts.Common.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Character.Skills
{
    public class WallJump : MonoBehaviour, IBasicSkill
    {

        [SerializeField] private UnityEvent _wallJumpUnavailable;
        [SerializeField] private UnityEvent _wallJumpAvailable;

        [SerializeField] private Vector2 _wallJumpDirection;
        [SerializeField] private float _wallJumpForce;


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

        private void Start()
        {
            _wallJumpDirection.Normalize();
        }
        //todo DEBUG
        private void ChkSkillAvailablilty()
        {
            if (_playerState.IsTouchingWall && !_playerState.isGrounded)
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
                float newYForce = _wallJumpDirection.y * _wallJumpForce;
                float newXVelocity = _wallJumpDirection.x * _wallJumpForce;
                newXVelocity = newXVelocity / _characterRigidbody.mass;
                newXVelocity *= (-_playerState.facingDirection);

                _characterRigidbody.AddForce(new Vector2(0, newYForce), ForceMode2D.Impulse);
                _characterRigidbody.velocity += new Vector2(newXVelocity, 0);

                _playerState.isJumping = true;
            }
        }
    }
}
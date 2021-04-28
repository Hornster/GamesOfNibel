using Assets.Scripts.Game.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Player.Character.Skills
{
    /// <summary>
    /// Defines the double jump skill behavior.
    /// </summary>
    public class DoubleJump : MonoBehaviour, IBasicSkill
    {
        /// <summary>
        /// Defines the force applied to character during double jump.
        /// </summary>
        private float _initialVelocity;
        [SerializeField] private float _jumpHeight = 3.0f;

        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        //TODO Debug feature to show skill usage.
        [SerializeField] private UnityEvent _skillUsed;

        [SerializeField]//TODO remove serialization, debug feature.
        private Rigidbody2D _characterRigidbody;

        /// <summary>
        /// Uses this skill.
        /// </summary>
        [SerializeField]//TODO remove serialization, debug feature.
        private PlayerState _playerState;

        private void Start()
        {
            _initialVelocity = CalculateJumpVelocity(_jumpHeight);
        }

        public void SetJumpHeight(float jumpHeight)
        {
            _jumpHeight = jumpHeight;
            _initialVelocity = CalculateJumpVelocity(_jumpHeight);
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
        /// <summary>
        /// Performs double jump.
        /// </summary>
        public void UseSkill()
        {
            if (_playerState.isGrounded==false && _playerState.CanDoubleJump && _playerState.IsTouchingWall == false)
            {
                //1.0f since double jump always pushes the character up.
                var dirVector = new Vector2(_playerState.xInput, 1.0f);
                _characterRigidbody.velocity = dirVector*_initialVelocity;

                _playerState.CanDoubleJump = false;

                //todo DEBUG
                _skillUsed?.Invoke();
            }
        }
        /// <summary>
        /// Calculates and returns jump velocity, accordingly to provided jump height.
        /// </summary>
        /// <param name="jumpHeight">How high the jump is relatively to starting position</param>
        /// <returns></returns>
        private float CalculateJumpVelocity(float jumpHeight)
        {
            var instance = GlobalGravityManager.Instance;
            float gravity = instance.GetBaseGravityValue();
            float jumpTime = instance.GetBaseJumpTime();

            return jumpHeight / jumpTime + 0.5f * gravity * jumpTime;
        }
    }
}

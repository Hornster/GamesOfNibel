using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Character.Skills
{
    /// <summary>
    /// Defines the double jump skill behavior.
    /// </summary>
    public class DoubleJump : MonoBehaviour, IBasicSkill
    {
        /// <summary>
        /// Defines the force applied to character during double jump.
        /// </summary>
        [SerializeField] private float _force = 20.0f;

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
        /// <summary>
        /// Performs double jump.
        /// </summary>
        public void UseSkill()
        {
            if (_playerState.isGrounded==false && _playerState.CanDoubleJump && _playerState.IsTouchingWall == false)
            {
                _characterRigidbody.velocity = Vector2.zero;
                //1.0f since double jump always pushes the character up.
                var dirVector = new Vector2(_playerState.xInput, 1.0f);
                _characterRigidbody.AddForce(dirVector*_force, ForceMode2D.Impulse);

                _playerState.CanDoubleJump = false;
                _playerState.IsGliding = false;

                //todo DEBUG
                _skillUsed?.Invoke();
            }
        }
    }
}

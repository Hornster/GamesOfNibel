using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Character.Skills
{
    /// <summary>
    /// Gives the characters the gliding ability. Usage of this skill
    /// does not reset other skills.
    /// </summary>
    public class Glide : MonoBehaviour, IBasicSkill
    {
        /// <summary>
        /// How fast the player descends while using the ability.
        /// </summary>
        [SerializeField] private float _maxFallingSpeed = 1.0f;

        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        //TODO Debug feature to show skill usage.
        [SerializeField] private UnityEvent _skillUsed;
        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        //TODO Debug feature to show skill usage.
        [SerializeField] private UnityEvent _skillReset;
        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        //TODO Debug feature to show skill usage.
        [SerializeField] private UnityEvent _skillUnavailable;
        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        //TODO Debug feature to show skill usage.
        [SerializeField] private UnityEvent _skillAvailable;



        [SerializeField]//TODO remove serialization, debug feature.
        private Rigidbody2D _characterRigidbody;
        /// <summary>
        /// Uses this skill.
        /// </summary>
        [SerializeField]//TODO remove serialization, debug feature.
        private PlayerState _playerState;

        private void FixedUpdate()
        {
            //todo DEBUG
            if (_playerState.isGrounded || _playerState.IsTouchingWall)
            {
                _skillUnavailable?.Invoke();
            }
            else if (!_playerState.isGrounded && !_playerState.IsTouchingWall)
            {
                _skillAvailable?.Invoke();
                if (!_playerState.IsGliding)
                {
                    _skillReset?.Invoke();
                }
            }
        }

        public void UseSkill()
        {
            //This skill can be used only when the player is airborne.
            if (!_playerState.isGrounded && !_playerState.IsTouchingWall && _playerState.IsGliding)
            {
                if (Mathf.Abs(_characterRigidbody.velocity.y) > _maxFallingSpeed)
                {
                    float newVelocityY = _maxFallingSpeed * Mathf.Sign(_characterRigidbody.velocity.y);
                    _characterRigidbody.velocity = new Vector2(_characterRigidbody.velocity.x, newVelocityY);
                }

                _skillUsed?.Invoke();
            }
        }
    }
}

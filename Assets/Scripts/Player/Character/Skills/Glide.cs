using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Player.Gravity.Constraints;
using UnityEditor;
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

        private void Start()
        {
            var maxVelocityConstraint = new GlideVelocityConstraint(_playerState, _maxFallingSpeed);
            _playerState.LocalGravityManager.ApplyMaxVelocityConstraint(maxVelocityConstraint);
        }

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
            var currentGlideState = _playerState.GlideStage;

            if (_playerState.isGrounded)
            {
                _playerState.IsGliding = false;
            }
            else
            {
                switch (currentGlideState)
                {
                    //We do not take into account the GlideStages.Keep case.
                    //For example, if the player double jumps, the gliding shall
                    //be stopped until the player let's go the glide button
                    //and presses it again.
                    case GlideStages.GlideBegin:
                        _playerState.IsGliding = true;
                        break;
                    case GlideStages.GlideStop:
                        _playerState.IsGliding = false;
                        break;
                }
            }
            
            //This skill can be used only when the player is airborne.
            if (!_playerState.isGrounded && !_playerState.IsTouchingWall && _playerState.IsGliding)
            {
                _skillUsed?.Invoke();
            }
        }
    }
}

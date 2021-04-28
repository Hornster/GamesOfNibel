using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Player.Gravity.Constraints;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Character.Skills
{
    public class WallSlide : MonoBehaviour, IBasicSkill
    {
        /// <summary>
        /// Max falling velocity of the player.
        /// </summary>
        [SerializeField] private float _maxVelocityCap = 3f;
        /// <summary>
        /// Scales gravity whn sliding down a wall. 
        /// </summary>
        [SerializeField] private float _gravityScale = 0.25f;
        /// <summary>
        /// Player's rigidbody.
        /// </summary>
        [SerializeField] private Rigidbody2D _rb;//TODO remove serialize, make the ref be assigned by factory
        /// <summary>
        /// State of the player.
        /// </summary>
        [SerializeField] private PlayerState _playerState;//TODO remove serialize, make the ref be assigned by factory
        /// <summary>
        /// Which wall slide attributes shall be applied?
        /// </summary>
        private WallSlideType[] _activeWallSlideTypes;
        /// <summary>
        /// Defines what influence types the wall slide has on the player.
        /// Check WallSlideType enum for details.
        /// </summary>
        private int _influenceTypes;
        /// <summary>
        /// All known types of influences of the wall slide skill on the player.
        /// </summary>
        private IEnumerable<WallSlideType> _wallSlideTypes;
        /// <summary>
        /// Stores reference to currently active max velocity constraint.
        /// </summary>
        private IMaxVelConstraint _activeVelocityConstraint;
        /// <summary>
        /// Adds the influence type to the WallSlide skill.
        /// </summary>
        /// <param name="type"></param>
        public void AddType(WallSlideType type)
        {
            _influenceTypes = _influenceTypes | (int)type;
        }
        /// <summary>
        /// Sets the rigidbody to provided one.
        /// </summary>
        /// <param name="rigidbody"></param>
        public void SetRigidBody(Rigidbody2D rigidbody)
        {
            _rb = rigidbody;
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
        /// Sets the available wall slide types to provided ones.
        /// </summary>
        /// <param name="slideTypes"></param>
        public void SetSlideType(WallSlideType[] slideTypes)
        {
            _activeWallSlideTypes = slideTypes;
        }
        /// <summary>
        /// Applies effect to rigidbody accordingly with their type.
        /// </summary>
        /// <param name="effect"></param>
        private void ApplyEffect(WallSlideType effect)
        {
            switch (effect)
            {
                case WallSlideType.GravityDecrease:
                    if (_rb.velocity.y <= 0)
                    {
                        //Apply the effect only when the player is descending
                        _playerState.LocalGravityManager.AddOneFrameGravityModifier(_gravityScale, this.GetHashCode());  
                    }
                    break;
            }
        }
        /// <summary>
        /// If necessary, applies max velocity constraint to gravity manager.
        /// </summary>
        private void CreateMaxVelocityConstraint()
        {
            foreach (var wallSlideType in _activeWallSlideTypes)
            {
                if (wallSlideType == WallSlideType.MaxVelocityCap)
                {
                    _activeVelocityConstraint = new WallSlideVelocityConstraint(_playerState, _maxVelocityCap);
                    _playerState.LocalGravityManager.ApplyMaxVelocityConstraint(_activeVelocityConstraint);
                }
            }
        }

        private void RemoveMaxVelocityConstraint()
        {
            _playerState.LocalGravityManager.RemoveMaxVelocityConstraint(_activeVelocityConstraint);
            _activeVelocityConstraint = null;
        }
        private void Start()
        {
            _wallSlideTypes = Enum.GetValues(typeof(WallSlideType)).Cast<WallSlideType>();
            foreach (var type in _activeWallSlideTypes)
            {
                AddType(type);
            }

            RemoveMaxVelocityConstraint();
            CreateMaxVelocityConstraint();
        }

        public void UseSkill()
        {
            if (_playerState.IsWallSliding || (_playerState.isOnSlope && _playerState.canWalkOnSlope == false))
            {
                foreach (var type in _activeWallSlideTypes)
                {
                    if (((int)type & _influenceTypes) != 0)
                    {
                        ApplyEffect(type);
                    }
                }
            }
            
        }

        public void SetMaxVelocity(float maxVelocity)
        {
            _maxVelocityCap = maxVelocity;
            RemoveMaxVelocityConstraint();
            CreateMaxVelocityConstraint();
        }

        public void SetGravityScale(float wallSlideGravityScale)
        {
            _gravityScale = wallSlideGravityScale;
        }
    }
}
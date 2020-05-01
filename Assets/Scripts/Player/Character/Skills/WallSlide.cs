﻿using Assets.Scripts.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player.Character.Skills
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
        [SerializeField] private WallSlideType[] _wallSlideType;//TODO This is debug. Remove it later on.
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
        /// Adds the influence type to the WallSlide skill.
        /// </summary>
        /// <param name="type"></param>
        public void AddType(WallSlideType type)
        {
            _influenceTypes = _influenceTypes | (int)type;
        }
        /// <summary>
        /// Applies effect to rigidbody accordingly with their type.
        /// </summary>
        /// <param name="effect"></param>
        private void ApplyEffect(WallSlideType effect)
        {
            switch (effect)
            {
                case WallSlideType.MaxVelocityCap:
                    if (Mathf.Abs(_playerState.newVelocity.y) >= _maxVelocityCap)
                    {
                        float newVelocityY = _maxVelocityCap * Mathf.Sign(_playerState.newVelocity.y);
                        _rb.velocity = new Vector2(_rb.velocity.x, newVelocityY);
                    }

                    break;
                case WallSlideType.GravityDecrease:
                    _playerState.GravityManager.AddGravityModifier(_gravityScale, this.GetHashCode());
                    break;
            }
        }

        private void Start()
        {
            _wallSlideTypes = Enum.GetValues(typeof(WallSlideType)).Cast<WallSlideType>();
            foreach (var type in _wallSlideType)
            {
                AddType(type);
            }
        }

        public void UseSkill()
        {
            if (_playerState.IsWallSliding == false)
            {
                return;
            }

            foreach (var type in _wallSlideTypes)
            {
                if (((int)type & _influenceTypes) != 0)
                {
                    ApplyEffect(type);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
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
                    
                    //TODO Create a gravity manager and connect it to player state.
                    //TODO Make the skills that influence gravity provide multipliers
                    //TODO to gravity manager, which he will apply to the gravity (chaining multipliers).
                    //TODO Each new frame, reset the gravity and calculate it again (fixedUpdate).

                    break;
                case WallSlideType.GravityDecrease:
                    break;
            }
        }

        private void Start()
        {
            _wallSlideTypes = Enum.GetValues(typeof(WallSlideType)).Cast<WallSlideType>();
        }

        public void UseSkill()
        {
            if (_playerState.IsWallSliding == false)
            {

            }

            foreach (var type in _wallSlideTypes)
            {
                if (((int) type & _influenceTypes) != 0)
                {
                    ApplyEffect(type);
                }
            }
        }
        }
    }
}
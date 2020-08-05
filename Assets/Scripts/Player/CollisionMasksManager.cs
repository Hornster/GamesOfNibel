
using Assets.Scripts.Common;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Manages collision masks.
    /// </summary>
    [RequireComponent(typeof(Timer))]
    public class CollisionMasksManager : MonoBehaviour
    {
        /// <summary>
        /// Layers that are used when the player decides to jump off the one-way platforms.
        /// </summary>
        [Header("Player Layers")]
        [SerializeField]
        private LayerMask _playerJumpsOffPlatform;
        /// <summary>
        /// Default collision layers present on player's character.
        /// </summary>
        [SerializeField]
        private LayerMask _defaultPlayer;

        /// <summary>
        /// Against which layers shall wall collisions be tested?
        /// </summary>
        [Header("Collision Detection Masks")]
        [SerializeField]
        private LayerMask _wallCollisionLayers;
        /// <summary>
        /// Layers for ground collisions resolving used by default.
        /// </summary>
        [SerializeField]
        private LayerMask _defaultGroundCollisionMask;
        /// <summary>
        /// Layers for PlayerPhysics class when the player is jumping down the platform.
        /// </summary>
        [SerializeField]
        private LayerMask _dropFromPlatformColMask;
        /// <summary>
        /// What layer defines the ground as droppable platforms?
        /// </summary>
        [SerializeField]
        private LayerMask _whatIsPlatform;
        /// <summary>
        /// The minimal time the character will not be trying to stay on any platforms after initiating the drop.
        /// In seconds.
        /// </summary>
        [Header("Parameters")]
        [SerializeField]
        private float _minimalHoldingTime = 0.5f;
        /// <summary>
        /// The object whose collision layers will be changed at runtime.
        /// </summary>
        [SerializeField] 
        private GameObject _influencedObject;
        /// <summary>
        /// What is treated by the character as collidable ground?
        /// </summary>
        private LayerMask _whatIsGround;
        /// <summary>
        /// Measures the time after initiating the dropping from platform ability.
        /// When time runs out, resets the collision masks so the player can stand on platforms again.
        /// </summary>
        private Timer _droppingTimer;
        /// <summary>
        /// Set to true when the jump down combination input from the player is active (like, buttons pressed).
        /// </summary>
        private bool _jumpDownInputActive;
        /// <summary>
        /// Used to access the _whatIsGround mask from the outside.
        /// </summary>
        public LayerMask WhatIsGround => _whatIsGround; //Auto properties are not visible in the inspector!
        /// <summary>
        /// Used to access the _wallCollisionLayers mask from the outside.
        /// </summary>
        public LayerMask WallCollisionLayers => _wallCollisionLayers; //Auto properties are not visible in the inspector!
        /// <summary>
        /// Layers that define the ground as droppable platforms.
        /// </summary>
        public LayerMask WhatIsPlatform => _whatIsPlatform; //Auto properties are not visible in the inspector!
        private void Start()
        {
            _whatIsGround = _defaultGroundCollisionMask;
            _droppingTimer = GetComponent<Timer>();
            _droppingTimer.MaxAwaitTime = _minimalHoldingTime;
            _droppingTimer.RegisterTimeoutHandler(MinimalJumpOffTimePassed);

            InputReader.RegisterJumpOffPlatformStartHandler(StartJumpingOffPlatforms);
            InputReader.RegisterJumpOffPlatformEndHandler(StopJumpingOffPlatforms);
        }
        /// <summary>
        /// Called when player starts jumping off platforms.
        /// </summary>
        private void StartJumpingOffPlatforms()
        {
            _jumpDownInputActive = true;
            ChangeCollisionScheme(GroundCheckType.JumpingOffPlatform);
        }
        /// <summary>
        /// Called when the player should stop dropping off platforms.
        /// </summary>
        private void StopJumpingOffPlatforms()
        {
            _jumpDownInputActive = false;
            ChangeCollisionScheme(GroundCheckType.Default);
        }
        /// <summary>
        /// Called by the timer when the jump off state lasts more than required minimum.
        /// </summary>
        private void MinimalJumpOffTimePassed()
        {
            ChangeCollisionScheme(GroundCheckType.Default);
        }
        /// <summary>
        /// Toggles collision scheme to provided one. If scheme not found - sets default one.
        /// </summary>
        /// <param name="newCheckType">New scheme type.</param>
        private void ChangeCollisionScheme(GroundCheckType newCheckType)
        {
            switch (newCheckType)
            {
                case GroundCheckType.Default:
                    if (_droppingTimer.IsTimeUp && _jumpDownInputActive == false)
                    {
                        _whatIsGround = _defaultGroundCollisionMask;
                        _influencedObject.layer = MathOperations.ConvertLayerMaskValueToIndex(_defaultPlayer.value);

                        _droppingTimer.Stop();
                    }
                    break;
                case GroundCheckType.JumpingOffPlatform:
                    _droppingTimer.Reset();
                    _droppingTimer.StartTimer();
                    //We need to wait for some time to allow the character to successfully drop off a platform.
                    //If the time is up - we can start using one-way platforms again.
                    _whatIsGround = _dropFromPlatformColMask;
                    _influencedObject.layer = MathOperations.ConvertLayerMaskValueToIndex(_playerJumpsOffPlatform.value);
                    break;
                default:
                    _whatIsGround = _defaultGroundCollisionMask;
                    _influencedObject.layer = MathOperations.ConvertLayerMaskValueToIndex(_defaultPlayer.value);
                    break;
            }
        }
    }
}

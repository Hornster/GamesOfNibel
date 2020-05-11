
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Manages collision masks.
    /// </summary>
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
        /// What is treated by the character as collidable ground?
        /// </summary>
        private LayerMask _whatIsGround;
        /// <summary>
        /// Used to access the _whatIsGround mask from the outside.
        /// </summary>
        public LayerMask WhatIsGround => _whatIsGround; //Auto properties are not visible in the inspector!
        /// <summary>
        /// Used to access the _wallCollisionLayers mask from the outside.
        /// </summary>
        public LayerMask WallCollisionLayers => _wallCollisionLayers; //Auto properties are not visible in the inspector!

        private void Start()
        {
            _whatIsGround = _defaultGroundCollisionMask;
            InputReader.RegisterChangeCollisionMaskHandler(ChangeCollisionScheme);
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
                    _whatIsGround = _defaultGroundCollisionMask;
                    gameObject.layer = MathOperations.ConvertLayerMaskValueToIndex(_defaultPlayer.value);
                    break;
                case GroundCheckType.JumpingOffPlatform:
                    _whatIsGround = _dropFromPlatformColMask;
                    gameObject.layer = MathOperations.ConvertLayerMaskValueToIndex(_playerJumpsOffPlatform.value);
                    break;
                default:
                    _whatIsGround = _defaultGroundCollisionMask;
                    gameObject.layer = MathOperations.ConvertLayerMaskValueToIndex(_defaultPlayer.value);
                    break;
            }
        }
    }
}

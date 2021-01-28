﻿using Assets.Scripts.Game.Player.Character.Skills;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Character
{
    /// <summary>
    /// Used by other GOs to get references to parts of player which contains their skills.
    /// </summary>
    public class SkillContainerProvider : MonoBehaviour
    {
        [Tooltip("The parent gameobject of character's skills.")]
        [SerializeField] private Transform _skillsContainerGO;
        [Tooltip("Reference to character's skill controller.")]
        [SerializeField] private SkillsController _skillsController;
        [Tooltip("The rigidbody of the player's character.")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [Tooltip("Script containing the current state of the character.")]
        [SerializeField] private PlayerState _playerState;

        /// <summary>
        /// Retrieves the reference to the gameobject that is the parent of skills.
        /// </summary>
        public Transform SkillsContainerGO => _skillsContainerGO;
        /// <summary>
        /// Retrieves the reference to the skills controller of the player.
        /// </summary>
        public SkillsController SkillsController => _skillsController;
        /// <summary>
        /// The rigidbody of the player's character.
        /// </summary>
        public Rigidbody2D CharacterRigidbody => _rigidbody;
        /// <summary>
        /// Gets the state of the player.
        /// </summary>
        public PlayerState PlayerState => _playerState;
    }
}

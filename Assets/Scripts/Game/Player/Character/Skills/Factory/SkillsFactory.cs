using System;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Character.Skills.Factory
{
    /// <summary>
    /// Creates skill gameobjects basing on provided data.
    /// </summary>
    public class SkillsFactory : MonoBehaviour, ISkillsFactory
    {
        private static SkillsFactory _instance;
        [Header("Double Jump")]
        [Tooltip("Prefab of the double jump skill.")]
        [SerializeField]
        private GameObject _doubleJumpPrefab;
        [Tooltip("Max height of the double jump, relative to player's position upon performing the jump.")]
        [SerializeField] 
        private float _doubleJumpHeight = 3.0f;

        [Header("Wall jump")]
        [Tooltip("Prefab of the wall jump skill.")]
        [SerializeField]
        private GameObject _wallJumpPrefab;
        [Tooltip("Angle of the jump from the wall, relatively to CHECK WHAT SHOULD BE HERE. In degrees.")]
        [Range(1, 90)]
        [SerializeField] private float _wallJumpAngle = 60;
        [Tooltip("How high the jump is, relatively from the position of jumping object.")]
        [SerializeField] private float _wallJumpHeight = 3;

        [Header("Wall slide")]
        [Tooltip("Prefab of the wall slide skill.")]
        [SerializeField]
        private GameObject _wallSlideModifierPrefab;
        [Tooltip("Max vertical velocity (along gravitation vector) the player can get while holding the wall.")]
        [SerializeField] private float _wallSlideMaxVelocityCap = 0f;
        [Tooltip("By what factor is the vertical acceleration multiplied by when holding the wall.")]
        [SerializeField] private float _wallSlideGravityScale = 0.5f;
        [Tooltip("How will the wall slide influence the vertical velocity?")]
        [SerializeField] private WallSlideType[] _wallSlideModifiers;

        [Header("Glide")]
        [Tooltip("Prefab of the glide skill.")]
        [SerializeField]
        private GameObject _glidePrefab;
        [Tooltip("Max vertical velocity that the player can achieve while gliding (along gravitation vector).")]
        [SerializeField]
        private float _maxGlideFallingSpeed = 3f;

        private static int _activeInstances;

        private void Awake()
        {
            _activeInstances++;

            if (_activeInstances > 1)
            {
                throw new GONBaseException("Only one instance of SkillsFactory is allowed!");
            }

            _instance = this;
        }

        public static SkillsFactory GetInstance()
        {
            if (_instance == null)
            {
                throw new GONBaseException("Skill factory not present!");
            }
            return _instance;
        }
        /// <summary>
        /// Creates a new skill of provided type, assigning it as child to provided parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="playerState"></param>
        /// <param name="rb"></param>
        /// <param name="skillType"></param>
        /// <returns></returns>
        public IBasicSkill CreateSkill(Transform parent, PlayerState playerState, Rigidbody2D rb, SkillType skillType)
        {
            switch(skillType)
            {
                case SkillType.DoubleJump:
                    return CreateDoubleJump(parent, playerState, rb);
                case SkillType.WallSlide:
                    return CreateWallSlide(parent, playerState, rb);
                case SkillType.WallJump:
                    return CreateWallJump(parent, playerState, rb);
                case SkillType.Glide:
                    return CreateGlide(parent, playerState, rb);
                default:
                    throw new ArgumentOutOfRangeException(nameof(skillType), skillType, "Such skill does not exist!");
            }
        }

        private IBasicSkill CreateDoubleJump(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newDoubleJump = Instantiate(_doubleJumpPrefab, parent);
            var doubleJumpScript = newDoubleJump.GetComponent<DoubleJump>();

            doubleJumpScript.SetRigidBody(rb);
            doubleJumpScript.SetPlayerState(playerState);
            doubleJumpScript.SetJumpHeight(_doubleJumpHeight);

            return doubleJumpScript;
        }

        private IBasicSkill CreateWallSlide(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newWallSlide = Instantiate(_wallSlideModifierPrefab, parent);
            var wallSlideScript = newWallSlide.GetComponent<WallSlide>();

            wallSlideScript.SetRigidBody(rb);
            wallSlideScript.SetPlayerState(playerState);
            wallSlideScript.SetSlideType(_wallSlideModifiers);
            wallSlideScript.SetMaxVelocity(_wallSlideMaxVelocityCap);
            wallSlideScript.SetGravityScale(_wallSlideGravityScale);

            return wallSlideScript;
        }

        private IBasicSkill CreateWallJump(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newWallJump = Instantiate(_wallJumpPrefab, parent);
            var wallJumpScript = newWallJump.GetComponent<WallJump>();

            wallJumpScript.SetRigidBody(rb);
            wallJumpScript.SetPlayerState(playerState);
            wallJumpScript.SetJumpAngle(_wallJumpAngle);
            wallJumpScript.SetJumpHeight(_wallJumpHeight);

            return wallJumpScript;
        }

        private IBasicSkill CreateGlide(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newGlide = Instantiate(_glidePrefab, parent);
            var glideScript = newGlide.GetComponent<Glide>();

            glideScript.SetRigidBody(rb);
            glideScript.SetPlayerState(playerState);
            glideScript.SetMaxFallingVelocity(_maxGlideFallingSpeed);

            return glideScript;
        }

        private void OnDestroy()
        {
            _instance = null;
            _activeInstances--;
        }
    }
}

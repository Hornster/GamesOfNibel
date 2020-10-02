using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Exceptions;
using UnityEngine;

namespace Assets.Scripts.Player.Character.Skills.Factory
{
    /// <summary>
    /// Creates skill gameobjects basing on provided data.
    /// </summary>
    public class SkillFactory : MonoBehaviour
    {
        private static SkillFactory _instance;

        [Tooltip("Prefab of the double jump skill.")]
        [SerializeField]
        private GameObject _doubleJumpPrefab;
        [Tooltip("Prefab of the wall jump skill.")]
        [SerializeField]
        private GameObject _wallJumpPrefab;
        [Tooltip("Prefab of the wall slide skill.")]
        [SerializeField]
        private GameObject _wallSlideModifierPrefab;
        [Tooltip("Prefab of the glide skill.")]
        [SerializeField]
        private GameObject _glidePrefab;

        private static int _activeInstances;

        private void Awake()
        {
            _activeInstances++;

            if (_activeInstances > 1)
            {
                throw new GONBaseException("Only one instance of SkillFactory is allowed!");
            }

            _instance = this;
        }

        public static SkillFactory GetInstance()
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

            return doubleJumpScript;
        }

        private IBasicSkill CreateWallSlide(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newWallSlide = Instantiate(_wallSlideModifierPrefab, parent);
            var wallSlideScript = newWallSlide.GetComponent<WallSlide>();

            wallSlideScript.SetRigidBody(rb);
            wallSlideScript.SetPlayerState(playerState);

            return wallSlideScript;
        }

        private IBasicSkill CreateWallJump(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newWallJump = Instantiate(_wallJumpPrefab, parent);
            var wallSlideScript = newWallJump.GetComponent<WallJump>();

            wallSlideScript.SetRigidBody(rb);
            wallSlideScript.SetPlayerState(playerState);

            return wallSlideScript;
        }

        private IBasicSkill CreateGlide(Transform parent, PlayerState playerState, Rigidbody2D rb)
        {
            var newGlide = Instantiate(_glidePrefab, parent);
            var glideScript = newGlide.GetComponent<Glide>();

            glideScript.SetRigidBody(rb);
            glideScript.SetPlayerState(playerState);

            return glideScript;
        }

        private void OnDestroy()
        {
            _instance = null;
            _activeInstances--;
        }
    }
}

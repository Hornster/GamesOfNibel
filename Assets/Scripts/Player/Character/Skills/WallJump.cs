using Assets.Scripts.Common.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Character.Skills
{
    public class WallJump : MonoBehaviour, IBasicSkill
    {
        /// <summary>
        /// Rigidbody of the character.
        /// </summary>
        //TODO Debug feature to show skill usage.
        [SerializeField] private UnityEvent _skillUsed;
        
        [SerializeField] private Vector2 _wallJumpDirection;
        [SerializeField] private float _wallJumpForce;


        [SerializeField] //TODO remove serialization, debug feature.
        private Rigidbody2D _characterRigidbody;

        /// <summary>
        /// Uses this skill.
        /// </summary>
        [SerializeField] //TODO remove serialization, debug feature.
        private PlayerState _playerState;

        private void Start()
        {
            _wallJumpDirection.Normalize();
        }

        public void UseSkill()
        {
            //wall hop
            if (_playerState.IsTouchingWall //If the player is holding a climbable wall...
                || (_playerState.isGrounded && _playerState.canWalkOnSlope == false)) //...or is on too steep slope to walk on.
            {
                //TODO fill in!
            }
        }
    }
}
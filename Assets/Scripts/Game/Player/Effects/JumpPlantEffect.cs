using Assets.Scripts.Game.Common;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Effects
{
    /// <summary>
    /// Effect of launching the player up upon jumping onto the jump plant.
    /// </summary>
    public class JumpPlantEffect : MonoBehaviour, IPlayerPhysicalEffect
    {
        /// <summary>
        /// Maximal height the player reaches after jumping onto the plant.
        /// </summary>
        [Header("Jump config")]
        [SerializeField]
        private float _maxJumpHeight = 20.0f;
        /// <summary>
        /// Direction in which the player is sent.
        /// </summary>
        [SerializeField]
        private Vector2 _direction = Vector2.up;
        /// <summary>
        /// Calculated velocity given to player upon landing on the plant.
        /// </summary>
        private float _launchVelocity;

        private void Start()
        {
            _direction = _direction.normalized;
            var refGravity = GlobalGravityManager.GetBaseGravityValue();
            var jumpTime = Mathf.Sqrt((2*_maxJumpHeight)/refGravity);
            _launchVelocity = _maxJumpHeight/jumpTime + 0.5f * GlobalGravityManager.GetBaseGravityValue() * jumpTime;
        }
        /// <summary>
        /// Changes the current velocity of the player, sending them in _direction with _launchVelocity.
        /// </summary>
        /// <param name="rb"></param>
        public void InfluencePlayer(PlayerState playerState, Rigidbody2D rb)
        {
            rb.velocity = _direction * _launchVelocity;
            playerState.PlayerJumps();
        }

        public void OnDrawGizmos()
        {
            var position = transform.position;
            Debug.DrawRay(position, _direction.normalized * _maxJumpHeight);
        }
    }
}

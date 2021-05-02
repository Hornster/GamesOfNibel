using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.CustomEvents;
using Assets.Scripts.Game.Player.Gravity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Player
{
    /// <summary>
    /// Used to reset the player's state.
    /// </summary>
    public class PlayerReset : MonoBehaviour, IReset
    {
        [Header("Reguired references")]
        [Tooltip("Object storing player's state.")]
        [SerializeField] private PlayerState _playerState;

        [Tooltip("Player's rigidbody component.")]
        [SerializeField] private Rigidbody2D _playerBody;

        [Tooltip("Player's gravity manager.")]
        [SerializeField] private LocalGravityManager _localGravityManager;

        [Tooltip("Player's main controller. Used to reset position.")]
        [SerializeField] private PlayerController _playerController;

        ///<summary>
        ///Called when player requests position reset.
        ///</summary>
        private UnityAction<int> _onResetPosition;

        /// <summary>
        /// Resets player's state, including position. Should be called from the outside,
        /// NOT FROM SPAWNER.
        /// </summary>
        public void Reset()
        {
            ResetState();
            
            if(_onResetPosition == null)
            {
                throw new Exception("Player position reset handler not set!");
            }

            _onResetPosition.Invoke(gameObject.GetInstanceID());
        }
        /// <summary>
        /// Register handler for player position reset request.
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterOnPositionResetHandler(UnityAction<int> handler)
        {
            _onResetPosition += handler;
        }

        /// <summary>
        /// Resets the player's state WITHOUT the position - shall be called by scripts that
        /// already reset player's position themselves, like spawners'.
        /// </summary>
        public void ResetState()
        {
            _playerState.ResetState();
            _playerBody.velocity = new Vector2(0, 0);
            _localGravityManager.Reset();
        }
    }
}

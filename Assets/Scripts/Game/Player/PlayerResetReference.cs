using Assets.Scripts.Game.Common;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Player
{
    /// <summary>
    /// A link to the player reset script. Can be used with, for example, colliders.
    /// This way the reset itself will be centralized, no matter where is it called from.
    /// </summary>
    public class PlayerResetReference : MonoBehaviour, IReset
    {
        [Tooltip("Reference to player reset reference.")]
        [SerializeField] private PlayerReset _playerReset;
        public void Reset()
        {
            _playerReset.Reset();
        }
    }
}

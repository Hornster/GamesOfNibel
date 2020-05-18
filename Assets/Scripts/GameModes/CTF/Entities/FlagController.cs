
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF.Entities
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FlagController
    {
        /// <summary>
        /// The transform of the flag carrying player.
        /// </summary>
        [SerializeField]
        private Transform _flagCarrierTransform;
        /// <summary>
        /// The team that this flag belongs to.
        /// </summary>
        [SerializeField]
        private Teams _myTeam;
        /// <summary>
        /// Indicates what team carries the flag.
        /// </summary>
        private Teams _carriedgByTeam;
        /// <summary>
        /// Is the flag carried by someone or is it resting at spawn/on the ground?
        /// </summary>
        private bool _isCarried;
        
        [SerializeField]
        private SpriteRenderer _flagSpriteRenderer;

        private void Start()
        {
            SetColor(_myTeam);
        }

        /// <summary>
        /// Sets the color of the flag.
        /// </summary>
        /// <param name="teamColor"></param>
        public void SetColor(Teams teamColor)
        {
            _flagSpriteRenderer.color = TeamColors.GetTeamColor(teamColor);
        }

        //TODO: Add position update when picked up. Add picking up detection.
    }
}

using Assets.Scripts.Game.Common.CustomEvents;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Puts the team definition in one place.
    /// </summary>
    public class TeamModule : MonoBehaviour
    {
        //[Tooltip("This event will be called when team is changed.")]
        //[SerializeField]
        private TeamsUnityEvent _onTeamChangedHandlers = new TeamsUnityEvent();
        [SerializeField]
        private Teams _myTeam;

        private void Start()
        {
            _onTeamChangedHandlers?.Invoke(_myTeam);
        }

        private void OnValidate()
        {
            _onTeamChangedHandlers?.Invoke(_myTeam);
        }
        /// <summary>
        /// The team of the entity this module was assigned to.
        /// </summary>
        public Teams MyTeam
        {
            get => _myTeam;
            set
            {
                _myTeam = value;
                _onTeamChangedHandlers?.Invoke(_myTeam);
            }
        }

        public void RegisterOnTeamChangedHandler(UnityAction<Teams> handler)
        {
            _onTeamChangedHandlers.AddListener(handler);
        }
    }
}

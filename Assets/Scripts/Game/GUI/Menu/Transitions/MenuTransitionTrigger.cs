using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Used by controls of given menus to trigger transitions between menus.
    /// </summary>
    public class MenuTransitionTrigger : MonoBehaviour
    {
        [Tooltip("Current menu of the calling control.")]
        [SerializeField]
        private MenuType _currentMenu;
        [Tooltip("Menu which should the current one be switched to upon call from the control.")]
        [SerializeField]
        private MenuType _targetMenu;
        [Tooltip("Handlers for transition request. Called when control has been used to switch menus.")]
        [SerializeField]
        private MenuTransitionUnityEvent _requestTransitionHandler;
        /// <summary>
        /// Used to report transition from current menu to some other.
        /// </summary>
        public void ReportTransition()
        {
            _requestTransitionHandler?.Invoke(_targetMenu);
        }

        
    }
}

using System;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Used to request the pause menu to show up during match.
    /// </summary>
    public class PauseMenuRequester : MonoBehaviour
    {
        [Tooltip("Called when something requests enabling the pause menu.")]
        [SerializeField]
        private MenuTransitionUnityEvent _transitionRequestHandler;

        /// <summary>
        /// Used to show the pause menu.
        /// </summary>
        private const MenuType FromMenu = MenuType.None;
        private const MenuType ToMenu = MenuType.PauseMenu;
        /// <summary>
        /// When this object is in the scene, one will be able to retrieve its instance from anywhere.
        /// </summary>
        private static PauseMenuRequester _instance;

        private void Start()
        {
            _instance = this;
        }
        public static PauseMenuRequester GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("Attempted to call pause menu requester but such gameobject was not provided in the scene!");
            }

            return _instance;
        }
        /// <summary>
        /// Used to show the pause menu.
        /// </summary>
        public void ShowPauseMenu()
        {
            _transitionRequestHandler?.Invoke(ToMenu);
        }
    }
}

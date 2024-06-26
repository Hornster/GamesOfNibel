﻿using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Detects changes in game window resolution.
    /// </summary>
    public class ResolutionDetector : SceneSingleton<ResolutionDetector>
    {
        /// <summary>
        /// Who to inform about screen resolution change.
        /// </summary>
        private static UnityAction<Vector2Int> _onScreenResolutionChange;
        /// <summary>
        /// Current resolution.
        /// </summary>
        private Vector2Int _currentScreenResolution;

        public Vector2Int CurrentScreenResolution => _currentScreenResolution;
        private void Awake()
        {
            FixedUpdate();
        }
        private void FixedUpdate()
        {
            var newScreenResolution = new Vector2Int(Screen.width, Screen.height);

            if (_currentScreenResolution != newScreenResolution)
            {
                _onScreenResolutionChange?.Invoke(newScreenResolution);
                _currentScreenResolution = newScreenResolution;
            }
        }

        private void OnDestroy()
        {
            _onScreenResolutionChange = null;
        }
        /// <summary>
        /// Registers handler for resolution change event.
        /// </summary>
        /// <param name="handler">Argument is new resolution, in pixels.</param>
        public static void RegisterOnScreenResolutionChange(UnityAction<Vector2Int> handler)
        {
            _onScreenResolutionChange += handler;
        }
    }
}

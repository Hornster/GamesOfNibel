using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// Detects changes in game window resolution.
    /// </summary>
    public class ResolutionDetector : MonoBehaviour
    {
        /// <summary>
        /// Who to inform about screen resolution change.
        /// </summary>
        private static UnityAction<Vector2Int> _onScreenResolutionChange;
        /// <summary>
        /// Current resolution.
        /// </summary>
        private Vector2Int _currentScreenResolution;

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
        /// <param name="handler">Argument is new resolution.</param>
        public static void RegisterOnScreenResolutionChange(UnityAction<Vector2Int> handler)
        {
            _onScreenResolutionChange += handler;
        }
    }
}

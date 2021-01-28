using UnityEngine;

namespace Assets.Scripts.Game.GUI.Camera
{
    /// <summary>
    /// Dictates behavior of the screen divisor.
    /// </summary>
    public class ScreenDivisorController : MonoBehaviour
    {
        [Tooltip("Parent game object of the divisor. Will be used to toggle it.")]
        [SerializeField]
        private GameObject _screenDivisorGameobject;
        /// <summary>
        /// Used to turn on or off the screen divisor.
        /// </summary>
        /// <param name="isEnabled">Pass TRUE for turned on divisor. FALSE for turned off.</param>
        public void SetEnabled(bool isEnabled)
        {
            _screenDivisorGameobject.SetActive(isEnabled);
        }
    }
}

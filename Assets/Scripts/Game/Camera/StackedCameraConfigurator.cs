using UnityEngine;

namespace Assets.Scripts.Game.Camera
{
    /// <summary>
    /// Configures provided camera in such a way that it can be stacked on top of other cameras.
    /// </summary>
    public class StackedCameraConfigurator : MonoBehaviour
    {
        [Header("Required references")] 
        [Tooltip("Camera to configure.")]
        private UnityEngine.Camera _camera;
    }
}

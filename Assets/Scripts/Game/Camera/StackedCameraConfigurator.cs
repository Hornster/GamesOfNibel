using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Camera
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

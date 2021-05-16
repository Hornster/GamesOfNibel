using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Used to retrieve camera from complicated set of objects.
    /// </summary>
    public class CameraRetriever : MonoBehaviour
    {
        [Tooltip("Returned camera.")]
        [SerializeField]
        private UnityEngine.Camera _camera;
        /// <summary>
        /// Retrieves the camera attached to the script.
        /// </summary>
        /// <returns></returns>
        public UnityEngine.Camera GetCamera()
        {
            if(_camera == null)
            {
                throw new Exception("Tried to retrieve camera without attaching it first!");
            }

            return _camera;
        }
    }
}

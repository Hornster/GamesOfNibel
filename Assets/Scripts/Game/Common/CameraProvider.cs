using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Sets camera for provided canvases.
    /// </summary>
    public class CameraProvider : MonoBehaviour
    {
        [Tooltip("Which canvases should the provider provide cameras to.")]
        [SerializeField]
        private Canvas[] _targetCanvases;
        /// <summary>
        /// Sets camera for set canvases.
        /// </summary>
        /// <param name="camera">Camera to set.</param>
        public void SetCamera(UnityEngine.Camera camera)
        {
            foreach(var canvas in _targetCanvases)
            {
                canvas.worldCamera = camera;
            }
        }
    }
}

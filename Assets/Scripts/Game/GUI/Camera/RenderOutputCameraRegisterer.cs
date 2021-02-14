using UnityEngine;

namespace Assets.Scripts.Game.GUI.Camera
{
    /// <summary>
    /// Used to register the camera of a player to the merged render.
    /// </summary>
    public class RenderOutputCameraRegisterer : MonoBehaviour
    {
        [Tooltip("Camera that shall be registered for the split-screen UI.")]
        [SerializeField]
        private UnityEngine.Camera _myCamera;

        private SplitScreenRenderController _renderController;

        private void Start()
        {
            Debug.Log(this.gameObject);
            _renderController = SplitScreenRenderController.Instance;
            _renderController.RegisterPlayer(_myCamera);
        }
    }
}

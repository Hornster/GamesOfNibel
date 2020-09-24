using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Camera
{
    /// <summary>
    /// Setting the rendering cameras for split-screen mode.
    /// </summary>
    public class SplitScreenRenderController : MonoBehaviour
    {
        /// <summary>
        /// Camera that renders entire screen to players, together with shared menu (pause menu, for example).
        /// </summary>
        [Tooltip("Camera that renders final image to the users, together with shared UI.")]
        [SerializeField]
        private UnityEngine.Camera _mainCamera;
        [Tooltip("Main canvas showing merged UI.")]
        [SerializeField]
        private RectTransform _mainCanvas;
        [Tooltip("Prefab for camera render output that is used to merge cameras from multiple players.")]
        [SerializeField]
        private GameObject _cameraRenderOutputPrefab;
        [Tooltip("Parent GO for created camera render outputs.")]
        [SerializeField]
        private Transform _cameraRenderOutputParent;
        [Tooltip("Used to divide the left and right halves of screen.")]
        [SerializeField]
        private Image _screenVerticalDivisor;
        /// <summary>
        /// Max amount of players in split-screen mode.
        /// </summary>
        private const int MaxPlayers = 2;
        /// <summary>
        /// Stores all created render outputs.
        /// </summary>
        private List<CameraRenderOutput> _registeredRenderOutputs = new List<CameraRenderOutput>();
        /// <summary>
        /// The instance for this singleton.
        /// </summary>
        private static SplitScreenRenderController _instance;

        public static SplitScreenRenderController GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("Tried to retrieve instance of this singleton without creating the singleton!");
            }

            return _instance;
       }
        /// <summary>
        /// Used to register new player in split-screen mode.
        /// </summary>
        /// <param name="playerCamera"></param>
        public void RegisterPlayer(UnityEngine.Camera playerCamera)
        {
            if (_registeredRenderOutputs.Count > MaxPlayers)
            {
                throw new Exception($"Current game version supports up to {MaxPlayers} players in split screen mode!");
            }

            var newRenderOutput = Instantiate(_cameraRenderOutputPrefab, _cameraRenderOutputParent);
            var renderOutputScript = newRenderOutput.GetComponentInChildren<CameraRenderOutput>();

            _registeredRenderOutputs.Add(renderOutputScript);

            renderOutputScript.SetSourceCamera(playerCamera);

            ReconfigureSize();
        }
        /// <summary>
        /// Accordingly changes the rects and render images of the registered players, splitting the screen. Affects camera rects, too.
        /// </summary>
        private void ReconfigureSize()
        {
            var mainCanvasRectWidth = _mainCanvas.rect.width;
            var mainCanvasRectHeight = _mainCanvas.rect.height;

            mainCanvasRectWidth /= _registeredRenderOutputs.Count;

            var cameraViewportWidth = 1f / _registeredRenderOutputs.Count;
                
            for (int i = 0; i < _registeredRenderOutputs.Count; i++)
            {
                _registeredRenderOutputs[i].SetRenderOutputSize((int)mainCanvasRectWidth, (int)mainCanvasRectHeight);
                _registeredRenderOutputs[i].SetRenderOutputPosition(i* mainCanvasRectWidth);
                _registeredRenderOutputs[i].SetCameraViewport(cameraViewportWidth);
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Instance of this singleton has already been declared in object hierarchy!");
            }
            _instance = this;
        }

    }
}

//TODO: Screen resolution changes on display, but two player rendering cameras render to rendertexture, not display.
//TODO: Make a simple class that detects change in screen resolution, register events to it and replace the
//TODO: render texture with new one. Like here: https://answers.unity.com/questions/969731/how-do-i-check-if-the-screengamewindow-has-changed.html
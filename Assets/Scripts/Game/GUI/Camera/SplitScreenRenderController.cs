using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.GUI.Camera
{
    /// <summary>
    /// Setting the rendering cameras for split-screen mode.
    /// </summary>
    [RequireComponent(typeof(PlayerUIController))]
    public class SplitScreenRenderController : SceneSingleton<SplitScreenRenderController>
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
        [Tooltip("Canvas scaler which contains the reference size for the main canvas.")]
        [SerializeField] 
        private CanvasScaler _mainCanvasScaler;
        [Tooltip("Used to turn on and off the screen divisor.")]
        [SerializeField]
        private ScreenDivisorController _screenDivisorController;
        [Tooltip("Object that checks current resolution.")]
        [SerializeField]
        private ResolutionDetector _resolutionDetector;
        [Tooltip("Script that will be used to create the player's UI on demand.")]
        [SerializeField]
        private PlayerUIController _playerUIController;
        /// <summary>
        /// Max amount of players in split-screen mode.
        /// </summary>
        private const int MaxPlayers = 2;
        /// <summary>
        /// Stores all created render outputs.
        /// </summary>
        private List<CameraRenderOutput> _registeredRenderOutputs = new List<CameraRenderOutput>();

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

            _playerUIController.CreatePlayerUI(playerCamera);

            var newRenderOutput = Instantiate(_cameraRenderOutputPrefab, _cameraRenderOutputParent);
            var renderOutputScript = newRenderOutput.GetComponentInChildren<CameraRenderOutput>();

            _registeredRenderOutputs.Add(renderOutputScript);

            renderOutputScript.SetSourceCamera(playerCamera);

            var screenResolution = _resolutionDetector.CurrentScreenResolution;//new Vector2Int(Screen.width, Screen.height);
            ChangeRenderTextureSizes(screenResolution);
        }
        /// <summary>
        /// Accordingly changes the rects and render images of the registered players, splitting the screen. Affects camera rects, too.
        /// </summary>
        private IEnumerator ReconfigureSize()
        {
            yield return new WaitForEndOfFrame();
            var mainCanvasRectWidth = _mainCanvas.rect.width;
            var mainCanvasRectHeight = _mainCanvas.rect.height;

            mainCanvasRectWidth /= _registeredRenderOutputs.Count;

            var cameraViewportWidth = 1f / _registeredRenderOutputs.Count;

            _screenDivisorController.SetEnabled(_registeredRenderOutputs.Count > 1);

            for (int i = 0; i < _registeredRenderOutputs.Count; i++)
            {
                _registeredRenderOutputs[i].SetRenderOutputSize((int)mainCanvasRectWidth, (int)mainCanvasRectHeight);
                _registeredRenderOutputs[i].SetRenderOutputPosition(i* mainCanvasRectWidth);
                _registeredRenderOutputs[i].SetCameraViewport(cameraViewportWidth);
                _registeredRenderOutputs[i].SetRawImageUVRect(cameraViewportWidth);
            }

        }
        
        /// <summary>
        /// Used to change the render texture sizes so they fit the new screen size.
        /// </summary>
        /// <param name="newTextureSize"></param>
        private void ChangeRenderTextureSizes(Vector2Int newTextureSize)
        {
            for (int i = 0; i < _registeredRenderOutputs.Count; i++)
            {
                _registeredRenderOutputs[i].CreateRenderTexture(newTextureSize);
            }

            StartCoroutine(ReconfigureSize());
        }

        private void Awake()
        {
            ResolutionDetector.RegisterOnScreenResolutionChange(ChangeRenderTextureSizes);
        }

    }
}
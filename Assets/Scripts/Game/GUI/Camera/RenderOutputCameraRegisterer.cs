﻿using UnityEngine;

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

        private bool _isInitialized = false;

        //private void OnEnable()
        //{
        //    if (_isInitialized == false)
        //    {
        //        //We need to wait for ResolutionDetector's awake method to be executed as screen
        //        //resollution is required. On the other hand, if we disabled and enabled the camera,
        //        //we
        //        return;
        //    }
        //}
        private void Start()
        {
            _renderController = SplitScreenRenderController.Instance;
            _renderController.RegisterPlayer(_myCamera);
        }
    }
}

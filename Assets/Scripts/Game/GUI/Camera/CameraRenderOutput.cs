﻿using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Assets.Scripts.Game.GUI.Camera
{
    /// <summary>
    /// Passes the render result of provided camera to provided UI raw image element.
    /// </summary>
    public class CameraRenderOutput : MonoBehaviour
    {
        [Tooltip("The image which will be showing the render performed by the camera..")]
        [SerializeField]
        private RawImage _renderOutput;
        [Tooltip("The transform of the image that shows the render.")]
        [SerializeField]
        private RectTransform _renderOutputTransform;

        [Tooltip("The texture which will be holding the render of the camera.")]
        [SerializeField]
        private RenderTexture _renderTexture;
        private UnityEngine.Camera _renderingCamera;
        private void Start()
        {
            _renderOutput.texture = _renderTexture;
            _renderingCamera.ResetAspect();
        }

        /// <summary>
        /// Sets the render source camera to provided one. Makes sure that the render texture is created for it as well.
        /// </summary>
        /// <param name="renderingCamera"></param>
        public void SetSourceCamera(UnityEngine.Camera renderingCamera)
        {
            _renderingCamera = renderingCamera;
        }

        /// <summary>
        /// Sets the size of the image on canvas using provided scale.
        /// </summary>
        /// <param name="sizeX">Size across X axis.</param>
        /// <param name="sizeY">Size across Y axis.</param>
        public void SetRenderOutputSize(int sizeX, int sizeY)
        {
            _renderOutputTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeX);
            _renderOutputTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY);
        }

        /// <summary>
        /// Sets the position of the image on canvas. Render output images should have their
        /// pivots set to upper left corner.
        /// </summary>
        /// <param name="positionX">Position across the X axis.</param>
        public void SetRenderOutputPosition(float positionX)
        {
            _renderOutput.transform.localPosition = new Vector2(positionX, 0f);
        }
        /// <summary>
        /// Creates render texture for the camera of provided size.
        /// </summary>
        /// <param name="renderTextureSize">Size of the new render texture.</param>
        public void CreateRenderTexture(Vector2Int renderTextureSize)
        {
            //Already created render texture cannot be modified during runtime - release it.
            if (_renderTexture != null)
            {
                _renderTexture.Release();
            }
            //Create new texture using provided size.
            var renderTexDesc = new RenderTextureDescriptor(renderTextureSize.x, renderTextureSize.y, GraphicsFormat.R8G8B8A8_UNorm, (int)DepthBits.Depth24);

            _renderTexture = new RenderTexture(renderTexDesc);
            _renderingCamera.targetTexture = _renderTexture;
            _renderOutput.texture = _renderTexture;
        }
        /// <summary>
        /// Updates the aspect of render texture size assigned to this render output's camera. 
        /// </summary>
        /// <param name="newAspect">Width of screen/height.</param>
        public void UpdateAspect(float newAspect)
        {
            var newTextureSize = new Vector2(_renderTexture.width, 0f);
            newTextureSize.y = newTextureSize.x / newAspect;
            CreateRenderTexture(new Vector2Int((int)newTextureSize.x, (int)newTextureSize.y));
        }
        /// <summary>
        /// Sets the viewport of the camera.
        /// </summary>
        /// <param name="viewPortWidth">Width of the viewport, from 0 to 1.</param>
        public void SetCameraViewport(float viewPortWidth)
        {
            viewPortWidth = Mathf.Clamp(viewPortWidth, 0f, 1f);

            var cameraRect = _renderingCamera.rect;
            cameraRect.width = viewPortWidth;
            _renderingCamera.rect = cameraRect;
        }
        /// <summary>
        /// Changes the width of the uv rect of the raw image component which is used to
        /// display the results from the cameras.
        /// </summary>
        /// <param name="viewportWidth">New viewport width.</param>
        public void SetRawImageUVRect(float viewportWidth)
        {
            var renderOutputUVRect = _renderOutput.uvRect;
            renderOutputUVRect.width = viewportWidth;
            _renderOutput.uvRect = renderOutputUVRect;
        }

        public Vector2Int GetRenderTextureSize()
        {
            return new Vector2Int(_renderTexture.width, _renderTexture.height);
        }
    }
}

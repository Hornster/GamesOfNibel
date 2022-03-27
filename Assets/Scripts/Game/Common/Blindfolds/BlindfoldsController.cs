using Assets.Scripts.Game.Common.Data.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Common.Blindfolds
{
    public class BlindfoldsController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _topBlindfold;
        [SerializeField]
        private RectTransform _bottomBlindfold;

        private Vector2 _topBlindfoldAnchor;
        private Vector2 _bottomBlindfoldAnchor;
        // Start is called before the first frame update
        void Start()
        {
            ResolutionDetector.RegisterOnScreenResolutionChange(this.OnResolutionChanged);
            _topBlindfoldAnchor = _topBlindfold.anchoredPosition;
            _bottomBlindfoldAnchor = _bottomBlindfold.anchoredPosition;
            OnResolutionChanged(ResolutionDetector.Instance.CurrentScreenResolution);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnResolutionChanged(Vector2Int newResolution)
        {
            var widthScaleRatio = (float)newResolution.x / SGConstants.ReferenceResolutionX;
            var menuNewHeight = SGConstants.ReferenceResolutionY * widthScaleRatio;
            var emptySpaceHeight = newResolution.y - menuNewHeight;
            var blindfoldOffset = emptySpaceHeight / 2;//2 since blindfolds come from bottom and top
            blindfoldOffset /= widthScaleRatio;//the blindfolds offset has to be rescaled back to 4k screen dimensions.

            var topBlindfoldPos = _topBlindfoldAnchor;
            var bottomBlindfoldPos = _bottomBlindfoldAnchor;

            topBlindfoldPos.y += blindfoldOffset;
            bottomBlindfoldPos.y -= blindfoldOffset;

            _topBlindfold.anchoredPosition = topBlindfoldPos;
            _bottomBlindfold.anchoredPosition = bottomBlindfoldPos;
        }
    }
}
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
        // Start is called before the first frame update
        void Start()
        {
            ResolutionDetector.RegisterOnScreenResolutionChange(this.OnResolutionChanged);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnResolutionChanged(Vector2Int newResolution)
        {
            var referenceResolutionRatio = SGConstants.ReferenceResolutionX / SGConstants.ReferenceResolutionY;
            var newResolutionRatio = newResolution.x / ((float)newResolution.y);

            var percentageDifference = newResolutionRatio / referenceResolutionRatio;
            var blindfoldedAreaPercent = 1 - percentageDifference;

            var singleBlindfoldOffset = newResolution.y * blindfoldedAreaPercent / 2;
            var topBlindfoldPos = _topBlindfold.anchoredPosition;
            var bottomBlindfoldPos = _bottomBlindfold.anchoredPosition;

            topBlindfoldPos.y -= singleBlindfoldOffset;
            bottomBlindfoldPos.y += singleBlindfoldOffset;

            _topBlindfold.anchoredPosition = topBlindfoldPos;
            _bottomBlindfold.anchoredPosition = bottomBlindfoldPos;
        }
    }
}
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
            var resolutionXCoeff = (double)newResolution.x / SGConstants.ReferenceResolutionX;
            var newHeight = SGConstants.ReferenceResolutionY * resolutionXCoeff;
            var resolutionYCoeff = (double)newResolution.y / SGConstants.ReferenceResolutionY;

            var blindfoldHeight = newHeight * (1 - resolutionYCoeff) * 0.5f;

            _topBlindfold.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (int)blindfoldHeight);
            _bottomBlindfold.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (int)blindfoldHeight);
        }
    }
}
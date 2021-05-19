using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common.UI
{
    /// <summary>
    /// Used to modify canvas parameters from code.
    /// </summary>
    public class CanvasModifier
    {
        /// <summary>
        /// Checks if provided gameobject has a canvas attached to it. If yes - changes it to full stretch
        /// positioned in the middle.
        /// </summary>
        public static IEnumerator ChkForCanvas(GameObject menu)
        {
            yield return new WaitForEndOfFrame();
            var gameObjectCanvas = menu.GetComponent<Canvas>();
            if (gameObjectCanvas != null)
            {
                var rectTransform = menu.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;  //Stretch the canvas over the top one.
                rectTransform.pivot = new Vector2(0.5f, 0.5f); //The pivot shall go to the center.
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
        }
    }
}

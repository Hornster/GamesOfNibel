using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.MapInitialization
{
    /// <summary>
    /// Used to asign topmost menu to top menu canvas. Top menu is the menu seen by everyone in split screen mode.
    /// </summary>
    public class InGameTopUIMenuAssigner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Parent which the menu(s) shall be assigned to.")]
        private Transform _parent;
        public void AssignMenu(GameObject menu)
        {
            menu.transform.SetParent(_parent.transform, true);
            StartCoroutine(ChkForCanvas(menu));
        }
        /// <summary>
        /// Checks if provided gameobject has a canvas attached to it. If yes - changes it to full stretch
        /// positioned in the middle.
        /// </summary>
        private IEnumerator ChkForCanvas(GameObject menu)
        {
            yield return new WaitForEndOfFrame();
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

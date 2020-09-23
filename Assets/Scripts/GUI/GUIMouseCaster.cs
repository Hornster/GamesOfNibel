using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    /// <summary>
    /// Performs casts of rays from mouse onto canvases in menus.
    /// </summary>
    public class GUIMouseCaster : MonoBehaviour
    {
        /// <summary>
        /// Reference to the event system
        /// </summary>
        [SerializeField] 
        private EventSystem _eventSystem;
        /// <summary>
        /// Reference to the graphics raycaster that will be used for casts.
        /// </summary>
        [SerializeField] 
        private GraphicRaycaster _graphicRaycaster;

        /// <summary>
        /// Performs raycast on UI, returns closest result.
        /// </summary>
        /// <returns></returns>
        public RaycastResult CastUIRayFromMouse()
        {
            var pointerData = new PointerEventData(_eventSystem) {position = Input.mousePosition};

            var results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(pointerData, results);

            var closestResult = new RaycastResult();

            if (results.Count > 0)
            {
                int resultsIndex = 0;
                closestResult = results[0];
                float closestDistance = closestResult.distance;

                for (int i = 1; i < results.Count; i++)
                {
                    if (closestDistance > results[i].distance)
                    {
                        resultsIndex = i;
                        closestDistance = results[i].distance;
                    }
                }

                closestResult = results[resultsIndex];
            }

            return closestResult;
        }
    }
}

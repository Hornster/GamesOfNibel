using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Used to reset the value of the scroll it is assigned to.
    /// </summary>
    public class ScrollResetter : MonoBehaviour
    {
        [Tooltip("What value should be the scroll reset to upon firing of the event.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _positionAfterReset = 0.0f;

        [SerializeField] private Scrollbar _scroll;
        /// <summary>
        /// Resets scroll position to provided one. Can be used as event handler.
        /// </summary>
        public void ResetScroll()
        {
            _scroll.value = _positionAfterReset;
        }
    }
}

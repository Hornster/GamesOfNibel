using System.Collections.Generic;
using Assets.Scripts.Game.GUI.Menu.Interface;
using UnityEngine;

namespace Assets.Scripts.Game.GUI.Menu
{
    /// <summary>
    /// Searches for custom controls across provided gameobject and its children.
    /// Allows for retrieving list of found items.
    /// </summary>
    public class CustomControlsFinder : MonoBehaviour
    {
        /// <summary>
        /// Stores all controls connected to this control, together with their IDs. All of them.
        /// </summary>
        private Dictionary<int, ICustomMenuControl> _foundControls = new Dictionary<int, ICustomMenuControl>();
        /// <summary>
        /// Returns all found controls for the menu this instance is assigned to.
        /// </summary>
        public Dictionary<int, ICustomMenuControl> FoundControls => _foundControls;
        /// <summary>
        /// Returns count of controls found in the menu.
        /// </summary>
        public int ControlsCount { get; private set; }
        /// <summary>
        /// Find all controls in the menu this instance is assigned to.
        /// </summary>
        private void Start()
        {
            ControlsCount = 0;
            var controls = GetComponentsInChildren<ICustomMenuControl>();
            foreach (var control in controls)
            {
                _foundControls.Add(ControlsCount, control);
                control.ControlId = ControlsCount;
                ControlsCount++;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu
{
    /// <summary>
    /// Manages buttons that are children of this scripts monobehavior.
    /// </summary>
    public class ButtonsController : MonoBehaviour
    {
        /// <summary>
        /// Amount of buttons connected to this controller. Iterated from 0!
        /// </summary>
        private int buttonsCount;
        /// <summary>
        /// Stores all buttons connected to this control, together with their IDs. All of them.
        /// </summary>
        private Dictionary<int, CustomButton> _buttons = new Dictionary<int, CustomButton>();
        // Start is called before the first frame update
        void Start()
        {
            int buttonId = 0;

            var buttons = GetComponentsInChildren<CustomButton>();
            foreach (var button in buttons)
            {
                _buttons.Add(buttonId, button);
                buttonId++;
            }
        }
        /// <summary>
        /// Checks if the player moved the selection among the buttons with use of vertical axis.
        /// </summary>
        private void ChkPlayerInput()
        {

        }
        void Update()
        {
            
        }
    }
}

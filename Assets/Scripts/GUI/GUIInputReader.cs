using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI
{
    /// <summary>
    /// Static class for reading the input data from the user when in GUI.
    /// </summary>
    public class GUIInputReader : MonoBehaviour
    {
        /// <summary>
        /// Stores handlers for the submit action from the player.
        /// </summary>
        private static UnityAction _submitHandler;
        /// <summary>
        /// Stores raw input from the player on horizontal and vertical axes. Updated as fast as Unity can.
        /// </summary>
        public static Vector2 PlayerInput { get; private set; }
        /// <summary>
        /// Stores raw input from submit control.
        /// </summary>
        public static float SubmitInput { get; private set; }

        private void Update()
        {
            PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SubmitInput = Input.GetAxisRaw("Submit");
        }
    }
}

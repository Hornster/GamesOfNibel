using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.GUI.Gamemodes.CTF.SingleControls
{
    /// <summary>
    /// Interface for text-based controls shown on the GUI.
    /// </summary>
    public interface IGuiTextControl
    {
        /// <summary>
        /// Assigns new value to a text control.
        /// </summary>
        /// <param name="newValue"></param>
        void ShowMessage(string newValue);
        /// <summary>
        /// Assigns new color to the text control.
        /// </summary>
        /// <param name="newColor"></param>
        void ChangeColor(Color newColor);
        /// <summary>
        /// Forces the message to be hidden.
        /// </summary>
        /// <param name="instant">Passing TRUE will ensure that the message becomes hidden instantly.
        /// Passing FALSE lets the control decide whether to use fading or not.</param>
        void HideMessage(bool instant = false);
    }
}

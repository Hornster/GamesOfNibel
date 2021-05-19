using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.UI;
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
            StartCoroutine(CanvasModifier.ChkForCanvas(menu));
        }
        
    }
}

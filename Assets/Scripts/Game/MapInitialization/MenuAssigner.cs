using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.MapInitialization
{
    /// <summary>
    /// Used to assign menus to given game object.
    /// </summary>
    public class MenuAssigner : MonoBehaviour
    {
        [Tooltip("The object which will become parent of assigned menus.")]
        [SerializeField]
        private Transform _parentObject;

        /// <summary>
        /// Sets provided transform as child of set parent object.
        /// </summary>
        /// <param name="menuTransform"></param>
        public void AssignMenu(Transform menuTransform)
        {
            menuTransform.SetParent(_parentObject);
        }
    }
}

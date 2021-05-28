using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// A singleton that lasts in given scene but does not survive switching it to another one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);


        public static T Instance => CheckForInstance();

        private static T CheckForInstance()
        {
            if (LazyInstance?.Value == null)
            {
                LazyInstance = new Lazy<T>(CreateSingleton);
            }
            
            return LazyInstance.Value;
        }

        private static T CreateSingleton()
        {
            var foundObject = FindObjectOfType(typeof(T)) as T;
            if (foundObject == null)
            {
                var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
                foundObject = ownerObject.AddComponent<T>();
            }

            return foundObject;
        }
    }
}

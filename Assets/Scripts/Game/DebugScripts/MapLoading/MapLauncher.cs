using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.MapInitialization;
using UnityEngine;

namespace Assets.Scripts.Game.DebugScripts.MapLoading
{
    public class MapLauncher : MonoBehaviour
    {
        [SerializeField] private SceneDataInitializer _sceneDataInitializer;

        private void Start()
        {
            StartCoroutine(_sceneDataInitializer.CreateData());
        }
    }
}

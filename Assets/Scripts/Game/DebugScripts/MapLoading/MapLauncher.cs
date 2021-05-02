using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.GUI.Menu.Transitions;
using Assets.Scripts.Game.MapInitialization;
using UnityEngine;

namespace Assets.Scripts.Game.DebugScripts.MapLoading
{
    public class MapLauncher : MonoBehaviour
    {
        [SerializeField] private MenuTransitionManager _transitionManager;
        /// <summary>
        /// What scene should the transition go to.
        /// </summary>
        [SerializeField] private string _startScene;

        private void Start()
        {
            _transitionManager.PerformSceneTransition(_startScene);
        }
    }
}

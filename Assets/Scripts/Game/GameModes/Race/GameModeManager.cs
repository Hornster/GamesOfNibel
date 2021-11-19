using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.GameModes.Race
{
    [RequireComponent(typeof(Timer))]
    public abstract class GameModeManager : MonoBehaviour
    {
        /// <summary>
        /// Set to true as long as the round of the match is being played. When match ending requirements are met, the match ends and
        /// the flag is set to false/
        /// </summary>
        protected bool _isRoundOn;

        protected Timer _roundTimer;

        public abstract void StartMatch();
        /// <summary>
        /// Registers awaiting event for scene load.
        /// </summary>
        protected void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        /// <summary>
        /// Called when scene is loaded - starts match begin sequence.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
            StartMatch();
        }
    }
}

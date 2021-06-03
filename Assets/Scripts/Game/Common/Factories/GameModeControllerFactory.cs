using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.GameModes.CTF;
using Assets.Scripts.Game.GameModes.CTF.Entities;
using Assets.Scripts.Game.GameModes.Managers;
using Assets.Scripts.Game.GUI.Gamemodes.CTF;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    /// <summary>
    /// Creates game mode controllers.
    /// </summary>
    public class GameModeControllerFactory : MonoBehaviour, IGameModeControllerFactory
    {
        [Tooltip("Prefab for the capture the flag mode controller.")]
        [SerializeField]
        private GameObject _ctfMatchControllerPrefab;
        
        /// <summary>
        /// Creates controller for CTF mode and connects it with bases and UI.
        /// </summary>
        /// <param name="sceneData">Data of the scene, takes neutral bases from here.</param>
        /// <param name="gameModeUI">UI for CTF game mode.</param>
        /// <returns></returns>
        private GameObject CreateCTFController(SceneData sceneData, GameObject gameModeUI)
        {
            var controller = Instantiate(_ctfMatchControllerPrefab);
            var flagController = controller.GetComponentInChildren<FlagSpawnersController>();
            var ctfGameModeManager = controller.GetComponentInChildren<CtfGameModeManager>();

            if (sceneData.Bases.TryGetValue(Teams.Neutral, out var neutralBases))
            {
                flagController.Initialize(neutralBases);
            }
            else
            {
                throw new GONBaseException("No neutral bases found for CTF mode!");
            }

            var ctfUIController = gameModeUI.GetComponentInChildren<CtfGuiController>();
            ctfGameModeManager.SetUIController(ctfUIController);

            if (ctfUIController == null)
            {
                throw new GONBaseException("Ctf UI Controller not found!");
            }

            return controller;
        }
        /// <summary>
        /// Creates selected game controller and connects it to provided UI. Make sure the UI is correct.
        /// </summary>
        /// <param name="gameModeUI"></param>
        /// <param name="gameplayMode"></param>
        /// <returns></returns>
        public GameObject CreateGameController(SceneData sceneData, GameObject gameModeUI, GameplayModesEnum gameplayMode)
        {
            switch (gameplayMode)
            {
                case GameplayModesEnum.CTF:
                    return CreateCTFController(sceneData, gameModeUI);
                case GameplayModesEnum.Race:
                    throw new NotImplementedException();
                case GameplayModesEnum.TimeAttack:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameplayMode), gameplayMode, "No such game mode present!");
            }
        }

        
    }
}
//TODO: Create spawning of CTF game controller, together with connecting it with UI and bases.
//TODO: Should there be a spawn trigger? As in, map fully loaded, start counting towards the flag spawn
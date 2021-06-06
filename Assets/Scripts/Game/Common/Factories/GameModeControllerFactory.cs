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
        private GameObject CreateCTFController(SceneData sceneData, List<GameObject> gameModeUIs)
        {
            var controller = Instantiate(_ctfMatchControllerPrefab);
            var flagController = controller.GetComponentInChildren<FlagSpawnersController>();
            var ctfGameModeManager = controller.GetComponentInChildren<CtfGameModeManager>();

            if (sceneData.Bases.TryGetValue(Teams.Neutral, out var neutralBases))
            {
                flagController.Initialize(neutralBases);
                ctfGameModeManager.AddFlagSpawnerController(flagController);
            }
            else
            {
                throw new GONBaseException("No neutral bases found for CTF mode!");
            }

            foreach (var gameModeUI in gameModeUIs)
            {
                var ctfUIController = gameModeUI.GetComponentInChildren<CtfGuiController>();
                ctfGameModeManager.AddUIController(ctfUIController);

                if (ctfUIController == null)
                {
                    throw new GONBaseException("Ctf UI Controller not found!");
                }
            }

            ctfGameModeManager = RegisterFlagCaptureHandlers(sceneData, ctfGameModeManager);

            return controller;
        }
        /// <summary>
        /// Registers (if possible) players' bases' flag capture handlers.
        /// </summary>
        /// <param name="sceneData">Source of bases.</param>
        /// <param name="gameModeManager">Capture The Flag mode manager script.</param>
        /// <returns></returns>
        private CtfGameModeManager RegisterFlagCaptureHandlers(SceneData sceneData, CtfGameModeManager gameModeManager)
        {
            if (sceneData.Bases.TryGetValue(Teams.Lily, out var lilyBases))
            {
                gameModeManager.RegisterFlagCaptureHandlers(lilyBases);
            }

            if (sceneData.Bases.TryGetValue(Teams.Lotus, out var lotusBases))
            {
                gameModeManager.RegisterFlagCaptureHandlers(lotusBases);
            }

            return gameModeManager;
        }
        /// <summary>
        /// Creates selected game controller and connects it to provided UI. Make sure the UI is correct.
        /// </summary>
        /// <param name="gameModeUIs">Players UIs - each player has a UI set that shows the match state to them.</param>
        /// <param name="gameplayMode">Match type which will be played.</param>
        /// <returns></returns>
        public GameObject CreateGameController(SceneData sceneData, List<GameObject> gameModeUIs, GameplayModesEnum gameplayMode)
        {
            switch (gameplayMode)
            {
                case GameplayModesEnum.CTF:
                    return CreateCTFController(sceneData, gameModeUIs);
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
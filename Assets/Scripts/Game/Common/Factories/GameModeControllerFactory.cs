using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Data.Constants;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.GameModes.CTF;
using Assets.Scripts.Game.GameModes.CTF.Entities;
using Assets.Scripts.Game.GameModes.Managers;
using Assets.Scripts.Game.GameModes.Race;
using Assets.Scripts.Game.GUI.Gamemodes.CTF;
using Assets.Scripts.Game.GUI.Gamemodes.Race;
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
        [Tooltip("Prefabs for available game managers (controllers).")]
        [SerializeField]
        private GameplayModeGameObjectDictionary _matchControllers = new GameplayModeGameObjectDictionary();

        #region CTFManager
        /// <summary>
        /// Creates controller for CTF mode and connects it with bases and UI.
        /// </summary>
        /// <param name="sceneData">Data of the scene, takes neutral bases from here.</param>
        /// <param name="gameModeUIs">UIs for CTF game mode.</param>
        /// <param name="gameModeController">Already instantiated instance of controller to set up.</param>
        /// <returns></returns>
        private GameObject CreateCTFController(SceneData sceneData, List<GameObject> gameModeUIs, GameObject gameModeController)
        {
            //var controller = Instantiate(gameModeController);
            var flagController = gameModeController.GetComponentInChildren<FlagSpawnersController>();
            var ctfGameModeManager = gameModeController.GetComponentInChildren<CtfGameModeManager>();

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

            return gameModeController;
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
        #endregion

        #region RaceManager

        private GameObject CreateRaceController(SceneData sceneData, List<GameObject> gameModeUIs, GameObject gameModeController)
        {
            var raceController = gameModeController.GetComponentInChildren<RaceGameModeManager>();
            foreach (var ui in gameModeUIs)
            {
                var uiController = ui.GetComponentInChildren<RaceGUIController>();
                raceController.AddPlayerGUI(uiController);
                RegisterRaceHandlers(raceController, sceneData);
            }

            return gameModeController;
        }

        private RaceGameModeManager RegisterRaceFinishHandlerForBase(RaceGameModeManager raceGameModeManager, GameObject playerBase)
        {
            var raceFinishBase = playerBase.GetComponentInChildren<RaceFinishBaseController>();
            if (raceFinishBase != null)
            {
                raceGameModeManager.RegisterPlayerFinishedRaceHandler(raceFinishBase);
            }

            return raceGameModeManager;
        }
        private RaceGameModeManager RegisterRaceHandlers(RaceGameModeManager raceGameModeManager, SceneData sceneData)
        {
            var bases = sceneData.Bases;

            foreach (var team in bases)
            {
                foreach (var playerBase in team.Value)
                {
                    raceGameModeManager = RegisterRaceFinishHandlerForBase(raceGameModeManager, playerBase);
                }
            }

            return raceGameModeManager;
        }

        #endregion
        /// <summary>
        /// Returns an instantiated instance of selected gameplay controller.
        /// </summary>
        /// <param name="gameplayMode">What gameplay controller should be instantiated.</param>
        /// <returns></returns>
        private GameObject InstantiateController(GameplayModesEnum gameplayMode)
        {
            if (_matchControllers.dictionary.TryGetValue(gameplayMode, out var controllerPrefab))
            {
                return Instantiate(controllerPrefab);
            }
            else
            {
                throw new Exception(ErrorMessages.NoGameplayControllerFound);
            }
        }
        /// <summary>
        /// Creates selected game controller and connects it to provided UI. Make sure the UI is correct.
        /// </summary>
        /// <param name="gameModeUIs">Players UIs - each player has a UI set that shows the match state to them.</param>
        /// <param name="gameplayMode">Match type which will be played.</param>
        /// <returns></returns>
        public GameObject CreateGameController(SceneData sceneData, List<GameObject> gameModeUIs, GameplayModesEnum gameplayMode)
        {
            var gameModeController = InstantiateController(gameplayMode);

            switch (gameplayMode)
            {
                case GameplayModesEnum.CTF:
                    return CreateCTFController(sceneData, gameModeUIs, gameModeController);
                case GameplayModesEnum.Race:
                    return CreateRaceController(sceneData, gameModeUIs, gameModeController);
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
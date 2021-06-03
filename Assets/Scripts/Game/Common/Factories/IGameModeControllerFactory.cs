using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    /// <summary>
    /// Interface for game mode controllers factory.
    /// </summary>
    public interface IGameModeControllerFactory
    {
        /// <summary>
        /// Creates selected game controller and connects it to provided UI. Make sure the UI is correct.
        /// </summary>
        /// <param name="gameModeUIs">Players UIs - each player has a UI set that shows the match state to them.</param>
        /// <param name="gameplayMode">Match type which will be played.</param>
        /// <returns></returns>
        GameObject CreateGameController(SceneData sceneData, List<GameObject> gameModeUIs, GameplayModesEnum gameplayMode);
    }
}

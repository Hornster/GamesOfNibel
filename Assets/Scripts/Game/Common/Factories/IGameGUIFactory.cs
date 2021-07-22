using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    public interface IGameGUIFactory
    {
        /// <summary>
        /// Creates and returns base menu gameobject that can have other UIs assigned to it.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="defaultPauseMenu">If set to true will add default pause menu to the GUI while creating it.</param>
        /// <returns></returns>
        GameObject CreateBaseUI(Transform parent, bool defaultPauseMenu = true);
        /// <summary>
        /// Creates UI visible by given player.
        /// </summary>
        /// <param name="gameplayMode">Decides what should the UI represent.</param>
        /// <param name="parent">What should be the parent of created UI.</param>
        /// <param name="playerID">Owning player's ID.</param>
        /// <returns></returns>
        GameObject CreatePlayerGameUI(GameplayModesEnum gameplayMode, Transform parent, int playerID);
    }
}

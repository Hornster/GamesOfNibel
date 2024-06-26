﻿using System;
using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GUI.Gamemodes.CTF;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    public class GameGUIFactory : MonoBehaviour, IGameGUIFactory
    {
        [Header("Main gameplay GUI.")]
        [Tooltip("Main game GUI prefab, should contain Pause.")]
        [SerializeField]
        private GameObject _baseGUIPrefab;

        [Header("Game type main UIs.")]
        [Tooltip("Capture the Flag main UI prefab.")]
        [SerializeField]
        private GameplayModeGameObjectDictionary _UIPrefabs = new GameplayModeGameObjectDictionary();
        /// <summary>
        /// Creates and returns base menu gameobject that can have other UIs assigned to it.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="defaultPauseMenu">If set to true will add default pause menu to the GUI while creating it.</param>
        /// <returns></returns>
        public GameObject CreateBaseUI(Transform parent, bool defaultPauseMenu = true)
        {
            var mainUI = Instantiate(_baseGUIPrefab);

            return mainUI;
        }

        /// <summary>
        /// Creates UI visible by given player.
        /// </summary>
        /// <param name="gameplayMode">Decides what should the UI represent.</param>
        /// <param name="parent">What should be the parent of created UI.</param>
        /// <param name="playerID">Owning player's ID.</param>
        /// <returns></returns>
        public GameObject CreatePlayerGameUI(GameplayModesEnum gameplayMode, Transform parent, int playerID)
        {
            if(_UIPrefabs.dictionary.TryGetValue(gameplayMode, out var prefab))
            {
                var menu = Instantiate(prefab, parent);

                var guiController = menu.GetComponentInChildren<IGUIInterface>();
                guiController.OwningPlayerID = playerID;

                return menu;
            }
            else
            {
                throw new Exception("GUI gameobject was null!");
            }
        }
    }
}

//TODO: You added the factory to scenedataini prefab. Now add the script reference to main ini script and
//write UI initializing code.
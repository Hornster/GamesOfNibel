using System;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Player;
using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    /// <summary>
    /// Creates player characters.
    /// </summary>
    public class CharacterFactory : MonoBehaviour, ICharacterFactory
    {
        [Tooltip("Prefab of the default character - player 1 variant.")]
        [SerializeField]
        private GameObject _defaultP1CharacterPrefab;
        [Tooltip("Prefab of the default character - player 2 variant.")]
        [SerializeField]
        private GameObject _defaultP2CharacterPrefab;
        public GameObject CreateCharacter(PlayerConfig playerConfig)
        {
            GameObject newCharacter = null;
            //TODO for now we just return new character.
            if (playerConfig.WhatPlayer == WhatPlayer.P1)
            {
                newCharacter = Instantiate(_defaultP1CharacterPrefab);
            }
            else if(playerConfig.WhatPlayer == WhatPlayer.P2)
            {
                newCharacter = Instantiate(_defaultP2CharacterPrefab);
            }
            else
            {
                throw new Exception($"There is noprefab defined for player type {playerConfig.WhatPlayer}");
            }

            var playerMatchData = newCharacter.GetComponentInChildren<PlayerController>().PlayerMatchData;

            SetPlayerTeam(newCharacter, playerConfig.PlayerTeam);
            SetPlayerID(playerMatchData, playerConfig.MyId);
            SetPlayerName(playerMatchData, playerConfig.PlayerName);

            return newCharacter;
        }

        /// <summary>
        /// Sets the team of the player.
        /// </summary>
        /// <param name="characterInstance">Instance of the character that should have it's team set.</param>
        /// <param name="team">What team shall the character instance be set to.</param>
        private void SetPlayerTeam(GameObject characterInstance, Teams team)
        {
            var playerTeamModule = characterInstance.GetComponentInChildren<TeamModule>();
            var playerColorSetter = characterInstance.GetComponentInChildren<ColorSetter>();
            playerColorSetter.SetColor(TeamColorsSO.GetTeamColor(team));
            playerTeamModule.MyTeam = team;
        }

        private void SetPlayerID(PlayerMatchData playerMatchData, int id)
        {
            playerMatchData.PlayerID = id;
        }

        private void SetPlayerName(PlayerMatchData playerMatchData, string playerName)
        {
            playerMatchData.PlayerName = playerName;
        }
    }
}

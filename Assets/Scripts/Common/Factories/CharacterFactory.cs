using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Factories
{
    /// <summary>
    /// Creates player characters.
    /// </summary>
    public class CharacterFactory : MonoBehaviour, ICharacterFactory
    {
        [Tooltip("Prefab of the default character.")]
        [SerializeField]
        private GameObject _defaultCharacterPrefab;
        public GameObject CreateCharacter(PlayerConfig playerConfig)
        {
            //TODO for now we just return new character.
            var newCharacter = Instantiate(_defaultCharacterPrefab);
            SetPlayerTeam(newCharacter, playerConfig.PlayerTeam);

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
            playerColorSetter.SetColor(TeamColors.GetTeamColor(team));
            playerTeamModule.MyTeam = team;
        }
    }
}

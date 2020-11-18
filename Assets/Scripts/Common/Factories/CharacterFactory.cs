using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
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
            return Instantiate(_defaultCharacterPrefab);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Common.Data.CustomContainers
{
    [Serializable]
    public class GameplayModeBaseSubtypeTuple
    {
        //Tooltips since serializing code for dictionaries cuts the names due to bad positioning.
        [Tooltip("Gameplay Mode")]
        [SerializeField]
        private GameplayModesEnum _gameplayMode;
        [Tooltip("Base Subtype")]
        [SerializeField]
        private BaseSubtypeEnum _baseSubtype;
        public GameplayModesEnum GameplayMode => _gameplayMode;
        public BaseSubtypeEnum BaseSubtype => _baseSubtype;

        public GameplayModeBaseSubtypeTuple(GameplayModesEnum gameplayMode, BaseSubtypeEnum baseSubtype)
        {
            _gameplayMode = gameplayMode;
            _baseSubtype = baseSubtype;
        }

        public override int GetHashCode()
        {
            //The fields have overlapping value ranges. Thus, we interpret one as units and the other one as dozens.
            return (int)GameplayMode + (int)BaseSubtype * 10;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameplayModeBaseSubtypeTuple);
        }

        public bool Equals(GameplayModeBaseSubtypeTuple other)
        {
            return other.GameplayMode == this.GameplayMode
            && other.BaseSubtype == this.BaseSubtype;
        }
    }
}

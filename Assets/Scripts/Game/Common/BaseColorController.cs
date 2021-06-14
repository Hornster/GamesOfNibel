using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    public class BaseColorController : MonoBehaviour
    {
        private TeamColorsSO _teamColorsSO;


        [SerializeField]
        private List<ColorSetter> _baseFloorColorSetter = new List<ColorSetter>();
        [SerializeField]
        private List<ColorSetter> _spireColorSetter = new List<ColorSetter>();
        [SerializeField]
        private List<ColorSetter> _additionalElementsColorSetter = new List<ColorSetter>();

        public List<ColorSetter> BaseFloorColorSetter
        {
            get => _baseFloorColorSetter;
            set => _baseFloorColorSetter = value;
        }

        public List<ColorSetter> SpireColorSetter
        {
            get => _spireColorSetter;
            set => _spireColorSetter = value;
        }
        public List<ColorSetter> AdditionalElementsColorSetter
        {
            get => _additionalElementsColorSetter;
            set => _additionalElementsColorSetter = value;
        }

        public void SetTeamColorsSO(TeamColorsSO teamColorsSO)
        {
            _teamColorsSO = teamColorsSO;
        }
        /// <summary>
        /// Changes the color accordingly to provided team.
        /// </summary>
        /// <param name="team"></param>
        public void ChangeColor(Teams team)
        {
            if (_teamColorsSO == null)
            {
                throw new Exception("You first need to initalize the color SO!");
            }
            switch(team)
            {
                case Teams.Lotus:
                    SetColorForList(_baseFloorColorSetter, _teamColorsSO.LotusBaseColor);
                    SetColorForList(_spireColorSetter, _teamColorsSO.LotusSpireColor);
                    SetColorForList(_additionalElementsColorSetter, _teamColorsSO.LotusAdditionalElementsColor);
                    break;
                case Teams.Lily:
                    SetColorForList(_baseFloorColorSetter, _teamColorsSO.LilyBaseColor);
                    SetColorForList(_spireColorSetter, _teamColorsSO.LilySpireColor);
                    SetColorForList(_additionalElementsColorSetter, _teamColorsSO.LilyAdditionalElementsColor);
                    break;
                case Teams.Neutral:
                    SetColorForList(_baseFloorColorSetter, _teamColorsSO.NeutralBaseColor);
                    SetColorForList(_spireColorSetter, _teamColorsSO.NeutralSpireColor);
                    SetColorForList(_additionalElementsColorSetter, _teamColorsSO.NeutralAdditionalElementsColor);
                    break;
                case Teams.Multi:
                    SetColorForList(_baseFloorColorSetter, _teamColorsSO.MultiBaseColor);
                    SetColorForList(_spireColorSetter, _teamColorsSO.MultiSpireColor);
                    SetColorForList(_additionalElementsColorSetter, _teamColorsSO.MultiAdditionalElementsColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        private void SetColorForList(List<ColorSetter> colorSetters, Color whatColor)
        {
            foreach (var colorSetter in colorSetters)
            {
                colorSetter.SetColor(whatColor);
            }
        }
    }
}

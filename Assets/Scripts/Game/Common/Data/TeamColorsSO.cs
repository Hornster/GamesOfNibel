using System;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data
{
    [CreateAssetMenu(fileName = TeamColorsSOName,
        menuName = SGConstants.SGSOMenuPath + SGConstants.SGSOColorsRelativePath + TeamColorsSOName)]
    public class TeamColorsSO : ScriptableObject
    {
        public const string TeamColorsSOName = "TeamColorsSO";
        //Values taken from GoNTeamColors.colors for bases
        [SerializeField]
        private Color _lilyAdditionalElements = new Color(0.5529412f, 0.40000004f, 1, 0.09803922f);
        [SerializeField]
        private Color _lilySpire = new Color(0.40291423f, 0.39999998f, 1, 1);
        [SerializeField]
        private Color _lilyBase = new Color(0.5524127f, 0.39999998f, 1, 1);
        [SerializeField]
        private Color _lotusAdditionalElements = new Color(0.9538226f, 1, 0.19339621f, 0.09803922f);
        [SerializeField]
        private Color _lotusSpire = new Color(0.8914445f, 0.8962264f, 0.2494215f, 1);
        [SerializeField]
        private Color _lotusBase = new Color(0.7515387f, 0.7735849f, 0.054734774f, 1);
        [SerializeField]
        public Color _neutralAdditionalElements = _teamNeutralColor;
        [SerializeField]
        public Color _neutralSpire = _teamNeutralColor;
        [SerializeField]
        public Color _neutralBase = _teamNeutralColor;
        [SerializeField]
        private Color _multiAdditionalElements = new Color(0.3882353f, 1, 0.9955552f, 0.09803922f);
        [SerializeField]
        private Color _multiSpire = new Color(0.3882353f, 1, 0.99607843f, 1);
        [SerializeField]
        private Color _multiBase = new Color(0.19166073f, 0.6886792f, 0.6854933f, 1);

        public Color LilyAdditionalElementsColor => _lilyAdditionalElements;
        public Color LilySpireColor => _lilySpire;
        public Color LilyBaseColor => _lilyBase;
        public Color LotusAdditionalElementsColor => _lotusAdditionalElements;
        public Color LotusSpireColor => _lotusSpire;
        public Color LotusBaseColor => _lotusBase;
        public Color NeutralAdditionalElementsColor => _neutralAdditionalElements;
        public Color NeutralSpireColor => _neutralSpire;
        public Color NeutralBaseColor => _neutralBase;
        public Color MultiAdditionalElementsColor => _multiAdditionalElements;
        public Color MultiSpireColor => _multiSpire;
        public Color MultiBaseColor => _multiBase;

        //Default values
        [SerializeField]
        private static Color _teamLilyColor = new Color(0.541176f, 0.5098039f, 0.92549f);
        [SerializeField]
        private static Color _teamLotusColor = new Color(0.99215f, 0.98039f, 0.4745098f);
        [SerializeField]
        private static Color _teamNeutralColor = new Color(0.9843137f, 0.97647f, 0.98823529f);
        [SerializeField]
        private static Color _teamMultiColor = new Color(0.388235f, 1.0f, 0.996078f);
        //#63FFFE 99 255 254
        public static Color GetTeamColor(Teams team)
        {
            switch (team)
            {
                case Teams.Lotus:
                    return _teamLotusColor;
                case Teams.Lily:
                    return _teamLilyColor;
                case Teams.Neutral:
                    return _teamNeutralColor;
                case Teams.Multi:
                    return _teamMultiColor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, "Team color not found!");
            }
        }

        public Color GetBaseColor(Teams team, BaseComponentTypeEnum componentType)
        {
            Color color;
            switch(team)
            {
                case Teams.Lotus:
                    color = GetLotusColor(componentType);
                    break;
                case Teams.Lily:
                    color = GetLilyColor(componentType);
                    break;
                case Teams.Neutral:
                    color = GetNeutralColor(componentType);
                    break;
                case Teams.Multi:
                    color = GetMultiColor(componentType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown team {team}!");
            }

            return color;
        }

        private Color GetLotusColor(BaseComponentTypeEnum componentType)
        {
            switch(componentType)
            {
                case BaseComponentTypeEnum.FloorObjectID:
                    return _lotusBase;
                case BaseComponentTypeEnum.SpireObjectID:
                    return _lotusSpire;
                case BaseComponentTypeEnum.AdditionalElementsID:
                    return _lotusAdditionalElements;
                default:
                    throw new Exception("Unknown component type for team Lotus!");
            }
        }
        private Color GetLilyColor(BaseComponentTypeEnum componentType)
        {
            switch (componentType)
            {
                case BaseComponentTypeEnum.FloorObjectID:
                    return _lilyBase;
                case BaseComponentTypeEnum.SpireObjectID:
                    return _lilySpire;
                case BaseComponentTypeEnum.AdditionalElementsID:
                    return _lilyAdditionalElements;
                default:
                    throw new Exception("Unknown component type for team Lily!");
            }
        }
        private Color GetNeutralColor(BaseComponentTypeEnum componentType)
        {
            switch (componentType)
            {
                case BaseComponentTypeEnum.FloorObjectID:
                    return _neutralBase;
                case BaseComponentTypeEnum.SpireObjectID:
                    return _neutralSpire;
                case BaseComponentTypeEnum.AdditionalElementsID:
                    return _neutralAdditionalElements;
                default:
                    throw new Exception("Unknown component type for team Neutral!");
            }
        }
        private Color GetMultiColor(BaseComponentTypeEnum componentType)
        {
            switch (componentType)
            {
                case BaseComponentTypeEnum.FloorObjectID:
                    return _multiBase;
                case BaseComponentTypeEnum.SpireObjectID:
                    return _multiSpire;
                case BaseComponentTypeEnum.AdditionalElementsID:
                    return _multiAdditionalElements;
                default:
                    throw new Exception("Unknown component type for team Multi!");
            }
        }
    }
}

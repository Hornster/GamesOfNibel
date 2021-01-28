using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data
{
    public class TeamColors
    {
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
            switch(team)
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
    }
}

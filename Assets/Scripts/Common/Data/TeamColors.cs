using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data
{
    public class TeamColors : MonoBehaviour
    {
        [SerializeField]
        private static Color _teamLilyColor = new Color(0.541176f, 0.5098039f, 0.92549f);
        [SerializeField]
        private static Color _teamLotusColor = new Color(0.99215f, 0.98039f, 0.4745098f);
        [SerializeField]
        private static Color _teamNeutralColor = new Color(0.9843137f, 0.97647f, 0.98823529f);

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, "Team color not found!");
            }
        }
    }
}

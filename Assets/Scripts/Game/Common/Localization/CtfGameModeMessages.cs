using System;
using Assets.Scripts.Common.Enums;

namespace Assets.Scripts.Common.Localization
{
    /// <summary>
    /// Stores messages for the Ctf game mode.
    /// </summary>
    public class CtfGameModeMessages
    {
        private string ReplacementMark = "$";

        public string GetTeamName(Teams whichTeam)
        {
            switch(whichTeam)
            {
                case Teams.Lotus:
                    return TeamLotusName;
                case Teams.Lily:
                    return TeamLilyName;
                case Teams.Neutral:
                    return TeamNeutralName;
                default:
                    throw new ArgumentOutOfRangeException(nameof(whichTeam), whichTeam, "Such team does not exist!");
            }
        }
        /// <summary>
        /// Returns victory quote, inserting provided team name in it.
        /// </summary>
        /// <param name="whichTeam">What team has won.</param>
        /// <returns></returns>
        public string GetVictoryQuote(Teams whichTeam)
        {
            var teamName = GetTeamName(whichTeam);
            string message = VictoryQuote.Replace(ReplacementMark, teamName);

            return message;
        }

        public static string TeamLotusName = "Lotus";
        public static string TeamLilyName = "Lily";
        public static string TeamNeutralName = "Neutral";
        public static string VictoryQuote = "Team $ is victorious!";
    }
}

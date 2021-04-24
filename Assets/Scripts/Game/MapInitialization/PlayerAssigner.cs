using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Spawner.PlayerSpawner;
using UnityEngine;

namespace Assets.Scripts.Game.MapInitialization
{
    /// <summary>
    /// Used to position created players on their bases.
    /// Team bases have priority over multi-team bases.
    /// </summary>
    public class PlayerAssigner : MonoBehaviour
    {
        /// <summary>
        /// Assigns players to bases and positions them accordingly.
        /// </summary>
        /// <param name="sceneData"></param>
        public void PositionPlayers(SceneData sceneData)
        {
            var bases = sceneData.Bases;
            var players = sceneData.Players;

            AssignAllPlayersToBases(players, bases);
        }

        private void AssignAllPlayersToBases(Dictionary<Teams, List<GameObject>> players, Dictionary<Teams, List<GameObject>> bases)
        {
            var teams = players.Keys;
            foreach (var team in teams)
            {
                //No need to check if the entry exists - we are using keys from this
                //very dictionary.
                players.TryGetValue(team, out var currentTeam);
                List<GameObject> teamBases;
                switch (team)
                {
                    case Teams.Lotus:
                    case Teams.Lily:
                    case Teams.Neutral:
                        if (bases.TryGetValue(team, out teamBases))
                        {
                            AssignPlayersToBases(currentTeam, teamBases);
                        }
                        else if (AssignToMultiTeamBase(currentTeam, bases) == false)
                        {
                            throw new GONBaseException($"Found players that cannot be assigned to any base! Players team: {team}");
                        }
                        break;
                    case Teams.Multi:
                        if (bases.TryGetValue(team, out teamBases))
                        {
                            AssignPlayersToBases(currentTeam, teamBases);
                        }
                        else
                        {
                            throw new GONBaseException($"Found players that cannot be assigned to any base! Players team: {team}");
                        }
                        break;
                    default:
                        throw new GONBaseException($"Unknown team found during assigning players to spawns! {team}");
                }
            }
        }
        private bool AssignToMultiTeamBase(List<GameObject> players,  Dictionary<Teams, List<GameObject>> basesDictionary)
        {
            if (basesDictionary.TryGetValue(Teams.Multi, out var multiTeamBases))
            {
                AssignPlayersToBases(players, multiTeamBases);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Assigns players to the first of provided bases and causes them to be repositioned to its player spawn point.
        /// </summary>
        /// <param name="players">Players of the same team as bases.</param>
        /// <param name="bases">Bases of the same team as players.</param>
        private void AssignPlayersToBases(List<GameObject> players, List<GameObject> bases)
        {
            //TODO Allow for configuration of spawn assignment perhaps? For example random. In the future.
            var targetBase = bases[0].GetComponentInChildren<PlayerPositioner>();
            foreach (var player in players)
            {
                var positioner = player.GetComponentInChildren<IRepositioner>();
                targetBase.AssignPlayer(positioner);
            }
            //Do NOT reposition players now. Wait for the initialization to finish, then
            //reposition players when on the map, right after moving the spawns.
        }
    }
}

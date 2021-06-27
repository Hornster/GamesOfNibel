using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Player;
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

            bases = FilterBasesByGameplayMode(bases, sceneData.GameplayMode);
            AssignAllPlayersToBases(players, bases);
        }
        /// <summary>
        /// Returns only bases that have the same gameplay mode set.
        /// </summary>
        /// <param name="bases"></param>
        /// <param name="gameplayMode"></param>
        /// <returns></returns>
        private Dictionary<Teams, List<GameObject>> FilterBasesByGameplayMode(Dictionary<Teams, List<GameObject>> bases, GameplayModesEnum gameplayMode)
        {
            var filteredBases = new Dictionary<Teams, List<GameObject>>();
            foreach(var team in bases)
            {
                var filteredBasesForTeam = new List<GameObject>();

                foreach (var checkedBase in team.Value)
                {
                    var baseData = checkedBase.GetComponent<BaseDataGOAdapter>();
                    if(baseData.GameMode == gameplayMode)
                    {
                        filteredBasesForTeam.Add(checkedBase);
                    }
                }
                filteredBases.Add(team.Key, filteredBasesForTeam);
            }

            return filteredBases;
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
                var playerID = player.gameObject.GetInstanceID();
                var positioner = player.GetComponentInChildren<IRepositioner>();
                var state = player.GetComponentInChildren<PlayerReset>();
                //TODO null exception happened here. Test if it still happens - base didn't have player repositioner script attached.
                //TODO you added filtering of the bases so only the same gamemode ones stay. Should work now
                //TODO also, maybe make it round robin type of player setting up.
                targetBase.AssignPlayer(playerID, positioner, state);
            }
            //Do NOT reposition players now. Wait for the initialization to finish, then
            //reposition players when on the map, right after moving the spawns.
        }
    }
}

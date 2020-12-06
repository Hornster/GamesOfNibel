using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.MapInitialization
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
            bool wasMultiteamBasePresent = false;

            var spawners = sceneData.Spawners;
            var players = EnlistPlayers(sceneData.Players);

            var teams = spawners.Keys;
            foreach (var team in teams)
            {
                if (team == Teams.Multi)
                {
                    wasMultiteamBasePresent = true; //These shall be checked the last. Specialized teams (like lily) have priority.
                }
                if (team == Teams.Neutral)
                {
                    continue;   //Neutral bases cannot have any players assigned.
                }
                if (spawners.TryGetValue(team, out var oneTeamSpawners) == false)
                {
                    continue;
                }

                SetPlayersPositions(oneTeamSpawners, players);
            }
        }
        /// <summary>
        /// Retrieves teams and repositioning components of the players.
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        private List<(Teams, IRepositioner)> EnlistPlayers(List<GameObject> players)
        {
            var playersQueue = new List<(Teams, IRepositioner)>();
            foreach (var player in players)
            {
                var team = player.GetComponentInChildren<TeamModule>().MyTeam;
                var repositioner = player.GetComponentInChildren<IRepositioner>();
                playersQueue.Add((team, repositioner));
            }

            return playersQueue;
        }

        private void SetPlayersPositions(List<GameObject> spawners, List<(Teams, IRepositioner)> players)
        {
            //TODO Allow for configuration of spawn assignment perhaps? For example random. In the future.

            //TODO - Sort players right after their creation (or during it, even) and assign them to dictionary.
            //TODO This way you will be able to call both dictionaries with use of teams, simply.
            var assignedToSpawner = spawners[0];
            var assignedPlayersIndices = new List<int>();

            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].Item1 == )
            }
        }
    }
}

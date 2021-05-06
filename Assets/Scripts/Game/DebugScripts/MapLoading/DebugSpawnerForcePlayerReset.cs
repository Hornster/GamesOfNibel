using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Spawner.PlayerSpawner;
using UnityEngine;

namespace Assets.Scripts.Game.DebugScripts.MapLoading
{
    public class DebugSpawnerForcePlayerReset : MonoBehaviour
    {
        [SerializeField] private bool _resetPlayer = false;
        [SerializeField] private int _playerID = 0;
        [SerializeField] private PlayerPositioner _playerPositioner;
        private void Update()
        {
            if (_resetPlayer)
            {
                if (_playerPositioner != null)
                {
                    _playerPositioner.ResetPlayer(_playerID);
                    _resetPlayer = false;
                    Debug.Log($"Resetted player of id {_playerID}");
                }
            }
        }
    }
}

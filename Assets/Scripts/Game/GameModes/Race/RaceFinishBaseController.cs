using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.CustomEvents;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Data.Constants;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.GameModes.Race
{
    //TODO Make serverside
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaceFinishBaseController : MonoBehaviour, IInjectionHook
    {
        private TeamModule _myTeam;
        private IntegerUnityEvent _playerArrived = new IntegerUnityEvent();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ChkForPlayer(collider);
        }

        private void ChkForPlayer(Collider2D collider)
        {
            var playerController = collider.GetComponent<PlayerController>();
            if (playerController == null)
            {
                return;
            }

            if (_myTeam.MyTeam == Teams.Multi || _myTeam.MyTeam == playerController.MyTeam)
            {
                _playerArrived?.Invoke(playerController.PlayerID);
            }
            
        }

        public void InjectReferences(GameObject source)
        {
            _myTeam = source.GetComponent<TeamModule>();
            if (_myTeam == null)
            {
                throw new Exception(ErrorMessages.InjectionHookErrorNoRefFound);
            }
        }

        public void RegisterPlayerArrivedEvent(UnityAction<int> handler)
        {
            _playerArrived.AddListener(handler);
        }
    }
}

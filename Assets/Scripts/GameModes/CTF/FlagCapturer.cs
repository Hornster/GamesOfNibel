using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameModes.CTF
{
    /// <summary>
    /// Defines the behavior of entities that can capture a flag, like bases.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class FlagCapturer : MonoBehaviour, IFlagCarrier
    {
        /// <summary>
        /// Called when flags have been captured by this object.
        /// Passes amount of captured flags as an argument.
        /// </summary>
        [SerializeField] private UnityEvent<int> _capturedFlag;
        [SerializeField]
        private Teams _myTeam;

        private Queue<IFlag> _capturedFlags;

        public Transform MyTransform => gameObject.transform;
        public Teams MyTeam => _myTeam;
        public bool HasFlag { get; private set; }

        //TODO Add capturing the flag from running players that collide with you.

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var flagCarrierComponent = collider.gameObject.GetComponent<IFlagCarrier>();
            if (flagCarrierComponent != null)
            {
                if (flagCarrierComponent.MyTeam == _myTeam && flagCarrierComponent.HasFlag)
                {
                    var takenOverFlag = flagCarrierComponent.TakeOverFlag();
                    takenOverFlag.CaptureFlag(this);
                    _capturedFlags.Enqueue(takenOverFlag);

                    Debug.Log($"Team {_myTeam} captured flag.");
                    _capturedFlag?.Invoke(1);//For now, only onne flag can be captured at any given time.
                }
            }
        }

        /// <summary>
        /// Returns one of the stored flags. If no flags left - returns null.
        /// Worth checking the HasFlag first.
        /// </summary>
        /// <returns></returns>
        public IFlag TakeOverFlag()
        {
            if (_capturedFlags.Count <= 0)
            {
                HasFlag = false;
                return null;
            }

            var givenAwayFlag = _capturedFlags.Dequeue();

            return givenAwayFlag;
        }
    }
}

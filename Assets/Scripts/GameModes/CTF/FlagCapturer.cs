using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using Assets.Scripts.GameModes.CTF.Observers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameModes.CTF
{
    /// <summary>
    /// Defines the behavior of entities that can capture a flag, like bases.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class FlagCapturer : MonoBehaviour, IFlagCarrier, IFlagCapturedObserver
    {
        /// <summary>
        /// Called when flags have been captured by this object.
        /// Passes amount of captured flags as an argument.
        /// </summary>
        [SerializeField]
        private UnityEvent<int> _capturedFlag;
        [SerializeField]
        private TeamModule _myTeam;

        private readonly Queue<IFlag> _capturedFlags = new Queue<IFlag>();
        /// <summary>
        /// The position of the first carried flag.
        /// </summary>
        [SerializeField]
        private Transform _flagPosition;

        public Transform MyTransform => gameObject.transform;
        public Transform FlagPosition => _flagPosition.transform;
        public Teams MyTeam => _myTeam.MyTeam;
        public bool HasFlag { get; private set; }
        

        private void OnTriggerEnter2D(Collider2D collider)
        {
            //Check if a player has touched this capturing entity and if they have flag.
            var flagCarrierComponent = collider.gameObject.GetComponent<IFlagCarrier>();
            if (flagCarrierComponent != null)
            {
                if (flagCarrierComponent.MyTeam == _myTeam.MyTeam && flagCarrierComponent.HasFlag)
                {
                    var takenOverFlag = flagCarrierComponent.TakeOverFlag(0);
                    CaptureFlag(takenOverFlag);
                }

                return; //No need to go further.
            }
            //If we got here, then something else is colliding with the capturing entity.
            //Check if a flag did not fell/roll over the capturing entity.
            var flagComponent = collider.gameObject.GetComponent<IFlag>();
            if (flagComponent != null)
            {
                if (flagComponent.IsCarried == false)
                {
                    CaptureFlag(flagComponent);
                }
            }
        }
        /// <summary>
        /// Captures the flag, setting this object as it's owner and changing the flag's properties.
        /// </summary>
        /// <param name="capturedFlag"></param>
        private void CaptureFlag(IFlag capturedFlag)
        {
            capturedFlag.CaptureFlag(this);
            _capturedFlags.Enqueue(capturedFlag);

            Debug.Log($"Team {_myTeam} captured flag.");
            _capturedFlag?.Invoke(1);//For now, only one flag can be captured at any given time.
        }

        /// <summary>
        /// Returns one of the stored flags. If no flags left - returns null.
        /// Worth checking the HasFlag first.
        /// </summary>
        /// <returns></returns>
        public IFlag TakeOverFlag(int takerID)
        {
            if (_capturedFlags.Count <= 0)
            {
                HasFlag = false;
                return null;
            }

            var givenAwayFlag = _capturedFlags.Dequeue();

            return givenAwayFlag;
        }
        /// <summary>
        /// Captures a flag that rolled close enough.
        /// </summary>
        /// <param name="flag"></param>
        public void PickedUpFlag(IFlag flag)
        {
            CaptureFlag(flag);
        }

        public void RegisterObserver(UnityAction<Teams, int> handler)
        {
            //
            throw new NotImplementedException();
        }
    }
}

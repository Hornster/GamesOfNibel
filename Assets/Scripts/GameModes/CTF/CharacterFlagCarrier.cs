
using System;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameModes.CTF
{
    /// <summary>
    /// Allows the player  object to carry flags.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class CharacterFlagCarrier : MonoBehaviour, IFlagCarrier
    {
        /// <summary>
        /// Remembers the flag that is currently being carried. When is not
        /// carrying any flag - is null.
        /// </summary>
        private IFlag _carriedFlag;

        private int _flagTakerID;
        /// <summary>
        /// Defines if the flag has just been taken away from this character.
        /// If the flag has just been taken, then the character cannot take the flag back until they
        /// stop colliding with the flag taker.
        /// </summary>
        private bool _flagWasJustTaken;

        /// <summary>
        /// Used to change the colors of the character's elements accordingly to their team.
        /// </summary>
        [SerializeField]
        private UnityColorEvent _changeColor;

        /// <summary>
        /// The position of the first carried flag.
        /// </summary>
        [SerializeField]
        private Transform _flagPosition;
        [SerializeField]
        private Teams _myTeam;
        public Teams MyTeam => _myTeam;

        public Transform MyTransform => gameObject.transform;
        public Transform FlagPosition => _flagPosition.transform;
        public bool HasFlag { get; private set; }

        private void Start()
        {
            var color = TeamColors.GetTeamColor(_myTeam);
            _changeColor?.Invoke(color);
        }

        //TODO The flag itself can be taken over by touching it. This most likely causes logic errors and blinking.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var otherFlagCarrier = collider.gameObject.GetComponent<IFlagCarrier>();

            if (otherFlagCarrier != null)
            {
                if (otherFlagCarrier.MyTeam != MyTeam && otherFlagCarrier.HasFlag)
                {
                    int takerHash = otherFlagCarrier.GetHashCode();
                    if (_flagWasJustTaken && takerHash == _flagTakerID)
                    {
                        return;//That player has just taken the flag away from us. We need to wait until we stop colliding with them.
                    }

                    int myHash = this.GetHashCode();
                    var flag = otherFlagCarrier.TakeOverFlag(myHash);
                    PickedUpFlag(flag);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var otherFlagCarrier = collider.gameObject.GetComponent<IFlagCarrier>();

            if (otherFlagCarrier != null)
            {
                int takerHash = otherFlagCarrier.GetHashCode();
                if (_flagWasJustTaken && takerHash == _flagTakerID)
                {
                    //When we left the taker's collider, we can recapture our flag.
                    _flagWasJustTaken = false;
                    _flagTakerID = -1;
                }
            }
        }
        
        /// <summary>
        /// Returns the carried flag. If no flag - returns null.
        /// Worth checking the HasFlag first.
        /// </summary>
        /// <param name="takerID">Unique ID of the object that taken over the flag.</param>
        /// <returns></returns>
        public IFlag TakeOverFlag(int takerID)
        {
            _flagWasJustTaken = true;
            _flagTakerID = takerID;
            HasFlag = false;
            var carriedFlag = _carriedFlag;
            _carriedFlag = null;

            return carriedFlag;
        }
        /// <summary>
        /// Picked up a flag from the ground/air/water/branch smasher.
        /// </summary>
        /// <param name="flag">Picked up flag.</param>
        public void PickedUpFlag(IFlag flag)
        {
            HasFlag = true;
            flag.WasTakenOverBy(this);
            _carriedFlag = flag;
        }
        /// <summary>
        /// Specialization of a generic event.
        /// </summary>
        [Serializable]
        private class UnityColorEvent : UnityEvent<Color> { };
    }
}

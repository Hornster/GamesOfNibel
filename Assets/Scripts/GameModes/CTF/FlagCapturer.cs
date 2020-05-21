using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF
{
    /// <summary>
    /// Defines the behavior of entities that can capture a flag, like bases.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class FlagCapturer : MonoBehaviour, IFlagCarrier
    {
        [SerializeField]
        private Teams _myTeam;

        private Queue<IFlag> _capturedFlags;

        public Transform MyTransform => gameObject.transform;
        public Teams MyTeam => _myTeam;
        public bool HasFlag { get; private set; }

        //TODO Add capturing the flag from running players that collide with you.

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

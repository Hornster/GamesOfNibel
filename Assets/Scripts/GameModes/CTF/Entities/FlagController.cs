
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Spawner;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF.Entities
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FlagController : MonoBehaviour, IFlag
    {
        /// <summary>
        /// Defines what objects can pick up the flag.
        /// </summary>
        [SerializeField]
        private LayerMask _whatCanTakeFlag;
        /// <summary>
        /// Defines which objects can capture the flag.
        /// </summary>
        [SerializeField]
        private LayerMask _whatCanCaptureFlag;
        [SerializeField]
        private Transform _flagTransform;
        /// <summary>
        /// The transform of the flag carrying player.
        /// </summary>
        private Transform _flagCarrierTransform;

        public Teams MyTeam => _myTeam;
        /// <summary>
        /// The team that this flag belongs to.
        /// </summary>
        [SerializeField]
        private Teams _myTeam;
        /// <summary>
        /// Indicates what team carries the flag.
        /// </summary>
        private Teams _carriedByTeam;
        /// <summary>
        /// Is the flag carried by someone or is it resting at spawn/on the ground?
        /// Only neutral flags and these that have been dropped are not carried. Already captured
        /// flag is being carried by the spawn that captured the flag.
        /// </summary>
        private bool _isCarried;
        
        [SerializeField]
        private SpriteRenderer _flagSpriteRenderer;

        private void Start()
        {
            SetColor(_myTeam);
        }

        private void FixedUpdate()
        {
            UpdatePosition();
        }
        
        /// <summary>
        /// Updates the global position  of the flag to the carrying character's one.
        /// </summary>
        private void UpdatePosition()
        {
            if (_isCarried == false)
            {
                return;
            }
            var posAboveCarrier = _flagCarrierTransform.position;
            _flagTransform.position = posAboveCarrier;
        }
        /// <summary>
        /// Checks if the entering collider can pickup the flag.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var colliderGameobject = collider.gameObject;
            int colliderLayerValue = colliderGameobject.layer;
            colliderLayerValue = MathOperations.ConvertLayerIndexToLayerMaskValue(colliderLayerValue);

            int testLayers = colliderLayerValue & _whatCanTakeFlag.value;

            if (testLayers != 0)
            {
                //The colliding object is eliglibe for picking up the flag.
                var flagCarrierScript = colliderGameobject.GetComponent<IFlagCarrier>();
                if (flagCarrierScript != null && !_isCarried)
                {
                    //The object can pick up the flag from the ground or neutral spawn.
                    if (flagCarrierScript.MyTeam != _carriedByTeam || flagCarrierScript.MyTeam != _myTeam)
                    {
                        WasTakenOverBy(flagCarrierScript);
                        flagCarrierScript.PickedUpFlag(this);
                    }
                }
            }
        }

        /// <summary>
        /// Reassigns the flag to this object.
        /// </summary>
        /// <param name="takingEntity"></param>
        private void ReassignFlag(IFlagCarrier takingEntity)
        {
            _flagCarrierTransform = takingEntity.FlagPosition;
            _isCarried = true;
            _carriedByTeam = takingEntity.MyTeam;
        }


        /// <summary>
        /// Called when a player takes the flag from another player.
        /// </summary>
        /// <param name="newCarrier"></param>
        public void WasTakenOverBy(IFlagCarrier newCarrier)
        {
            ReassignFlag(newCarrier);
        }

        /// <summary>
        /// Called when the flag was delivered to a base and was captured.
        /// </summary>
        /// <param name="capturingEntity">Te entity that captured the flag.</param>
        public void CaptureFlag(IFlagCarrier capturingEntity)
        {
            ReassignFlag(capturingEntity);
            _myTeam = capturingEntity.MyTeam;
            SetColor(_myTeam);
        }
        /// <summary>
        /// Sets the color of the flag.
        /// </summary>
        /// <param name="teamColor"></param>
        public void SetColor(Teams teamColor)
        {
            _flagSpriteRenderer.color = TeamColors.GetTeamColor(teamColor);
        }
        /// <summary>
        /// Changes the team which the flag belongs to.
        /// </summary>
        /// <param name="newTeam">New team of the flag.</param>
        public void SetFlagData(FlagIniData flagIniData)
        {
            _myTeam = flagIniData.FlagTeam;
            SetColor(_myTeam);
        }
        //TODO - upon creating of the flag:
        //TODO set the respawn position in SetFlagData
        //TODO set the respawn signal callback
        //TODO upon capturing the flag, given base could send it's data in FlagIniData class to reconfigure the flag.
    }
}

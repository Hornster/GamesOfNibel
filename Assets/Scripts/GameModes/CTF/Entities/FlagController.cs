
using Assets.Scripts.Common;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Spawner;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameModes.CTF.Entities
{
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    public class FlagController : MonoBehaviour, IFlag
    {
        /// <summary>
        /// The time after which the flag will reset itself back to its spawn.
        /// </summary>
        [SerializeField]
        private float _awaitUnstuckTime;
        /// <summary>
        /// The amount of time it takes for the same player that just dropped the
        /// flag to pick it up again, in seconds.
        /// </summary>
        [SerializeField] private float _flagDropCooldown = 1.0f;
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
        /// The team that this flag belongs to.
        /// </summary>
        [SerializeField]
        private TeamModule _myTeam;
        /// <summary>
        /// The transform of the flag carrying player.
        /// </summary>
        private Transform _flagCarrierTransform;
        /// <summary>
        /// The transform of the spawner which the flag currently belongs to.
        /// </summary>
        private Transform _flagSpawnerTransform;

        public Teams MyTeam => _myTeam.MyTeam;
        /// <summary>
        /// Indicates what team carries the flag.
        /// </summary>
        private Teams _carriedByTeam;
        /// <summary>
        /// Callback to the spawn - called when the flag has been unstuck.
        /// </summary>
        private UnityAction _notifySpawnFlagUnstuck;
        private Timer _unstuckTimer;
        private Rigidbody2D _rb;
        /// <summary>
        /// Measures time since the flag being dropped by a player.
        /// </summary>
        private Timer _flagDropTimer;
        /// <summary>
        /// Is the flag carried by someone or is it resting at spawn/on the ground?
        /// Only neutral flags and these that have been dropped are not carried. Already captured
        /// flag is being carried by the spawn that captured the flag.
        /// </summary>
        public bool IsCarried { get; private set; }


        [SerializeField]
        private SpriteRenderer _flagSpriteRenderer;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            DisableRigidbody();
            _unstuckTimer = new Timer(_awaitUnstuckTime, ResetFlag);
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            _unstuckTimer.Update();
        }

        /// <summary>
        /// Updates the global position  of the flag to the carrying character's one.
        /// </summary>
        private void UpdatePosition()
        {
            if (IsCarried == false)
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
                if (flagCarrierScript != null && !IsCarried)
                {
                    //The object can pick up the flag from the ground or neutral spawn.
                    if (flagCarrierScript.MyTeam != _carriedByTeam || flagCarrierScript.MyTeam != _myTeam.MyTeam)
                    {
                        WasTakenOverBy(flagCarrierScript);
                        flagCarrierScript.PickedUpFlag(this);
                        _unstuckTimer.Stop();
                        _unstuckTimer.Reset();
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
            DisableRigidbody();
            _flagCarrierTransform = takingEntity.FlagPosition;
            IsCarried = true;
            _carriedByTeam = takingEntity.MyTeam;
        }
        /// <summary>
        /// Disables the rigidbody of the flag.
        /// </summary>
        private void DisableRigidbody()
        {
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
        }
        /// <summary>
        /// Enables the rigidbody.
        /// </summary>
        private void EnableRigidbody()
        {
            _rb.isKinematic = false;
            //TODO add velocity request from the character's rigidbody perhaps? Event-based
        }
        /// <summary>
        /// Resets the flag back to its spawn.
        /// </summary>
        private void ResetFlag()
        {
            DisableRigidbody();
            _flagTransform.position = _flagSpawnerTransform.position;
            _flagCarrierTransform = null;
            IsCarried = false;
            _carriedByTeam = _myTeam.MyTeam;
            _unstuckTimer.ResetAndStop();
            _notifySpawnFlagUnstuck?.Invoke();
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
            _myTeam.MyTeam = capturingEntity.MyTeam;
            SetColor(_myTeam.MyTeam);
            _unstuckTimer.ResetAndStop();
        }
        /// <summary>
        /// Carried flag has been dropped on the floor.
        /// </summary>
        public void DropCarriedFlag()
        {
            EnableRigidbody();
            IsCarried = false;
            _unstuckTimer.Start();
            _carriedByTeam = Teams.Neutral;
            _flagCarrierTransform = null;
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
        /// <param name="flagIniData">Config data for the flag.</param>
        public void SetFlagData(FlagIniData flagIniData)
        {
            _myTeam.MyTeam = flagIniData.FlagTeam;
            SetColor(_myTeam.MyTeam);
            _flagSpawnerTransform = flagIniData.FlagSpawnPosition;
            _notifySpawnFlagUnstuck = flagIniData.FlagUnstuckSignal;
        }
        //TODO - upon creating of the flag:
        //TODO set the respawn position in SetFlagData
        //TODO set the respawn signal callback
        //TODO upon capturing the flag, given base could send it's data in FlagIniData class to reconfigure the flag.

    }
}

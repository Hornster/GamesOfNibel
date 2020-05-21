
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
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
        /// <summary>
        /// Transform of the area that spawned the flag.
        /// </summary>
        [SerializeField]
        private Transform _homeSpawnTransform;
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
        /// Sets the color of the flag.
        /// </summary>
        /// <param name="teamColor"></param>
        public void SetColor(Teams teamColor)
        {
            _flagSpriteRenderer.color = TeamColors.GetTeamColor(teamColor);
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
        /// 
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var colliderGameobject = collider.gameObject;
            int colliderLayerValue = colliderGameobject.layer;
            colliderLayerValue = MathOperations.ConvertLayerIndexToLayerMaskValue(colliderLayerValue);

            int testLayers = colliderLayerValue & _whatCanTakeFlag.value;

            if (testLayers != 0)
            {
                var flagCarrierScript = colliderGameobject.GetComponent<IFlagCarrier>();
                if (flagCarrierScript != null)
                {
                    if (flagCarrierScript.MyTeam != _carriedByTeam || flagCarrierScript.MyTeam != _myTeam)
                    {
                        WasTakenOverBy(flagCarrierScript);
                        //TODO: Should the flag be taken away by player of the same team from the carrier?
                        //TODO: Should the flag be taken away when it directly collides with enemy? YES
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="takingEntity"></param>
        private void ReassignFlag(IFlagCarrier takingEntity)
        {
            _flagCarrierTransform = takingEntity.MyTransform;
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
        }
    }
}

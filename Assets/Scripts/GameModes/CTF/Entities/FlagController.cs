
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Helpers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF.Entities
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FlagController : MonoBehaviour
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
                    if (flagCarrierScript.MyTeam != _carriedByTeam)
                    {
                        FlagWasTakenOver(colliderGameobject.transform);
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
        private void FlagWasTakenOver(Transform takingEntity)
        {
            _flagCarrierTransform = takingEntity;
            _isCarried = true;
        }
    }
}

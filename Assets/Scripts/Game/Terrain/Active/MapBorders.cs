using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Terrain.Active
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MapBorders : MonoBehaviour
    {
        [Tooltip("What objects will this collider interact with.")]
        [SerializeField]
        private LayerMask _testedLayers;

        private BoxCollider2D _myCollider;

        private void Start()
        {
            _myCollider = GetComponent<BoxCollider2D>();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            int colliderLayer = MathOperations.ConvertLayerIndexToLayerMaskValue(collision.gameObject.layer);
            if (HasObjectCheckedLayers(colliderLayer) == false)
            {
                return; //This object is not among checked layers. Ignore.
            }

            var reset = collision.gameObject.GetComponent<IReset>();
            reset?.Reset();
        }
        /// <summary>
        /// Called when an entity leaves the object.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collider)
        {
            int colliderLayer = MathOperations.ConvertLayerIndexToLayerMaskValue(collider.gameObject.layer); 
            if (HasObjectCheckedLayers(colliderLayer) == false)
            {
                return; //This object is not among checked layers. Ignore.
            }

            var reset = collider.gameObject.GetComponent<IReset>();
            reset?.Reset();
        }
        /// <summary>
        /// Checks if provided layermask has common layers with the one assigned to this
        /// gameobject.
        /// </summary>
        /// <param name="layers">Layermask to check.</param>
        /// <returns></returns>
        private bool HasObjectCheckedLayers(LayerMask layers)
        {
            return (layers & _testedLayers) > 0;
        }
    }
}


using UnityEngine;

namespace Assets.Scripts.Game.DebugScripts.Common
{
    public class DebugDrawHorizontalLine : MonoBehaviour
    {
        /// <summary>
        /// Radius of the gizmo sphere to be drawn that indicates
        /// the position of the line source.
        /// </summary>
        [SerializeField] private float _radius;

        [SerializeField] private float _lineHalfWidth;

        /// <summary>
        /// The object which will be followed along the x and z axes.
        /// </summary>
        [SerializeField] private Transform _followedObject;

        private void FixedUpdate()
        {
            var followedObjPos = _followedObject.transform.position;
            var newPos = new Vector3(followedObjPos.x, transform.position.y, followedObjPos.z);
            transform.position = newPos;
        }

        private void OnDrawGizmos()
        {
            var thisPos = transform.position;
            var dir = Vector3.right * _lineHalfWidth;

            Debug.DrawRay(thisPos, dir);
            dir *= -1;
            Debug.DrawRay(thisPos, dir);

            Gizmos.DrawWireSphere(thisPos, _radius);
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.Game.Player.Physics
{
    /// <summary>
    /// Casts a singular ray basing on provided in the Unity inspector origin.
    /// </summary>
    public class Raycaster2D : MonoBehaviour, IRaycaster2D
    {
        /// <summary>
        /// Origin of the ray.
        /// </summary>
        [SerializeField]
        private Transform _origin;
        [SerializeField] private float _rayLength;
        /// <summary>
        /// The result of last ray cast. Changes every time CastRay method is called.
        /// </summary>
        private RaycastHit2D _lastHit;
        /// <summary>
        /// Casts a ray and returns the cast result, using provided in the unity inspector origin.
        /// </summary>
        /// <param name="direction">The direction in which the ray shall be cast. Should be a normalized vector.</param>
        /// <param name="whatCollides">Which layers will the ray collide with.</param>
        /// <returns></returns>
        public ref RaycastHit2D CastRay(Vector2 direction, LayerMask whatCollides)
        {
            _lastHit = Physics2D.Raycast(_origin.position, direction, _rayLength, whatCollides);

            Debug.DrawRay(_origin.position, direction * _rayLength, Color.red);

            return ref _lastHit;
        }
    }
}

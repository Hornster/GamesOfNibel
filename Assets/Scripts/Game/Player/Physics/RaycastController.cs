using UnityEngine;

namespace Assets.Scripts.Game.Player.Physics
{
    /// <summary>
    /// Manages casting of associated rays.
    /// </summary>
    public class RaycastController : MonoBehaviour
    {
        private IRaycaster2D[] _raycasters;
        /// <summary>
        /// The last, smallest hit. Changes every time the CastAllRays method is called!
        /// </summary>
        private RaycastHit2D _lastHit;

        private void Start()
        {
            _raycasters = GetComponentsInChildren<IRaycaster2D>();
        }
        /// <summary>
        /// Casts all rays associated with this controller.
        /// </summary>
        /// <param name="direction">The direction in which the rays shall be cast. Should be a normalized vector.</param>
        /// <param name="whatCollides">Which layers will the rays collide with.</param>
        /// <returns></returns>
        public ref RaycastHit2D CastAllRays(Vector2 direction, LayerMask whatCollides)
        {
            _lastHit = new RaycastHit2D();
            foreach (var raycaster in _raycasters)
            {
                var newHit = raycaster.CastRay(direction, whatCollides);
                if (newHit)//If we hit something
                {
                    if (_lastHit)//and already store a meaningful hit
                    {
                        if (newHit.distance < _lastHit.distance)
                        {//Assign new hit only if it was closer.
                            _lastHit = newHit;
                        }
                    }
                    else
                    {//We do not have any meaningful hit assigned yet, so feel free to assign the first one.
                        _lastHit = newHit;
                    }
                }
            }

            return ref _lastHit;
        }
    }
}

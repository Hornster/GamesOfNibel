using Assets.Scripts.Game.Player.Effects;
using UnityEngine;

namespace Assets.Scripts.Game.Terrain.Active
{
    /// <summary>
    /// Controls the jump plant behavior.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D), typeof(JumpPlantEffect))]
    public class JumpPlant : MonoBehaviour
    {
        private JumpPlantEffect _myEffect;

        private void Start()
        {
            _myEffect = GetComponent<JumpPlantEffect>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            //TODO play the animation later on or something.

            var effectManager = other.GetComponent<IEffectsManager>();
            effectManager?.AddPhysicalEffect(_myEffect);
        }
    }
}

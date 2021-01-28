using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.GUI.Menu
{
    /// <summary>
    /// Used to disable shaders on gameobject which is attached to.
    /// </summary>
    public class ControlShaderDisabler : MonoBehaviour
    {
        [SerializeField] private Image _shaderImage;

        public void DisableShader()
        {
            _shaderImage.enabled = false;
        }

        public void EnableShader()
        {
            _shaderImage.enabled = true;
        }
    }
}

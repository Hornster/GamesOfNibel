using UnityEngine;

namespace Assets.Scripts.Game.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _mainCamera;

        [SerializeField] private Transform _character;

        private void Update()
        {
            _mainCamera.position =  new Vector3(_character.position.x, _character.position.y, _mainCamera.position.z);
        }
    }
}

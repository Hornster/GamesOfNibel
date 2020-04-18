using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Camera
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

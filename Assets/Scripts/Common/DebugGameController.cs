using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class DebugGameController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _bottomBorder;

        private Vector3 _playerStartPos;

        private void Start()
        {
            InputReader.RegisterGameEnd(LeaveGame);
            _playerStartPos = _playerTransform.position;
        }

        private void FixedUpdate()
        {
            if (_playerTransform.position.y < _bottomBorder.position.y)
            {
                _playerTransform.position = _playerStartPos;
            }
        }
        private void LeaveGame()
        {
            Application.Quit();
        }
    }
}

using Assets.Scripts.Game.Player;
using UnityEngine;

namespace Assets.Scripts.Game.Common
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

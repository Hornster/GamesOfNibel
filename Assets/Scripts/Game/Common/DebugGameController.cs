using Assets.Scripts.Game.Player;
using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    public class DebugGameController : MonoBehaviour
    {
        private void Start()
        {
            InputReader.RegisterGameEnd(LeaveGame);
        }

        private void LeaveGame()
        {
            Application.Quit();
        }
    }
}

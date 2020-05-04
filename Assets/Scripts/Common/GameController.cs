using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class GameController : MonoBehaviour
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

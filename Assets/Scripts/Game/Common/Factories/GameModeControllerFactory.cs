using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    /// <summary>
    /// Creates game mode controllers.
    /// </summary>
    public class GameModeControllerFactory : MonoBehaviour
    {

        

        private GameObject CreateCTFController(GameObject gameModeUI)
        {
            throw new NotImplementedException();
        }
        public GameObject CreateGameController(GameObject gameModeUI, GameplayModesEnum gameplayMode)
        {
            switch (gameplayMode)
            {
                case GameplayModesEnum.CTF:
                    return CreateCTFController(gameModeUI);
                    break;
                case GameplayModesEnum.Race:
                    throw new NotImplementedException();
                case GameplayModesEnum.TimeAttack:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameplayMode), gameplayMode, "No such game mode present!");
            }
        }

        
    }
}
//TODO: Create spawning of CTF game controller, together with connecting it with UI and bases.
//TODO: Should there be a spawn trigger? As in, map fully loaded, start counting towards the flag spawn
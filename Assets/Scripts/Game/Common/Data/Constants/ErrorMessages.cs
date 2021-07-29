using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Data.Constants
{
    public class ErrorMessages
    {
        public const string RaceControllerPlayerGUIOverrideAttempt = "Tried to assign two GUIs for the same player!";
        public const string RaceControllerPlayerMatchDataOverrideAttempt = "Tried to assign two player match data containers for the same player!";
        public const string InjectionHookErrorNoRefFound = "Provided object did not have required reference.";

        public const string NeutralCTFFlagSpawnersNotFound = "No flag spawners found!";
        public const string NoGameplayControllerFound = "No game controller found for provided gameplay type!";
    }
}

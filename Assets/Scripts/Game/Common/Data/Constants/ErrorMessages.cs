using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Data.Constants
{
    public class ErrorMessages
    {
        public const string DuplicatePlayerIDFound = "Found duplicate player ID upon setting up the race manager!";
        public const string InjectionHookErrorNoRefFound = "Provided object did not have required reference.";

        public const string NeutralCTFFlagSpawnersNotFound = "No flag spawners found!";
        public const string NoGameplayControllerFound = "No game controller found for provided gameplay type!";
    }
}

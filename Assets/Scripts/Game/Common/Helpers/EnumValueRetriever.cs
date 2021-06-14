using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Helpers
{
    public class EnumValueRetriever 
    {
        public static T[] GetEnumArray<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)) as T[];
        }

        public static GameplayModesEnum GetGameplayModeFromBaseType(BaseTypeEnum baseType)
        {
            switch (baseType)
            {
                case BaseTypeEnum.CtfDefault:
                    return GameplayModesEnum.CTF;
                case BaseTypeEnum.RaceStart:
                case BaseTypeEnum.RaceFinish:
                    return GameplayModesEnum.Race;
                default:
                    throw new ArgumentOutOfRangeException($"No such base type as {baseType}!");
            }
        }
    }
}

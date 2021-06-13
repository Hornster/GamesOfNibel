using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Helpers
{
    public class EnumValueRetriever 
    {
        public static T[] GetEnumArray<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)) as T[];
        }
    }
}

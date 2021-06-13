using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Enums
{
    // Contains IDs of given components of bases and base markers.
    // These are used for example in factories to determine which component is which
    // using arrays, hence special care should be made when tampering with this enum.
    //
    //Make sure that all arrays that use these values are sequential and begin from 0.
    public enum BaseComponentTypeEnum
    {
        FloorObjectID = 0,
        SpireObjectID = 1,
        AdditionalElementsID = 2,
    }
}

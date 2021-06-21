using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;

namespace Assets.Scripts.Game.Common.Data.Maps
{
    /// <summary>
    /// Class directly used to serialize and deserialize base data.
    /// </summary>
    [Serializable]
    public class BaseData
    {
        /// <summary>
        /// Unique ID of the base.
        /// </summary>
        public int ID;
        public Teams BaseTeam;
        public GameplayModesEnum GameMode;
        public BaseTypeEnum BaseType;
    }
}

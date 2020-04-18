using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.Character.Skills
{
    public interface IBasicSkill
    {
        /// <summary>
        /// Makes given skill be used.
        /// </summary>
        void UseSkill();
    }
}

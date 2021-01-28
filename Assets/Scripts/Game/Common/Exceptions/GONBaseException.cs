using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Exceptions
{
    public class GONBaseException : Exception
    {
        public GONBaseException(string msg) : base(msg)
        {

        }
    }
}

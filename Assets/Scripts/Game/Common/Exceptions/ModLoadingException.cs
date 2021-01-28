using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Exceptions
{
    /// <summary>
    /// Exception for mod loading problems.
    /// </summary>
    public class ModLoadingException : Exception
    {
        public ModLoadingException()
        {
        }

        public ModLoadingException(string msg)
        :base(msg)
        {
            
        }
    }
}

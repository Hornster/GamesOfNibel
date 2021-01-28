using System;

namespace Assets.Scripts.Game.Common.Exceptions
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

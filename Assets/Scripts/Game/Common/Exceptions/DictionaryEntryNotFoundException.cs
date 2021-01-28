using System;

namespace Assets.Scripts.Game.Common.Exceptions
{
    /// <summary>
    /// Exception for retrieving values from dictionaries.
    /// </summary>
    public class DictionaryEntryNotFoundException : Exception
    {
        public DictionaryEntryNotFoundException()
        {

        }

        public DictionaryEntryNotFoundException(string msg)
            : base(msg)
        {

        }
    }
}

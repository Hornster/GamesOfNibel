using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Exceptions
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

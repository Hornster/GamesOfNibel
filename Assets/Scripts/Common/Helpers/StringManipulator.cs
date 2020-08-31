using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Helpers
{
    /// <summary>
    /// Contains methods capable of manipulating strings.
    /// </summary>
    public class StringManipulator
    {
        /// <summary>
        /// Removes all characters but digits, [A-Z], [a-z], '-' and regular space.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static string RemoveSpecialCharacters(string source)
        {
            var stringBuilder = new StringBuilder(source.Length);
            foreach (var c in source)
            {
                if (c <= 'z' && c >= 'a'
                    || c <= 'Z' && c >= 'A'
                    || c <= '9' && c >= '0'
                    || c == '-'
                    || c == ' ')
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
    }
}

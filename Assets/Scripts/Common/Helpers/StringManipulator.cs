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
        public static string RemoveSpecialCharacters(string source)
        {
            var stringBuilder = new StringBuilder(source.Length);
            foreach (var c in source)
            {
                if (c <= 'z' && c >= 'a'
                    || c <= 'Z' && c >= 'A'
                    || c <= '9' && c >= '0'
                    || c == '-'
                    || c == ' '
                    || c == '\'')
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
        /// <summary>
        /// Joins provided strings with provided dividing phrase.
        /// </summary>
        /// <param name="dividingPhrase">Will be put after every string in provided list except for the last one.</param>
        /// <param name="strings">What should be joined.</param>
        /// <returns></returns>
        public static string JoinStrings(string dividingPhrase, List<string> strings)
        {
            var stringBuilder = new StringBuilder();

            foreach (var part in strings)
            {
                stringBuilder.Append(part);
                stringBuilder.Append(dividingPhrase);
            }

            stringBuilder.Remove(stringBuilder.Length - dividingPhrase.Length, dividingPhrase.Length);

            return stringBuilder.ToString();
        }
    }
}

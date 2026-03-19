using System;
using System.Collections.Generic;

namespace Tanuki.Atlyss.FluffUtilities.Extensions;

public static class StringKeyedDictionary
{
    extension<T>(IReadOnlyDictionary<string, T> instance)
    {
        /// <summary>
        /// Retrieves all dictionary keys that contain the specified substring.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values stored on the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        /// The source dictionary containing string keys.
        /// </param>
        /// <param name="match">
        /// The substring to search for within each key.
        /// </param>
        /// <param name="hightlightFormat">
        /// A composite format string used to highlight the matched substring.
        /// </param>
        /// <param name="stringComparison">
        /// <see cref="StringComparison"/> method to use.
        /// </param>
        /// <returns>
        /// A list of keys where each occurrence of the matched substring is wrapped according to the specified <paramref name="hightlightFormat"/>.
        /// </returns>
        public List<string> GetHighlightedKeys(
            string match,
            string hightlightFormat = "*{0}*",
            StringComparison stringComparison = StringComparison.OrdinalIgnoreCase
        )
        {
            List<string> matches = [];

            foreach (string key in instance.Keys)
            {
                int position = key.IndexOf(match, stringComparison);

                if (position < 0)
                    continue;

                matches.Add(
                    string.Concat(
                        key[..position],
                        string.Format(hightlightFormat, key.Substring(position, match.Length)),
                        key[(position + match.Length)..]
                    )
                );
            }

            return matches;
        }
    }
}

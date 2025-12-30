using System;
using System.Collections.Generic;

namespace Tanuki.Atlyss.FluffUtilities.Helpers;

internal static partial class Commands
{
    public static List<string> FormatDictionaryKeyMatches<T>(IReadOnlyDictionary<string, T> Dictionary, string Search, string Format, StringComparison StringComparsion)
    {
        List<string> Matches = [];
        int Index;

        foreach (string Key in Dictionary.Keys)
        {
            Index = Key.IndexOf(Search, StringComparsion);

            if (Index < 0)
                continue;

            Matches.Add(
                string.Concat(
                    Key.Substring(0, Index),
                    string.Format(Format, Key.Substring(Index, Search.Length)),
                    Key.Substring(Index + Search.Length)
                )
            );
        }

        return Matches;
    }
}
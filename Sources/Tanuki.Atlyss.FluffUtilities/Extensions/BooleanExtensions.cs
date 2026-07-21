using System.Collections.Generic;

namespace Tanuki.Atlyss.FluffUtilities.Extensions;

public static class BooleanExtensions
{
    private static readonly HashSet<string>
        BOOLEAN_TRUE_VALUES = ["true", "1", "yes", "y"],
        BOOLEAN_FALSE_VALUES = ["false", "0", "no", "n"];

    extension(bool)
    {

        public static bool AdvancedTryParse(string value, out bool result)
        {
            result = false;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim().ToLowerInvariant();

            if (BOOLEAN_TRUE_VALUES.Contains(value))
                result = true;
            else if (BOOLEAN_FALSE_VALUES.Contains(value))
                result = false;
            else
                return false;

            return true;
        }
    }
}

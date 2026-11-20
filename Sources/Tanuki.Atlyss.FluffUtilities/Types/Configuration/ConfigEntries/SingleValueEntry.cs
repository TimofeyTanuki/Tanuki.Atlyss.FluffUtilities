using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.ConfigEntries;

public readonly struct SingleValueEntry<T>(ConfigDefinition configDefinition, T defaultValue, ConfigDescription? configDescription = null)
{
    public readonly ConfigDefinition ConfigDefinition = configDefinition;
    public readonly T DefaultValue = defaultValue;
    public readonly ConfigDescription? ConfigDescription = configDescription;
}

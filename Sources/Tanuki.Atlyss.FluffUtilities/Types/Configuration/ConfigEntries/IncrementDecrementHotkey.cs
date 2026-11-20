using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.ConfigEntries;

public sealed class IncrementDecrementHotkey(ConfigFile configFile, SingleValueEntry<KeyCode> decreaseHotkey, SingleValueEntry<KeyCode> increaseHotkey, SingleValueEntry<float> step)
{
    public readonly ConfigEntry<KeyCode> Decrease = configFile.Bind(decreaseHotkey.ConfigDefinition, decreaseHotkey.DefaultValue, decreaseHotkey.ConfigDescription), Increase = configFile.Bind(increaseHotkey.ConfigDefinition, increaseHotkey.DefaultValue, increaseHotkey.ConfigDescription);
    public readonly ConfigEntry<float> Step = configFile.Bind(step.ConfigDefinition, step.DefaultValue, step.ConfigDescription);
}

using BepInEx.Configuration;
using System;
using System.Numerics;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.ConfigEntries;

public sealed class FloatRange : IDisposable
{
    public readonly struct EntryConfiguration(ConfigDefinition configDefinition, float defaultValue, ConfigDescription? configDescription = null)
    {
        public readonly ConfigDefinition ConfigDefinition = configDefinition;
        public readonly float DefaultValue = defaultValue;
        public readonly ConfigDescription? ConfigDescription = configDescription;
    }

    public readonly ConfigEntry<float> Minimum, Maximum;

    public FloatRange(
        ConfigFile configFile,
        EntryConfiguration minimum,
        EntryConfiguration maximum
    )
    {
        Minimum = configFile.Bind(minimum.ConfigDefinition, minimum.DefaultValue, minimum.ConfigDescription);
        Maximum = configFile.Bind(maximum.ConfigDefinition, maximum.DefaultValue, maximum.ConfigDescription);

        Minimum.SettingChanged += ValidateValues;
        Maximum.SettingChanged += ValidateValues;
    }

    private void ValidateValues(object sender, EventArgs e)
    {
        float
            minimum = Minimum.Value,
            maximum = Maximum.Value;

        if (minimum <= maximum)
            return;

        if (sender == Minimum)
            Minimum.Value = maximum;
        else
            Maximum.Value = minimum;
    }

    public void Dispose()
    {
        Minimum.SettingChanged -= ValidateValues;
        Maximum.SettingChanged -= ValidateValues;
    }

    public Vector2 ValueAsVector2 => new(Minimum.Value, Maximum.Value);
}

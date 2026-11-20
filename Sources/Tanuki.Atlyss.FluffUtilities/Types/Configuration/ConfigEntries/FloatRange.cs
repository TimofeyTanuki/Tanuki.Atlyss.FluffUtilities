using BepInEx.Configuration;
using System;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.ConfigEntries;

public sealed class FloatRange : IDisposable
{
    public readonly ConfigEntry<float> Minimum, Maximum;

    public FloatRange(ConfigFile configFile, SingleValueEntry<float> minimum, SingleValueEntry<float> maximum)
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
}

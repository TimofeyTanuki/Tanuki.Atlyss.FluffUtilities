using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class GlobalRaceDisplayParameters(ref ConfigFile ConfigFile)
{
    private const string Section = "GlobalRaceDisplayParameters";
    public class RangeParameter(ref ConfigFile ConfigFile, string Name, float Minimum, float Maximum)
    {
        public ConfigEntry<float>
            Minimum = ConfigFile.Bind(Section, $"{Name}_Minimum", Minimum),
            Maximum = ConfigFile.Bind(Section, $"{Name}_Maximum", Maximum);

        public Vector2 AsVector2 => new(Minimum.Value, Maximum.Value);
    }

    public ConfigEntry<bool> Override = ConfigFile.Bind(Section, "Enabled", true);

    public RangeParameter
        HeadWidth = new(ref ConfigFile, "HeadWidth", 0, 4),
        MuzzleLength = new(ref ConfigFile, "MuzzleLength", -500f, 1000f),
        Height = new(ref ConfigFile, "Height", 0, 5),
        Width = new(ref ConfigFile, "Width", 0, 5),
        TorsoSize = new(ref ConfigFile, "TorsoSize", -100f, 1000),
        BreastSize = new(ref ConfigFile, "BreastSize", -100, 1000),
        ArmsSize = new(ref ConfigFile, "ArmsSize", -200, 1000),
        BellySize = new(ref ConfigFile, "BellySize", -300, 1000),
        BottomSize = new(ref ConfigFile, "BottomSize", -300, 1000),
        VoicePitch = new(ref ConfigFile, "VoicePitch", 0.05f, 3);
}
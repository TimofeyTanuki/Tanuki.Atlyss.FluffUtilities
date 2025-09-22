using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class NoClip(ref ConfigFile ConfigFile)
{
    private const string Section = "NoClip";
    public ConfigEntry<float> Speed = ConfigFile.Bind(Section, "Speed", 50f);
    public ConfigEntry<float> AlternativeSpeed = ConfigFile.Bind(Section, "AlternativeSpeed", 150f);
}
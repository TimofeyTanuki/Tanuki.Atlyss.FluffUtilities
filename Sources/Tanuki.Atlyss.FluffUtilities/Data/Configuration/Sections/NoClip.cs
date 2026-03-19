using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.Sections;

internal sealed class NoClip(ConfigFile ConfigFile)
{
    private const string SECTION_NAME = "NoClip";

    public ConfigEntry<float> Speed = ConfigFile.Bind(SECTION_NAME, "Speed", 50f);
    public ConfigEntry<float> AlternativeSpeed = ConfigFile.Bind(SECTION_NAME, "AlternativeSpeed", 150f);
}
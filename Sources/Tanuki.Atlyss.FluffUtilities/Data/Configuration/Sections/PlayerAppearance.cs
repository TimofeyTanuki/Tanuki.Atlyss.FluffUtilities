using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.Sections;

internal class PlayerAppearance(ConfigFile ConfigFile)
{
    private const string SECTION_NAME = "PlayerAppearance";

    public ConfigEntry<bool> AllowParametersBeyondLimits = ConfigFile.Bind(SECTION_NAME, "AllowParametersBeyondLimits", true);
    public ConfigEntry<bool> HotkeysUpdateCharacterSave = ConfigFile.Bind(SECTION_NAME, "HotkeysUpdateCharacterSave", true);
}
using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

internal sealed class PlayerAppearance(ConfigFile ConfigFile)
{
    private const string SECTION_NAME = "PlayerAppearance";

    public ConfigEntry<bool> AllowRaceParametersBeyondLimits = ConfigFile.Bind(SECTION_NAME, "AllowRaceParametersBeyondLimits", true);
    public ConfigEntry<bool> HotkeysUpdateCharacterSave = ConfigFile.Bind(SECTION_NAME, "HotkeysUpdateCharacterSave", true);
}
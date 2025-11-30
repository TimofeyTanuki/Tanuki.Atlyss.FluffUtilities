using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class PlayerAppearance(ConfigFile ConfigFile)
{
    private const string Section = "PlayerAppearance";
    public ConfigEntry<bool> AllowParametersBeyondLimits = ConfigFile.Bind(Section, "AllowParametersBeyondLimits", true);
    public ConfigEntry<bool> HotkeysUpdateCharacterSave = ConfigFile.Bind(Section, "HotkeysUpdateCharacterSave", true);
}
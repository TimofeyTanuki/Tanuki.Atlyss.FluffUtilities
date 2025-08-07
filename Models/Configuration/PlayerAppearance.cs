using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class PlayerAppearance(ref ConfigFile ConfigFile)
{
    private const string Section = "PlayerAppearance";
    public ConfigEntry<bool> DisableParametersCheck = ConfigFile.Bind(Section, "DisableParametersCheck", true);
}
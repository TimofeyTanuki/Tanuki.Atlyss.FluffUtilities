using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class Commands(ConfigFile ConfigFile)
{
    private const string Section = "Commands";

    public ConfigEntry<string> SteamProfile_LinkTemplate = ConfigFile.Bind(Section, "SteamProfile_LinkTemplate", "https://steamcommunity.com/profiles/{0}");
}
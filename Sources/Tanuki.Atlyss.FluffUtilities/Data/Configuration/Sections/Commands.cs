using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.Sections;

internal sealed class Commands(ConfigFile configFile)
{
    private const string SECTION_NAME = "Commands";

    public readonly ConfigEntry<string> SteamProfile_LinkTemplate = configFile.Bind(SECTION_NAME, "SteamProfile_LinkTemplate", "https://steamcommunity.com/profiles/{0}");
}
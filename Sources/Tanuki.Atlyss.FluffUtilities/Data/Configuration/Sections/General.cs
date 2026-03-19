using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.Sections;

internal sealed class General(ConfigFile configFile)
{
    private const string SECTION_NAME = "General";

    public readonly ConfigEntry<bool> PresenceEffectsOnJoin = configFile.Bind(SECTION_NAME, "PresenceEffectsOnJoin", true);
    public readonly ConfigEntry<bool> OtherPluginUserNotificationOnJoin = configFile.Bind(SECTION_NAME, "OtherPluginUserNotificationOnJoin", true);
    public readonly ConfigEntry<bool> HideUsagePresenceFromNonUserHosts = configFile.Bind(SECTION_NAME, "HideUsagePresenceFromNonUserHosts", false);
}
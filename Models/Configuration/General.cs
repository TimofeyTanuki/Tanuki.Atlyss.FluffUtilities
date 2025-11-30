using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class General(ConfigFile ConfigFile)
{
    private const string Section = "General";

    public ConfigEntry<bool> PresenceEffectsOnJoin = ConfigFile.Bind(Section, "PresenceEffectsOnJoin", true);
    public ConfigEntry<bool> OtherPluginUserNotificationOnJoin = ConfigFile.Bind(Section, "OtherPluginUserNotificationOnJoin", true);
    public ConfigEntry<bool> HideUsagePresenceFromNonUserHosts = ConfigFile.Bind(Section, "HideUsagePresenceFromNonUserHosts", false);
}
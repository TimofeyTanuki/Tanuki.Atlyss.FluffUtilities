using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class General(ref ConfigFile ConfigFile)
{
    private const string Section = "General";

    public ConfigEntry<bool> Plugin_ShowUsagePresenceOnJoin = ConfigFile.Bind(Section, "Plugin_ShowUsagePresenceOnJoin", true);
    public ConfigEntry<bool> Plugin_ShowOtherPluginUserMessageOnJoin = ConfigFile.Bind(Section, "Plugin_ShowOtherPluginUserMessageOnJoin", true);
}
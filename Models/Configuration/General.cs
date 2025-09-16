using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class General(ref ConfigFile ConfigFile)
{
    private const string Section = "General";

    /*
     * Anti-cheat result from the community
     * I will not compromise players, as unscrupulous hosts can ban them automatically without warning.
     */
    public ConfigEntry<bool> Plugin_PresenceMessageOnJoinLobby = ConfigFile.Bind(Section, "Plugin_PresenceMessageOnJoinLobby", false);
}
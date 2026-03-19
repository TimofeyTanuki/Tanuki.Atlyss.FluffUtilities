using BepInEx.Logging;
using Steamworks;
using System.Diagnostics.CodeAnalysis;

namespace Tanuki.Atlyss.FluffUtilities;

[BepInEx.BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
[BepInEx.BepInDependency(Core.PluginInfo.GUID, BepInEx.BepInDependency.DependencyFlags.HardDependency)]
public sealed class Main : Core.Bases.Plugin
{
    internal static Main Instance = null!;

    private ManualLogSource manualLogSource = null!;

    private bool reloaded = false;

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void Awake()
    {
        Instance = this;
        manualLogSource = Logger;

        Configuration.Initialize(Config);
    }

    protected override void Load()
    {
        manualLogSource.LogInfo("Tanuki.Atlyss.FluffUtilities by Timofey Tanuki / tanu.su");

        if (reloaded)
            Config.Reload();

        Game.Patches.Player.OnStartAuthority.OnPostfix += OnStartAuthority_OnPostfix;
    }

    private void OnStartAuthority_OnPostfix(Player player)
    {
        if (AtlyssNetworkManager._current._soloMode)
            return;

        // Temp solution
        // This will be replaced by sending a packet to clients, as the number of entries per user in the lobby is limited
        SteamMatchmaking.SetLobbyMemberData(Network.Tanuki.Instance.Providers.SteamLobby.SteamId, "cc8615a7-47a4-4321-be79-11e36887b64a.ver", $"{PluginInfo.VERSION}_DEVBUILD");
    }

    protected override void Unload()
    {
        reloaded = true;
        Game.Patches.Player.OnStartAuthority.OnPostfix -= OnStartAuthority_OnPostfix;
    }

}

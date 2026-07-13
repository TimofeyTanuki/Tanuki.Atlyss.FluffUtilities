using BepInEx.Logging;
using Steamworks;
using System.Diagnostics.CodeAnalysis;
using Tanuki.Atlyss.Core.Managers;

namespace Tanuki.Atlyss.FluffUtilities;

[BepInEx.BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
[BepInEx.BepInDependency(Core.PluginInfo.GUID, BepInEx.BepInDependency.DependencyFlags.HardDependency)]
public sealed class Main : Core.Bases.Plugin
{
    internal static Main instance = null!;
    internal Types.Managers managers = null!;

    private ManualLogSource manualLogSource = null!;

    private bool reloaded = false;

    public static Main Instance => instance;

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void Awake()
    {
        instance = this;
        manualLogSource = Logger;

        Configuration.Initialize(Config);

        managers = new()
        {
            noClip = new()
        };


    }

    protected override void Load()
    {
        manualLogSource.LogInfo("Tanuki.Atlyss.FluffUtilities by Timofey Tanuki / tanu.su");

        if (reloaded)
            Config.Reload();

        Configuration configuration = Configuration.Instance;

        managers.noClip.Reconfigure();

        Game.Patches.Player.OnStartAuthority.OnPostfix += OnStartAuthority_OnPostfix;

        RegisterHotkeys();
    }

    internal void DeregisterHotkeys()
    {
        Core.Tanuki.Instance.Managers.Hotkey.Deregister(managers.noClip.Toggle);
    }

    internal void RegisterHotkeys()
    {
        Types.Configuration.Sections.Hotkeys hotkeySection = Configuration.instance.Hotkeys;
        Core.Tanuki.Instance.Managers.Hotkey.Register([new(hotkeySection.NoClip_Toggle.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)], managers.noClip.Toggle);

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
        DeregisterHotkeys();

        reloaded = true;
        Game.Patches.Player.OnStartAuthority.OnPostfix -= OnStartAuthority_OnPostfix;
    }

}

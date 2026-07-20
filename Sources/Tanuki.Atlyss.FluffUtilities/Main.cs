using Steamworks;
using System.Diagnostics.CodeAnalysis;
using Tanuki.Atlyss.FluffUtilities.Managers;

namespace Tanuki.Atlyss.FluffUtilities;

[BepInEx.BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
[BepInEx.BepInDependency(Core.PluginInfo.GUID, BepInEx.BepInDependency.DependencyFlags.HardDependency)]
[BepInEx.BepInDependency(NessieEasySettings.GUID, BepInEx.BepInDependency.DependencyFlags.SoftDependency)]
public sealed class Main : Core.Bases.Plugin
{
    private static Main instance = null!;
    private Types.Main.Managers managers = null!;
    private bool reloaded = false;

    public static Main Instance => instance;
    public Types.Main.Managers Managers => managers;

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void Awake()
    {
        instance = this;

        Configuration.Initialize(Config);

        managers = new()
        {
            NoClip = new(),
            FreeCamera = new(),
            GameWorld = new(),
            NessieEasySettings = NessieEasySettings.GetInstance()
        };
    }

    protected override void Load()
    {
        Logger.LogInfo("Tanuki.Atlyss.FluffUtilities by Timofey Tanuki / tanu.su");

        if (reloaded)
            Config.Reload();

        Configuration configuration = Configuration.Instance;

        managers.NoClip.Reconfigure();
        managers.FreeCamera.Reconfigure();

        Game.Patches.Player.OnStartAuthority.OnPostfix += OnStartAuthority_OnPostfix;

        managers.FreeCamera.RegisterHotkeys();
        managers.NoClip.RegisterHotkeys();
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
        managers.FreeCamera.DeregisterHotkeys();
        managers.NoClip.DeregisterHotkeys();

        reloaded = true;
        Game.Patches.Player.OnStartAuthority.OnPostfix -= OnStartAuthority_OnPostfix;
    }
}

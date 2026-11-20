using System;
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
    private Types.Main.Services services = null!;
    private bool reloaded = false;

    public event Action? OnInitialized;

    public static Main Instance => instance;
    public Types.Main.Managers Managers => managers;
    public Types.Main.Services Services => services;

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
            Game = new(),
            NessieEasySettings = NessieEasySettings.GetInstance()
        };

        services = new()
        {
            ClientInfo = new()
        };

        OnInitialized?.Invoke();
    }

    protected override void Load()
    {
        Logger.LogInfo("Tanuki.Atlyss.FluffUtilities by Timofey Tanuki / tanu.su");

        if (reloaded)
            Config.Reload();
    }

    protected override void Unload()
    {
        reloaded = true;
    }
}

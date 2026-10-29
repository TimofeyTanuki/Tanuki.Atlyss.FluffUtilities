namespace Tanuki.Atlyss.FluffUtilities.Managers;

public sealed class NoClip
{
    private readonly Configuration pluginConfiguration;
    private readonly Main plugin;
    private Core.Managers.Hotkey HotkeyManager => Core.Tanuki.Instance.Managers.Hotkey;

    private Components.NoClip? NoClipComponent => Components.NoClip.GetOrCreate();
    public bool CurrentState => NoClipComponent?.enabled ?? false;

    internal NoClip()
    {
        pluginConfiguration = Configuration.Instance;

        plugin = Main.Instance;

        plugin.OnInitialized += OnPluginInitialized;
        plugin.OnLoaded += OnPluginLoaded;
        plugin.OnUnload += OnPluginUnload;
    }

    private void OnPluginInitialized() =>
        plugin.Managers.NessieEasySettings.OnModSettingsTabInitialized += OnModSettingTabInitialized;

    private void OnSettingsUpdated() => Reconfigure();

    private void OnHotkeysUpdated()
    {
        DeregisterHotkeys();
        RegisterHotkeys();
    }

    private void OnModSettingTabInitialized(Nessie.ATLYSS.EasySettings.SettingsTab settingsTab)
    {
        NessieEasySettings nessieEasySettings = plugin.Managers.NessieEasySettings;

        Types.Configuration.Sections.Hotkeys hotkeysSection = Configuration.Instance.Hotkeys;
        Types.Configuration.Sections.NoClip noClipSection = Configuration.Instance.NoClip;

        settingsTab.AddHeader("No clip");

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Toggle", hotkeysSection.NoClip_Toggle),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Speed", noClipSection.Speed, true),
            OnSettingsUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Alternative speed", noClipSection.AlternativeSpeed, true),
            OnSettingsUpdated
        );
    }

    private void OnPluginUnload() => DeregisterHotkeys();

    private void OnPluginLoaded()
    {
        Reconfigure();
        RegisterHotkeys();
    }

    internal void Reconfigure() => NoClipComponent?.Reconfigure();

    internal void RegisterHotkeys() =>
        HotkeyManager.Register(
            [
                new(pluginConfiguration.Hotkeys.NoClip_Toggle.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            ToggleState
        );

    internal void DeregisterHotkeys() => HotkeyManager.Deregister(ToggleState);

    public void SetState(bool state)
    {
        if (state)
            Main.Instance.Managers.FreeCamera.SetState(false);

        NoClipComponent?.enabled = state;
    }

    public void ToggleState() => SetState(!CurrentState);
}

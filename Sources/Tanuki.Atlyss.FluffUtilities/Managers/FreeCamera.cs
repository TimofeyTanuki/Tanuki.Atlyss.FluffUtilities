using Nessie.ATLYSS.EasySettings;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

public sealed class FreeCamera
{
    private readonly Configuration pluginConfiguration;
    private readonly Main plugin;
    private Core.Managers.Hotkey HotkeyManager => Core.Tanuki.Instance.Managers.Hotkey;
    private Components.FreeCamera? FreeCameraComponent => Components.FreeCamera.GetOrCreate();
    public bool CurrentState => FreeCameraComponent?.enabled ?? false;

    internal FreeCamera()
    {
        pluginConfiguration = Configuration.Instance;

        plugin = Main.Instance;

        plugin.OnInitialized += OnPluginInitialized;
        plugin.OnLoaded += OnPluginLoaded;
        plugin.OnUnload += OnPluginUnload;
    }

    private void OnPluginInitialized() =>
        plugin.Managers.NessieEasySettings.OnModSettingsTabInitialized += OnModSettingsTabInitialized;

    private void OnSettingsUpdated() => Reconfigure();

    private void OnHotkeysUpdated()
    {
        DeregisterHotkeys();
        RegisterHotkeys();
    }

    private void OnModSettingsTabInitialized(SettingsTab settingsTab)
    {
        NessieEasySettings nessieEasySettings = plugin.Managers.NessieEasySettings;

        Types.Configuration.Sections.Hotkeys hotkeysSection = Configuration.Instance.Hotkeys;
        Types.Configuration.Sections.FreeCamera freeCameraSection = Configuration.Instance.FreeCamera;

        settingsTab.AddHeader("Free camera");

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Toggle", hotkeysSection.FreeCamera_Toggle),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Base speed", freeCameraSection.BaseSpeed, true),
            OnSettingsUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Speed adjustment scroll multiplier", freeCameraSection.SpeedAdjustmentScrollMultiplier, true),
            OnSettingsUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddToggle("Lock character controls", freeCameraSection.LockCharacterControls),
            OnSettingsUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddToggle("Smooth look", freeCameraSection.SmoothLook),
            OnSettingsUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Smooth look • Interpolation", freeCameraSection.SmoothLook_Interpolation, true),
            OnSettingsUpdated
        );
    }

    private void OnPluginUnload() => DeregisterHotkeys();

    private void OnPluginLoaded()
    {
        Reconfigure();
        RegisterHotkeys();
    }

    internal void Reconfigure() => FreeCameraComponent?.Reconfigure();

    internal void RegisterHotkeys() =>
        HotkeyManager.Register(
            [
                new(pluginConfiguration.Hotkeys.FreeCamera_Toggle.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            ToggleState
        );

    internal void DeregisterHotkeys() => HotkeyManager.Deregister(ToggleState);

    public void SetState(bool state)
    {
        if (state)
            Main.Instance.Managers.NoClip.SetState(false);

        FreeCameraComponent?.enabled = state;
    }

    public void ToggleState() => SetState(!CurrentState);
}

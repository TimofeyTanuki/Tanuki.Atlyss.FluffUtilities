using Nessie.ATLYSS.EasySettings;
using Nessie.ATLYSS.EasySettings.UIElements;
using System;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

/// <summary>
/// Provides access to the Nessie Easy Settings integration (<see cref="BepInEx.BepInDependency.DependencyFlags.SoftDependency"/>).
/// </summary>
/// <remarks>
/// Must be initialized by calling <see cref="GetInstance"/> from <see cref="Main.Awake"/>.
/// </remarks>
internal sealed class NessieEasySettings
{
    [Flags]
    private enum ESettingsUpdate
    {
        None = 0,
        FreeCamera = 1 << 0,
        FreeCameraHotkeys = 1 << 1,
        NoClip = 1 << 2,
        NoClipHotkeys = 1 << 3
    }

    public const string GUID = "EasySettings";
    private static NessieEasySettings? instance;

    public readonly bool IsAvailable = false;

    private ESettingsUpdate pendingUpdates = ESettingsUpdate.None;

    private NessieEasySettings()
    {
        if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID))
            return;

        IsAvailable = true;
        Settings.OnInitialized.AddListener(OnInitialized);
        Settings.OnApplySettings.AddListener(OnApplySettings);
    }

    public static NessieEasySettings GetInstance()
    {
        instance ??= new();
        return instance;
    }

    private void AddPendingUpdate(ESettingsUpdate settingsUpdate) => pendingUpdates |= settingsUpdate;
    private bool HasPendingUpdate(ESettingsUpdate settingsUpdate) => (pendingUpdates & settingsUpdate) != 0;

    /// <summary>
    /// Implements <see cref="AddPendingUpdate(ESettingsUpdate)"/> for classes:
    /// <list type="bullet">
    /// <item><see cref="AtlyssAdvancedSlider"/></item>
    /// <item><see cref="AtlyssToggle"/></item>
    /// <item><see cref="AtlyssKeyButton"/></item>
    /// </list>
    /// </summary>
    private void AddPendingUpdateListener(BaseAtlyssElement element, ESettingsUpdate settingUpdate)
    {
        switch (element)
        {
            case AtlyssAdvancedSlider advancedSlider:
                advancedSlider.OnValueChanged.AddListener(_ => AddPendingUpdate(settingUpdate));
                break;
            case AtlyssToggle toggle:
                toggle.OnValueChanged.AddListener(_ => AddPendingUpdate(settingUpdate));
                break;
            case AtlyssKeyButton keyButton:
                keyButton.OnValueChanged.AddListener(_ => AddPendingUpdate(settingUpdate));
                break;
        }
    }

    private void OnInitialized()
    {
        SettingsTab settingsTab = Settings.GetOrAddCustomTab(PluginInfo.NAME);

        Types.Configuration.Sections.Hotkeys hotkeysSection = Configuration.Instance.Hotkeys;
        Types.Configuration.Sections.FreeCamera freeCameraSection = Configuration.Instance.FreeCamera;
        Types.Configuration.Sections.NoClip noClipSection = Configuration.Instance.NoClip;

        settingsTab.AddHeader("Free camera");

        AddPendingUpdateListener(
            settingsTab.AddKeyButton("Toggle", hotkeysSection.FreeCamera_Toggle),
            ESettingsUpdate.FreeCameraHotkeys
        );

        AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Base speed", freeCameraSection.BaseSpeed, true),
            ESettingsUpdate.FreeCamera
        );

        AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Speed adjustment scroll multiplier", freeCameraSection.SpeedAdjustmentScrollMultiplier, true),
            ESettingsUpdate.FreeCamera
        );

        AddPendingUpdateListener(
            settingsTab.AddToggle("Lock character controls", freeCameraSection.LockCharacterControls),
            ESettingsUpdate.FreeCamera
        );

        AddPendingUpdateListener(
            settingsTab.AddToggle("Smooth look", freeCameraSection.SmoothLook),
            ESettingsUpdate.FreeCamera
        );

        AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Smooth look • Interpolation", freeCameraSection.SmoothLook_Interpolation, true),
            ESettingsUpdate.FreeCamera
        );

        settingsTab.AddHeader("No clip");

        AddPendingUpdateListener(
            settingsTab.AddKeyButton("Toggle", hotkeysSection.NoClip_Toggle),
            ESettingsUpdate.NoClipHotkeys
        );

        AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Speed", noClipSection.Speed, true),
            ESettingsUpdate.NoClip
        );

        AddPendingUpdateListener(
            settingsTab.AddAdvancedSlider("Alternative speed", noClipSection.AlternativeSpeed, true),
            ESettingsUpdate.NoClip
        );
    }

    private void OnApplySettings()
    {
        Types.Managers managers = Main.Instance.Managers;

        if (HasPendingUpdate(ESettingsUpdate.FreeCamera))
            managers.FreeCamera.Reconfigure();

        if (HasPendingUpdate(ESettingsUpdate.NoClip))
            managers.NoClip.Reconfigure();

        if (HasPendingUpdate(ESettingsUpdate.FreeCameraHotkeys))
        {
            managers.FreeCamera.DeregisterHotkeys();
            managers.FreeCamera.RegisterHotkeys();
        }

        if (HasPendingUpdate(ESettingsUpdate.NoClipHotkeys))
        {
            managers.NoClip.DeregisterHotkeys();
            managers.NoClip.RegisterHotkeys();
        }

        pendingUpdates = ESettingsUpdate.None;
    }
}

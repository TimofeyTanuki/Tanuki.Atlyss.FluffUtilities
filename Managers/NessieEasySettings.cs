using Nessie.ATLYSS.EasySettings;
using System;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class NessieEasySettings
{
    public const string GUID = "EasySettings";

    internal static NessieEasySettings Instance;

    private NessieEasySettings()
    {
        Settings.OnInitialized.AddListener(NessieEasySettings_OnInitialize);
        Settings.OnApplySettings.AddListener(NessieEasySettings_OnApplySettings);
    }

    public static void Initialize() => Instance ??= new();
    private void NessieEasySettings_OnInitialize()
    {
        SettingsTab SettingsTab = Settings.ModTab;

        SettingsTab.AddHeader("- Tanuki Atlyss FluffUtilities -");

        SettingsTab.AddHeader("General");

        SettingsTab.AddToggle("Presence effects on join", Configuration.Instance.General.PresenceEffectsOnJoin);
        SettingsTab.AddToggle("Hide usage presence from non user hosts", Configuration.Instance.General.HideUsagePresenceFromNonUserHosts);
        SettingsTab.AddToggle("Show other plugin user notification on join", Configuration.Instance.General.OtherPluginUserNotificationOnJoin);

        SettingsTab.AddHeader("Free camera");

        SettingsTab.AddKeyButton("Toggle", Configuration.Instance.Hotkeys.FreeCamera);
        SettingsTab.AddKeyButton("Movement • Forward", Configuration.Instance.Hotkeys.FreeCamera_Forward);
        SettingsTab.AddKeyButton("Movement • Right", Configuration.Instance.Hotkeys.FreeCamera_Right);
        SettingsTab.AddKeyButton("Movement • Backward", Configuration.Instance.Hotkeys.FreeCamera_Backward);
        SettingsTab.AddKeyButton("Movement • Left", Configuration.Instance.Hotkeys.FreeCamera_Left);
        SettingsTab.AddKeyButton("Movement • Up", Configuration.Instance.Hotkeys.FreeCamera_Up);
        SettingsTab.AddKeyButton("Movement • Down", Configuration.Instance.Hotkeys.FreeCamera_Down);
        SettingsTab.AddToggle(
            "Lock character controls",
            Configuration.Instance.FreeCamera.LockCharacterControls
        );
        SettingsTab.AddAdvancedSlider(
            "Speed",
            Configuration.Instance.FreeCamera.Speed.Value,
            1f,
            100,
            true
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.FreeCamera.Speed.Value = (float)Math.Round(Value, 2)
        );
        SettingsTab.AddAdvancedSlider(
            "Speed • Scroll adjustment step",
            Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value,
            1f,
            25,
            false
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value = (float)Math.Round(Value, 2)
        );
        SettingsTab.AddToggle(
            "Smooth mode",
            Configuration.Instance.FreeCamera.SmoothLookMode
        );
        SettingsTab.AddAdvancedSlider(
            "Smooth mode • Interpolation",
            Configuration.Instance.FreeCamera.SmoothLookModeInterpolation.Value,
            1f,
            10,
            false
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.FreeCamera.SmoothLookModeInterpolation.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddHeader("No clip");

        SettingsTab.AddKeyButton("Toggle", Configuration.Instance.Hotkeys.NoClip_Toggle);
        SettingsTab.AddKeyButton("Movement • Forward", Configuration.Instance.Hotkeys.NoClip_Forward);
        SettingsTab.AddKeyButton("Movement • Right", Configuration.Instance.Hotkeys.NoClip_Right);
        SettingsTab.AddKeyButton("Movement • Backward", Configuration.Instance.Hotkeys.NoClip_Backward);
        SettingsTab.AddKeyButton("Movement • Left", Configuration.Instance.Hotkeys.NoClip_Left);
        SettingsTab.AddKeyButton("Movement • Up", Configuration.Instance.Hotkeys.NoClip_Up);
        SettingsTab.AddKeyButton("Movement • Down", Configuration.Instance.Hotkeys.NoClip_Down);
        SettingsTab.AddKeyButton("Movement • Alternative Speed", Configuration.Instance.Hotkeys.NoClip_AlternativeSpeedKey);
        SettingsTab.AddAdvancedSlider(
            "Speed",
            Configuration.Instance.NoClip.Speed.Value,
            0.01f,
            250,
            false
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.NoClip.Speed.Value = (float)Math.Round(Value, 2)
        );
        SettingsTab.AddAdvancedSlider(
            "Alternative speed",
            Configuration.Instance.NoClip.AlternativeSpeed.Value,
            0.01f,
            1000,
            false
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.NoClip.AlternativeSpeed.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddHeader("Player Appearance");

        SettingsTab.AddToggle("Allow parameters beyond limits", Configuration.Instance.PlayerAppearance.AllowParametersBeyondLimits);
        SettingsTab.AddToggle("Hotkeys update character save", Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave);

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Head width • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Decrease);
        SettingsTab.AddKeyButton("Head width • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Increase);
        SettingsTab.AddAdvancedSlider(
            "Head width • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.HeadWidth.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Muzzle length • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Decrease);
        SettingsTab.AddKeyButton("Muzzle length • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Increase);
        SettingsTab.AddAdvancedSlider(
            "Muzzle length • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.MuzzleLength.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Step.Value = (float)Math.Round(Value, 2);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Height • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Decrease);
        SettingsTab.AddKeyButton("Height • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Increase);
        SettingsTab.AddAdvancedSlider(
            "Height • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.Height.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Step.Value = (float)Math.Round(Value, 2);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Width • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Decrease);
        SettingsTab.AddKeyButton("Width • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Increase);
        SettingsTab.AddAdvancedSlider(
            "Width • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.Width.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Torso size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Decrease);
        SettingsTab.AddKeyButton("Torso size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Increase);
        SettingsTab.AddAdvancedSlider(
            "Torso size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.TorsoSize.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Breast size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Decrease);
        SettingsTab.AddKeyButton("Breast size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Increase);
        SettingsTab.AddAdvancedSlider(
            "Breast size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.BreastSize.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Arms size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Decrease);
        SettingsTab.AddKeyButton("Arms size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Increase);
        SettingsTab.AddAdvancedSlider(
            "Arms size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.ArmsSize.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Belly size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Decrease);
        SettingsTab.AddKeyButton("Belly size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Increase);
        SettingsTab.AddAdvancedSlider(
            "Belly size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.BellySize.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Bottom size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Decrease);
        SettingsTab.AddKeyButton("Bottom size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Increase);
        SettingsTab.AddAdvancedSlider(
            "Bottom size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.BottomSize.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Voice pitch • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Decrease);
        SettingsTab.AddKeyButton("Voice pitch • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Increase);
        SettingsTab.AddAdvancedSlider(
            "Voice pitch • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.VoicePitch.Maximum.Value
        ).OnValueChanged.AddListener(
            Value => Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Step.Value = (float)Math.Round(Value, 2)
        );

        SettingsTab.AddSpace();
    }
    private void NessieEasySettings_OnApplySettings()
    {
        Configuration.Instance.Load(Main.Instance.Config);

        PlayerAppearance.Instance.Reload();
        FreeCamera.Instance.Reload();
        NoClip.Instance.Reload();
        Main.Instance.ReloadHotkeys();
        Lobby.Instance.Reload();
    }
}
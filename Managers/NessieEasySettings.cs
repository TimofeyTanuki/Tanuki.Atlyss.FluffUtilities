using Nessie.ATLYSS.EasySettings;
using System;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class NessieEasySettings
{
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

        SettingsTab.AddHeader("Tanuki.Atlyss.FluffUtilities");

        SettingsTab.AddToggle("Disable player appearance parameters check", Configuration.Instance.PlayerAppearance.DisableParametersCheck);

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Free camera", Configuration.Instance.Hotkeys.FreeCamera_Toggle_Default);
        SettingsTab.AddKeyButton("Free camera (with controls)", Configuration.Instance.Hotkeys.FreeCamera_Toggle_WithControls);
        SettingsTab.AddKeyButton("Free camera • Forward", Configuration.Instance.Hotkeys.FreeCamera_Forward);
        SettingsTab.AddKeyButton("Free camera • Right", Configuration.Instance.Hotkeys.FreeCamera_Right);
        SettingsTab.AddKeyButton("Free camera • Backward", Configuration.Instance.Hotkeys.FreeCamera_Backward);
        SettingsTab.AddKeyButton("Free camera • Left", Configuration.Instance.Hotkeys.FreeCamera_Left);
        SettingsTab.AddKeyButton("Free camera • Up", Configuration.Instance.Hotkeys.FreeCamera_Up);
        SettingsTab.AddKeyButton("Free camera • Down", Configuration.Instance.Hotkeys.FreeCamera_Down);
        SettingsTab.AddAdvancedSlider("Free camera • Base Speed", Configuration.Instance.FreeCamera.Speed.Value, 0.01f, 100, true).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.FreeCamera.Speed.Value >= Value)
                    return;

                Configuration.Instance.FreeCamera.Speed.Value = Value;
            }
        );
        SettingsTab.AddAdvancedSlider("Free camera • Scroll Speed Step", Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value, 0.01f, 25, true).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value >= Value)
                    return;

                Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value = Value;
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("NoClip", Configuration.Instance.Hotkeys.NoClip_Toggle);
        SettingsTab.AddKeyButton("NoClip • Alternative Speed Key", Configuration.Instance.Hotkeys.NoClip_AlternativeSpeedKey);
        SettingsTab.AddKeyButton("NoClip • Forward", Configuration.Instance.Hotkeys.NoClip_Forward);
        SettingsTab.AddKeyButton("NoClip • Right", Configuration.Instance.Hotkeys.NoClip_Right);
        SettingsTab.AddKeyButton("NoClip • Backward", Configuration.Instance.Hotkeys.NoClip_Backward);
        SettingsTab.AddKeyButton("NoClip • Left", Configuration.Instance.Hotkeys.NoClip_Left);
        SettingsTab.AddKeyButton("NoClip • Up", Configuration.Instance.Hotkeys.NoClip_Up);
        SettingsTab.AddKeyButton("NoClip • Down", Configuration.Instance.Hotkeys.NoClip_Down);
        SettingsTab.AddAdvancedSlider("NoClip • Base Speed", Configuration.Instance.NoClip.Speed.Value, 0.01f, 250, true).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.NoClip.Speed.Value >= Value)
                    return;

                Configuration.Instance.NoClip.Speed.Value = Value;
            }
        );
        SettingsTab.AddAdvancedSlider("NoClip • Alternative Speed", Configuration.Instance.NoClip.AlternativeSpeed.Value, 0.01f, 1000, true).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.NoClip.AlternativeSpeed.Value >= Value)
                    return;

                Configuration.Instance.NoClip.AlternativeSpeed.Value = Value;
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Head Width • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Decrease);
        SettingsTab.AddKeyButton("Head Width • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Increase);
        SettingsTab.AddToggle("Head Width • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Head Width • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.HeadWidth.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Muzzle Length • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Decrease);
        SettingsTab.AddKeyButton("Muzzle Length • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Increase);
        SettingsTab.AddToggle("Muzzle Length • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Muzzle Length • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.MuzzleLength.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Height • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_Height.Decrease);
        SettingsTab.AddKeyButton("Height • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_Height.Increase);
        SettingsTab.AddToggle("Height • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_Height.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Height • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_Height.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.Height.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_Height.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_Height.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Width • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_Width.Decrease);
        SettingsTab.AddKeyButton("Width • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_Width.Increase);
        SettingsTab.AddToggle("Width • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_Width.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Width • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_Width.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.Width.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_Width.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_Width.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Torso Size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Decrease);
        SettingsTab.AddKeyButton("Torso Size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Increase);
        SettingsTab.AddToggle("Torso Size • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Torso Size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.TorsoSize.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Breast Size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Decrease);
        SettingsTab.AddKeyButton("Breast Size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Increase);
        SettingsTab.AddToggle("Breast Size • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Breast Size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.BreastSize.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Arms Size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Decrease);
        SettingsTab.AddKeyButton("Arms Size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Increase);
        SettingsTab.AddToggle("Arms Size • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Arms Size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.ArmsSize.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Belly Size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Decrease);
        SettingsTab.AddKeyButton("Belly Size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Increase);
        SettingsTab.AddToggle("Belly Size • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Belly Size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.BellySize.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Bottom Size • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Decrease);
        SettingsTab.AddKeyButton("Bottom Size • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Increase);
        SettingsTab.AddToggle("Bottom Size • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Bottom Size • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.BottomSize.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Step.Value = (float)Math.Round(Value);
            }
        );

        SettingsTab.AddSpace();

        SettingsTab.AddKeyButton("Voice Pitch • Decrease", Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Decrease);
        SettingsTab.AddKeyButton("Voice Pitch • Increase", Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Increase);
        SettingsTab.AddToggle("Voice Pitch • Update Character File", Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.UpdateCharacterFile);
        SettingsTab.AddAdvancedSlider(
            "Voice Pitch • Step",
            Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Step.Value,
            0,
            Configuration.Instance.GlobalRaceDisplayParameters.VoicePitch.Maximum.Value
        ).OnValueChanged.AddListener(
            delegate (float Value)
            {
                if (Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Step.Value >= Value)
                    return;

                Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Step.Value = (float)Math.Round(Value);
            }
        );
    }
    private void NessieEasySettings_OnApplySettings()
    {
        Configuration.Instance.Load(Main.Instance.Config);

        PlayerAppearance.Instance.Reload();
        FreeCamera.Instance.Reload();
        NoClip.Instance.Reload();
        Main.Instance.ReloadHotkeys();
    }
}
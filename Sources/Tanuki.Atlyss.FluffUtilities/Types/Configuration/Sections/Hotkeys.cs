using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

internal sealed class Hotkeys(ConfigFile configFile)
{
    private const string SECTION_NAME = "Hotkeys";

    public readonly ConfigEntry<KeyCode>
        NoClip_Toggle = configFile.Bind(SECTION_NAME, "NoClip_Toggle", KeyCode.Delete);

    public readonly ConfigEntry<KeyCode>
        FreeCamera_Toggle = configFile.Bind(SECTION_NAME, "FreeCamera_Toggle", KeyCode.End);

    public readonly ConfigEntries.IncrementDecrementHotkey
        ModifyHeadWidth =
            new(
                configFile,
                new(new(SECTION_NAME, "HeadWidth_Decrease"), KeyCode.None),
                new(new(SECTION_NAME, "HeadWidth_Increase"), KeyCode.None),
                new(new(SECTION_NAME, "HeadWidth_Step"), 0.25f)
            ),
        ModifyMuzzleLength =
            new(
                configFile,
                new(new(SECTION_NAME, "MuzzleLength_Decrease"), KeyCode.None),
                new(new(SECTION_NAME, "MuzzleLength_Increase"), KeyCode.None),
                new(new(SECTION_NAME, "MuzzleLength_Step"), 0.1f)
            ),
        ModifyHeight =
            new(
                configFile,
                new(new(SECTION_NAME, "Height_Decrease"), KeyCode.Keypad2),
                new(new(SECTION_NAME, "Height_Increase"), KeyCode.Keypad8),
                new(new(SECTION_NAME, "Height_Step"), 0.1f)
            ),
        ModifyWidth =
            new(
                configFile,
                new(new(SECTION_NAME, "Width_Decrease"), KeyCode.Keypad4),
                new(new(SECTION_NAME, "Width_Increase"), KeyCode.Keypad6),
                new(new(SECTION_NAME, "Width_Step"), 5f)
            ),
        ModifyTorsoSize =
            new(
                configFile,
                new(new(SECTION_NAME, "TorsoSize_Decrease"), KeyCode.None),
                new(new(SECTION_NAME, "TorsoSize_Increase"), KeyCode.None),
                new(new(SECTION_NAME, "TorsoSize_Step"), 25f)
            ),
        ModifyBreastSize =
            new(
                configFile,
                new(new(SECTION_NAME, "BreastSize_Decrease"), KeyCode.Keypad7),
                new(new(SECTION_NAME, "BreastSize_Increase"), KeyCode.Keypad9),
                new(new(SECTION_NAME, "BreastSize_Step"), 25f)
            ),
        ModifyArmSize =
            new(
                configFile,
                new(new(SECTION_NAME, "ArmSize_Decrease"), KeyCode.None),
                new(new(SECTION_NAME, "ArmSize_Increase"), KeyCode.None),
                new(new(SECTION_NAME, "ArmSize_Step"), 25f)
            ),
        ModifyBellySize =
            new(
                configFile,
                new(new(SECTION_NAME, "BellySize_Decrease"), KeyCode.KeypadMinus),
                new(new(SECTION_NAME, "BellySize_Increase"), KeyCode.KeypadPlus),
                new(new(SECTION_NAME, "BellySize_Step"), 25f)
            ),
        ModifyBottomSize =
            new(
                configFile,
                new(new(SECTION_NAME, "BottomSize_Decrease"), KeyCode.Keypad1),
                new(new(SECTION_NAME, "BottomSize_Increase"), KeyCode.Keypad3),
                new(new(SECTION_NAME, "BottomSize_Step"), 25f)
            ),
        ModifyVoicePitch =
            new(
                configFile,
                new(new(SECTION_NAME, "VoicePitch_Increase"), KeyCode.None),
                new(new(SECTION_NAME, "VoicePitch_Decrease"), KeyCode.None),
                new(new(SECTION_NAME, "VoicePitch_Step"), 0.01f)
            );
}

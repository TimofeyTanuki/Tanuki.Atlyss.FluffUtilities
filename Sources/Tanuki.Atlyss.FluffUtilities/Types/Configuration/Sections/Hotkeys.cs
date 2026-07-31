using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

internal sealed class Hotkeys(ConfigFile ConfigFile)
{
    private const string SECTION_NAME = "Hotkeys";

    public readonly ConfigEntry<KeyCode>
        NoClip_Toggle = ConfigFile.Bind(SECTION_NAME, "NoClip_Toggle", KeyCode.Delete);

    public readonly ConfigEntry<KeyCode>
        FreeCamera_Toggle = ConfigFile.Bind(SECTION_NAME, "FreeCamera_Toggle", KeyCode.End);
}
/*
internal sealed class Hotkeys(ConfigFile ConfigFile)
{
    private const string SECTION_NAME = "Hotkeys";

    public class PlayerAppearanceHotkey(ref ConfigFile ConfigFile, string Name, float Step, KeyCode Decrease, KeyCode Increase)
    {
        public ConfigEntry<float> Step = ConfigFile.Bind(SECTION_NAME, $"{Name}_Step", Step);
        public ConfigEntry<KeyCode>
            Increase = ConfigFile.Bind(SECTION_NAME, $"{Name}_Increase", Increase),
            Decrease = ConfigFile.Bind(SECTION_NAME, $"{Name}_Decrease", Decrease);
    }

    public readonly PlayerAppearanceHotkey
        PlayerAppearance_ModifyHeadWidth = new(ref ConfigFile, "HeadWidth", 0.25f, KeyCode.Keypad7, KeyCode.Keypad9),
        PlayerAppearance_ModifyMuzzleLength = new(ref ConfigFile, "MuzzleLength", 25, KeyCode.None, KeyCode.None),
        PlayerAppearance_ModifyHeight = new(ref ConfigFile, "Height", 0.1f, KeyCode.None, KeyCode.None),
        PlayerAppearance_ModifyWidth = new(ref ConfigFile, "Width", 0.1f, KeyCode.None, KeyCode.None),
        PlayerAppearance_ModifyTorsoSize = new(ref ConfigFile, "TorsoSize", 25, KeyCode.None, KeyCode.None),
        PlayerAppearance_ModifyBreastSize = new(ref ConfigFile, "BreastSize", 25, KeyCode.Keypad4, KeyCode.Keypad6),
        PlayerAppearance_ModifyArmsSize = new(ref ConfigFile, "ArmsSize", 25, KeyCode.None, KeyCode.None),
        PlayerAppearance_ModifyBellySize = new(ref ConfigFile, "BellySize", 25, KeyCode.Keypad2, KeyCode.Keypad5),
        PlayerAppearance_ModifyBottomSize = new(ref ConfigFile, "BottomSize", 25, KeyCode.Keypad1, KeyCode.Keypad3),
        PlayerAppearance_ModifyVoicePitch = new(ref ConfigFile, "VoicePitch", 0.01f, KeyCode.None, KeyCode.None);
*/

using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class Hotkeys(ref ConfigFile ConfigFile)
{
    private const string Section = "Hotkeys";

    public class PlayerAppearanceHotkey(ref ConfigFile ConfigFile, string Name, float Step, bool UpdateCharacterFile, KeyCode Decrease, KeyCode Increase)
    {
        public ConfigEntry<float> Step = ConfigFile.Bind(Section, $"{Name}_Step", Step);
        public ConfigEntry<bool> UpdateCharacterFile = ConfigFile.Bind(Section, $"{Name}_UpdateCharacterFile", UpdateCharacterFile);
        public ConfigEntry<KeyCode>
            Increase = ConfigFile.Bind(Section, $"{Name}_Increase", Increase),
            Decrease = ConfigFile.Bind(Section, $"{Name}_Decrease", Decrease);
    }

    public PlayerAppearanceHotkey
        PlayerAppearance_HeadWidth = new(ref ConfigFile, "HeadWidth", 0.25f, true, KeyCode.Keypad7, KeyCode.Keypad9),
        PlayerAppearance_MuzzleLength = new(ref ConfigFile, "MuzzleLength", 25, true, KeyCode.None, KeyCode.None),
        PlayerAppearance_Height = new(ref ConfigFile, "Height", 0.1f, true, KeyCode.None, KeyCode.None),
        PlayerAppearance_Width = new(ref ConfigFile, "Width", 0.1f, true, KeyCode.None, KeyCode.None),
        PlayerAppearance_TorsoSize = new(ref ConfigFile, "TorsoSize", 25, true, KeyCode.None, KeyCode.None),
        PlayerAppearance_BreastSize = new(ref ConfigFile, "BreastSize", 25, true, KeyCode.Keypad4, KeyCode.Keypad6),
        PlayerAppearance_ArmsSize = new(ref ConfigFile, "ArmsSize", 25, true, KeyCode.None, KeyCode.None),
        PlayerAppearance_BellySize = new(ref ConfigFile, "BellySize", 25, true, KeyCode.Keypad2, KeyCode.Keypad5),
        PlayerAppearance_BottomSize = new(ref ConfigFile, "BottomSize", 25, true, KeyCode.Keypad1, KeyCode.Keypad3),
        PlayerAppearance_VoicePitch = new(ref ConfigFile, "VoicePitch", 0.01f, true, KeyCode.None, KeyCode.None);

    public ConfigEntry<KeyCode>
        FreeCamera_Toggle_Default = ConfigFile.Bind(Section, "FreeCamera_Toggle_Default", KeyCode.End),
        FreeCamera_Toggle_WithControls = ConfigFile.Bind(Section, "FreeCamera_Toggle_WithControls", KeyCode.None),
        FreeCamera_Forward = ConfigFile.Bind(Section, "FreeCamera_Forward", KeyCode.W),
        FreeCamera_Right = ConfigFile.Bind(Section, "FreeCamera_Right", KeyCode.D),
        FreeCamera_Backward = ConfigFile.Bind(Section, "FreeCamera_Backward", KeyCode.S),
        FreeCamera_Left = ConfigFile.Bind(Section, "FreeCamera_Left", KeyCode.A),
        FreeCamera_Up = ConfigFile.Bind(Section, "FreeCamera_Up", KeyCode.Space),
        FreeCamera_Down = ConfigFile.Bind(Section, "FreeCamera_Down", KeyCode.LeftShift);

    public ConfigEntry<KeyCode>
        NoClip_Toggle = ConfigFile.Bind(Section, "NoClip_Toggle", KeyCode.Delete),
        NoClip_AlternativeSpeedKey = ConfigFile.Bind(Section, "NoClip_AlternativeSpeedKey", KeyCode.LeftShift),
        NoClip_Forward = ConfigFile.Bind(Section, "NoClip_Forward", KeyCode.W),
        NoClip_Right = ConfigFile.Bind(Section, "NoClip_Right", KeyCode.D),
        NoClip_Backward = ConfigFile.Bind(Section, "NoClip_Backward", KeyCode.S),
        NoClip_Left = ConfigFile.Bind(Section, "NoClip_Left", KeyCode.A),
        NoClip_Up = ConfigFile.Bind(Section, "NoClip_Up", KeyCode.Space),
        NoClip_Down = ConfigFile.Bind(Section, "NoClip_Down", KeyCode.LeftControl);
}
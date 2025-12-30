using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class Hotkeys(ConfigFile ConfigFile)
{
    public class PlayerAppearanceHotkey(ref ConfigFile ConfigFile, string Name, float Step, KeyCode Decrease, KeyCode Increase)
    {
        public ConfigEntry<float> Step = ConfigFile.Bind(Section, $"{Name}_Step", Step);
        public ConfigEntry<KeyCode>
            Increase = ConfigFile.Bind(Section, $"{Name}_Increase", Increase),
            Decrease = ConfigFile.Bind(Section, $"{Name}_Decrease", Decrease);
    }

    private const string Section = "Hotkeys";

    public PlayerAppearanceHotkey
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

    public ConfigEntry<KeyCode>
        FreeCamera = ConfigFile.Bind(Section, "FreeCamera", KeyCode.End),
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
using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

public sealed class NoClip(ConfigFile ConfigFile)
{
    private const string SECTION_NAME = "NoClip";

    public ConfigEntry<float> BaseSpeed = ConfigFile.Bind(SECTION_NAME, "BaseSpeed", 50f);
    public ConfigEntry<float> AlternativeBaseSpeed = ConfigFile.Bind(SECTION_NAME, "AlternativeBaseSpeed", 150f);

    public ConfigEntry<KeyCode>
        AlternativeSpeedKey = ConfigFile.Bind(SECTION_NAME, "NoClip_AlternativeSpeedKey", KeyCode.LeftShift),
        Move_Forward = ConfigFile.Bind(SECTION_NAME, "Move_Forward", KeyCode.W),
        Move_Right = ConfigFile.Bind(SECTION_NAME, "Move_Right", KeyCode.D),
        Move_Backward = ConfigFile.Bind(SECTION_NAME, "Move_Backward", KeyCode.S),
        Move_Left = ConfigFile.Bind(SECTION_NAME, "Move_Left", KeyCode.A),
        Move_Up = ConfigFile.Bind(SECTION_NAME, "Move_Up", KeyCode.Space),
        Move_Down = ConfigFile.Bind(SECTION_NAME, "Move_Down", KeyCode.LeftControl);
}
using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

public sealed class NoClip(ConfigFile configFile)
{
    private const string SECTION_NAME = "NoClip";

    public ConfigEntry<float> Speed = configFile.Bind(
        SECTION_NAME,
        "Speed",
        50f,
        new ConfigDescription(
            "Base movement speed.",
            new AcceptableValueRange<float>(0, 500)
        )
    );
    public ConfigEntry<float> AlternativeSpeed = configFile.Bind(
        SECTION_NAME,
        "AlternativeSpeed",
        150f,
        new ConfigDescription(
            "Alternative movement speed.",
            new AcceptableValueRange<float>(0, 1000)
        )
    );

    public ConfigEntry<KeyCode>
        AlternativeSpeedKey = configFile.Bind(SECTION_NAME, "AlternativeSpeedKey", KeyCode.LeftShift),
        Move_Forward = configFile.Bind(SECTION_NAME, "Move_Forward", KeyCode.W),
        Move_Right = configFile.Bind(SECTION_NAME, "Move_Right", KeyCode.D),
        Move_Backward = configFile.Bind(SECTION_NAME, "Move_Backward", KeyCode.S),
        Move_Left = configFile.Bind(SECTION_NAME, "Move_Left", KeyCode.A),
        Move_Up = configFile.Bind(SECTION_NAME, "Move_Up", KeyCode.Space),
        Move_Down = configFile.Bind(SECTION_NAME, "Move_Down", KeyCode.LeftControl);
}
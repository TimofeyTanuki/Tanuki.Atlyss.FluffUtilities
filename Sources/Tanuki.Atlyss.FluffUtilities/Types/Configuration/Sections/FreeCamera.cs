using BepInEx.Configuration;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

public sealed class FreeCamera(ConfigFile configFile)
{
    private const string SECTION_NAME = "FreeCamera";

    public readonly ConfigEntry<float> BaseSpeed = configFile.Bind(
        SECTION_NAME,
        "BaseSpeed",
        20f,
        new ConfigDescription(
            "Base movement speed.",
            new AcceptableValueRange<float>(0, 500)
        )
    );
    public readonly ConfigEntry<float> SpeedAdjustmentScrollMultiplier = configFile.Bind(
        SECTION_NAME,
        "SpeedAdjustmentScrollMultiplier",
        10f,
        new ConfigDescription(
            "The multiplier by which the mouse wheel adjusts the movement speed.",
            new AcceptableValueRange<float>(0.001f, 30)
        )
    );
    public readonly ConfigEntry<bool> LockCharacterControls = configFile.Bind(SECTION_NAME, "LockCharacterControls", true);

    public readonly ConfigEntry<bool> SmoothLook = configFile.Bind(SECTION_NAME, "SmoothLook", false);
    public readonly ConfigEntry<float> SmoothLook_Interpolation =
        configFile.Bind(
            SECTION_NAME,
            "SmoothLook_Interpolation",
            3f,
            new ConfigDescription(
                "Input responsiveness.",
                new AcceptableValueRange<float>(1, 25)
            )
        );

    public ConfigEntry<KeyCode>
        Move_Forward = configFile.Bind(SECTION_NAME, "Move_Forward", KeyCode.W),
        Move_Right = configFile.Bind(SECTION_NAME, "Move_Right", KeyCode.D),
        Move_Backward = configFile.Bind(SECTION_NAME, "Move_Backward", KeyCode.S),
        Move_Left = configFile.Bind(SECTION_NAME, "Move_Left", KeyCode.A),
        Move_Up = configFile.Bind(SECTION_NAME, "Move_Up", KeyCode.Space),
        Move_Down = configFile.Bind(SECTION_NAME, "Move_Down", KeyCode.LeftControl);
}
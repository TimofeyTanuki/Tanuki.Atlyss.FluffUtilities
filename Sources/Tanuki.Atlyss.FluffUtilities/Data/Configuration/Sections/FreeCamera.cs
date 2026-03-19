using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.Sections;

internal sealed class FreeCamera(ConfigFile configFile)
{
    private const string SECTION_NAME = "FreeCamera";

    public readonly ConfigEntry<float> Speed = configFile.Bind(SECTION_NAME, "Speed", 20f);
    public readonly ConfigEntry<float> ScrollSpeedAdjustmentStep = configFile.Bind(SECTION_NAME, "ScrollSpeedAdjustmentStep", 5f);
    public readonly ConfigEntry<bool> LockCharacterControls = configFile.Bind(SECTION_NAME, "LockCharacterControls", true);
    public readonly ConfigEntry<bool> SmoothLookMode = configFile.Bind(SECTION_NAME, "SmoothLookMode", false);
    public readonly ConfigEntry<float> SmoothLookModeInterpolation = configFile.Bind(SECTION_NAME, "SmoothLookModeInterpolation", 5f);
}
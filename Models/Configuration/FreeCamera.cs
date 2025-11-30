using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class FreeCamera(ConfigFile ConfigFile)
{
    private const string Section = "FreeCamera";
    public ConfigEntry<float> Speed = ConfigFile.Bind(Section, "Speed", 20f);
    public ConfigEntry<float> ScrollSpeedAdjustmentStep = ConfigFile.Bind(Section, "ScrollSpeedAdjustmentStep", 5f);
    public ConfigEntry<bool> LockCharacterControls = ConfigFile.Bind(Section, "LockCharacterControls", true);
    public ConfigEntry<bool> SmoothLookMode = ConfigFile.Bind(Section, "SmoothLookMode", false);
    public ConfigEntry<float> SmoothLookModeInterpolation = ConfigFile.Bind(Section, "SmoothLookModeInterpolation", 5f);
}
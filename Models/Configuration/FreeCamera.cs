using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class FreeCamera(ref ConfigFile ConfigFile)
{
    private const string Section = "FreeCamera";
    public ConfigEntry<float> Speed = ConfigFile.Bind(Section, "Speed", 20f);
    public ConfigEntry<float> ScrollSpeedAdjustmentStep = ConfigFile.Bind(Section, "ScrollSpeedAdjustmentStep", 5f);
}
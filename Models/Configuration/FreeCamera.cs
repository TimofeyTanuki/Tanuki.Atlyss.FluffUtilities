using BepInEx.Configuration;

namespace Tanuki.Atlyss.FluffUtilities.Models.Configuration;

internal class FreeCamera(ref ConfigFile ConfigFile)
{
    private const string Section = "FreeCamera";
    public ConfigEntry<float> BaseSpeed = ConfigFile.Bind(Section, "BaseSpeed", 20f);
    public ConfigEntry<float> ScrollSpeedStep = ConfigFile.Bind(Section, "ScrollSpeedStep", 20f);
}
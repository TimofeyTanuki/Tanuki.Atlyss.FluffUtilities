using BepInEx.Configuration;
using Tanuki.Atlyss.FluffUtilities.Types.Configuration.ConfigEntries;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

internal sealed class RaceDisplayParametersOverrideItem(string sectionName, ConfigFile configFile)
{
    public readonly ConfigEntry<bool> Enabled = configFile.Bind(sectionName, "Enabled", true);

    public readonly FloatRange
        HeadWidth =
            new(configFile,
                new(new(sectionName, "HeadWidth_Minimum"), 0),
                new(new(sectionName, "HeadWidth_Maximum"), 4)
            ),
        MuzzleLength =
            new(configFile,
                new(new(sectionName, "HeadWidth_Minimum"), -500),
                new(new(sectionName, "HeadWidth_Maximum"), 1000)
            ),
        Height =
            new(configFile,
                new(new(sectionName, "HeadWidth_Minimum"), 0),
                new(new(sectionName, "HeadWidth_Maximum"), 5)
            ),
        Width =
            new(configFile,
                new(new(sectionName, "Height_Minimum"), 0),
                new(new(sectionName, "Height_Maximum"), 5)
            ),
        TorsoSize =
            new(configFile,
                new(new(sectionName, "Width_Minimum"), -100),
                new(new(sectionName, "Width_Maximum"), 1000)
            ),
        BreastSize =
            new(configFile,
                new(new(sectionName, "TorsoSize_Minimum"), -100),
                new(new(sectionName, "TorsoSize_Maximum"), 1000)
            ),
        ArmsSize =
            new(configFile,
                new(new(sectionName, "BreastSize_Minimum"), -200),
                new(new(sectionName, "BreastSize_Maximum"), 1000)
            ),
        BellySize =
            new(configFile,
                new(new(sectionName, "BellySize_Minimum"), -300),
                new(new(sectionName, "BellySize_Maximum"), 1000)
            ),
        BottomSize =
            new(configFile,
                new(new(sectionName, "BottomSize_Minimum"), -300),
                new(new(sectionName, "BottomSize_Maximum"), 1000)
            ),
        VoicePitch =
            new(configFile,
                new(new(sectionName, "VoicePitch_Minimum"), 0.05f),
                new(new(sectionName, "VoicePitch_Maximum"), 3)
            );
}

using BepInEx.Configuration;
using Tanuki.Atlyss.FluffUtilities.Data.Configuration.ConfigEntries;

namespace Tanuki.Atlyss.FluffUtilities.Data.Configuration.Sections;

internal sealed class GlobalRaceDisplayParameters(ConfigFile configFile)
{
    private const string SECTION_NAME = "GlobalRaceDisplayParameters";

    public readonly ConfigEntry<bool> Override = configFile.Bind(SECTION_NAME, "Override", true);

    public readonly FloatRange
        HeadWidth =
            new(configFile,
                new(new(SECTION_NAME, "HeadWidth_Minimum"), 0),
                new(new(SECTION_NAME, "HeadWidth_Maximum"), 4)
            ),
        MuzzleLength =
            new(configFile,
                new(new(SECTION_NAME, "HeadWidth_Minimum"), -500),
                new(new(SECTION_NAME, "HeadWidth_Maximum"), 1000)
            ),
        Height =
            new(configFile,
                new(new(SECTION_NAME, "HeadWidth_Minimum"), 0),
                new(new(SECTION_NAME, "HeadWidth_Maximum"), 5)
            ),
        Width =
            new(configFile,
                new(new(SECTION_NAME, "Height_Minimum"), 0),
                new(new(SECTION_NAME, "Height_Maximum"), 5)
            ),
        TorsoSize =
            new(configFile,
                new(new(SECTION_NAME, "Width_Minimum"), -100),
                new(new(SECTION_NAME, "Width_Maximum"), 1000)
            ),
        BreastSize =
            new(configFile,
                new(new(SECTION_NAME, "TorsoSize_Minimum"), -100),
                new(new(SECTION_NAME, "TorsoSize_Maximum"), 1000)
            ),
        ArmsSize =
            new(configFile,
                new(new(SECTION_NAME, "BreastSize_Minimum"), -200),
                new(new(SECTION_NAME, "BreastSize_Maximum"), 1000)
            ),
        BellySize =
            new(configFile,
                new(new(SECTION_NAME, "BellySize_Minimum"), -300),
                new(new(SECTION_NAME, "BellySize_Maximum"), 1000)
            ),
        BottomSize =
            new(configFile,
                new(new(SECTION_NAME, "BottomSize_Minimum"), -300),
                new(new(SECTION_NAME, "BottomSize_Maximum"), 1000)
            ),
        VoicePitch =
            new(configFile,
                new(new(SECTION_NAME, "VoicePitch_Minimum"), 0.05f),
                new(new(SECTION_NAME, "VoicePitch_Maximum"), 3)
            );
}
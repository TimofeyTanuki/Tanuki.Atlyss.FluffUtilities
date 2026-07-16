using BepInEx.Configuration;
using Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

namespace Tanuki.Atlyss.FluffUtilities;

internal sealed class Configuration
{
    public static Configuration Instance = null!;

    public Types.Configuration.Sections.Commands Commands;
    public PlayerAppearance PlayerAppearance;
    public GlobalRaceDisplayParameters GlobalRaceDisplayParameters;
    public FreeCamera FreeCamera;
    public NoClip NoClip;
    public Hotkeys Hotkeys;
    public General General;

    public static void Initialize(ConfigFile configFile)
    {
        if (Instance is not null)
            return;

        Instance = new(configFile);
    }

    private Configuration(ConfigFile configFile)
    {
        Commands = new(configFile);
        PlayerAppearance = new(configFile);
        GlobalRaceDisplayParameters = new(configFile);
        FreeCamera = new(configFile);
        NoClip = new(configFile);
        Hotkeys = new(configFile);
        General = new(configFile);
    }
}

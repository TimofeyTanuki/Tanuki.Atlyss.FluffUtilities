using BepInEx.Configuration;
using Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

namespace Tanuki.Atlyss.FluffUtilities;

public sealed class Configuration
{
    internal static Configuration instance = null!;

    public static Configuration Instance => instance;

    public Types.Configuration.Sections.Commands Commands;
    public PlayerAppearance PlayerAppearance;
    public GlobalRaceDisplayParameters GlobalRaceDisplayParameters;
    public FreeCamera FreeCamera;
    public NoClip NoClip;
    public Hotkeys Hotkeys;
    public General General;

    public static void Initialize(ConfigFile configFile)
    {
        if (instance is not null)
            return;

        instance = new(configFile);
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

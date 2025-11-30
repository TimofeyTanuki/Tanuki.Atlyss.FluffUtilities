using BepInEx.Configuration;
using Tanuki.Atlyss.FluffUtilities.Models.Configuration;

namespace Tanuki.Atlyss.FluffUtilities;

internal class Configuration
{
    public static Configuration Instance;

    public Models.Configuration.Commands Commands;
    public PlayerAppearance PlayerAppearance;
    public GlobalRaceDisplayParameters GlobalRaceDisplayParameters;
    public FreeCamera FreeCamera;
    public NoClip NoClip;
    public Hotkeys Hotkeys;
    public General General;

    private Configuration() { }
    public static void Initialize() =>
        Instance ??= new();

    public void Load(ConfigFile ConfigFile)
    {
        Commands = new(ConfigFile);
        PlayerAppearance = new(ConfigFile);
        GlobalRaceDisplayParameters = new(ConfigFile);
        FreeCamera = new(ConfigFile);
        NoClip = new(ConfigFile);
        Hotkeys = new(ConfigFile);
        General = new(ConfigFile);
    }
}
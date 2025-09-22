using BepInEx.Configuration;
using Tanuki.Atlyss.FluffUtilities.Models.Configuration;

namespace Tanuki.Atlyss.FluffUtilities;

internal class Configuration
{
    public static Configuration Instance;

    private Configuration() { }
    public static void Initialize()
    {
        Instance ??= new();
    }

    public Models.Configuration.Commands Commands;
    public PlayerAppearance PlayerAppearance;
    public GlobalRaceDisplayParameters GlobalRaceDisplayParameters;
    public FreeCamera FreeCamera;
    public NoClip NoClip;
    public Hotkeys Hotkeys;
    public General General;

    public void Load(ConfigFile ConfigFile)
    {
        Commands = new(ref ConfigFile);
        PlayerAppearance = new(ref ConfigFile);
        GlobalRaceDisplayParameters = new(ref ConfigFile);
        FreeCamera = new(ref ConfigFile);
        NoClip = new(ref ConfigFile);
        Hotkeys = new(ref ConfigFile);
        General = new(ref ConfigFile);
    }
}
using Tanuki.Atlyss.FluffUtilities.Managers.Game.Controllers;

namespace Tanuki.Atlyss.FluffUtilities.Managers.Game;

public sealed class Manager
{
    private readonly Race raceController;
    private readonly PlayerAppearance playerAppearanceController;

    public Race RaceController => raceController;
    public PlayerAppearance PlayerAppearanceController => playerAppearanceController;

    internal Manager()
    {
        raceController = new();
        playerAppearanceController = new();
    }
}

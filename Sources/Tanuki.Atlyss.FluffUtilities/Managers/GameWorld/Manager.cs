using Tanuki.Atlyss.FluffUtilities.Managers.GameWorld.Controllers;

namespace Tanuki.Atlyss.FluffUtilities.Managers.GameWorld;

public sealed class Manager
{
    private readonly Time timeController;
    private readonly Weather weatherController;

    public Time TimeController => timeController;
    public Weather WeatherController => weatherController;

    internal Manager()
    {
        timeController = new();
        weatherController = new();
    }
}

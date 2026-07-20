using Tanuki.Atlyss.FluffUtilities.Managers.GameWorld.Controllers;

namespace Tanuki.Atlyss.FluffUtilities.Managers.GameWorld;

public sealed class Manager
{
    private readonly Time timeController;

    public Time TimeController => timeController;

    internal Manager()
    {
        timeController = new();
    }

}

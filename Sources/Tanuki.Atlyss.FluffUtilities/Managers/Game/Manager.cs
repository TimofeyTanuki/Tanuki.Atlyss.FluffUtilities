using System;
using System.Collections.Generic;
using System.Text;
using Tanuki.Atlyss.FluffUtilities.Managers.Game.Controllers;

namespace Tanuki.Atlyss.FluffUtilities.Managers.Game;

public sealed class Manager
{
    private readonly Race raceController;

    public Race RaceController => raceController;

    internal Manager()
    {
        raceController = new();
    }
}

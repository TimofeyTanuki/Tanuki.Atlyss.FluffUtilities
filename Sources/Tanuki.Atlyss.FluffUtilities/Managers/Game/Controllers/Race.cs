using System;
using System.Collections.Generic;

namespace Tanuki.Atlyss.FluffUtilities.Managers.Game.Controllers;

public sealed class Race
{
    private static GameManager GameManager => GameManager._current;
    private IReadOnlyDictionary<string, ScriptablePlayerRace> CachedRaces => Atlyss.Game.Accessors.GameManager._cachedScriptableRaces(GameManager);

    internal Race()
    {
        Atlyss.Game.Patches.GameManager.Cache_ScriptableAssets.OnPostfix += OnScriptableAssetsCached;
    }

    private void OnScriptableAssetsCached()
    {
        Override();
    }

    public void Override()
    {
        foreach (ScriptablePlayerRace scriptablePlayerRace in CachedRaces.Values)
        {
            CharacterParamsGroup characterParamsGroup = scriptablePlayerRace._raceDisplayParams;

            Console.WriteLine($"RACE: {scriptablePlayerRace._raceName}");
        }
    }
}

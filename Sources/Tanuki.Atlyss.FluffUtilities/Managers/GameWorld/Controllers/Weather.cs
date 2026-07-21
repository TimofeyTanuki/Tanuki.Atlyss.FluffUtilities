using Mirror;

namespace Tanuki.Atlyss.FluffUtilities.Managers.GameWorld.Controllers;

public sealed class Weather
{
    private static MapInstance MapInstance => Player._mainPlayer._playerMapInstance;

    private bool lastWeatherState = false;
    private bool cachedWeatherState = false;

    private bool hostSync = true;

    public bool HostSync => hostSync;

    internal Weather() =>
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;

    private void OnStopClient() =>
        SetHostSyncState(true);

    public void SetHostSyncState(bool active)
    {
        if (hostSync == active)
            return;

        if (!Player._mainPlayer || Player._mainPlayer.Network_isHostPlayer)
            return;

        hostSync = active;

        if (active)
        {
            Game.Patches.MapInstance.DeserializeSyncVars.OnPrefix -= CacheHostWeatherOnSync;
            Game.Patches.MapInstance.DeserializeSyncVars.OnPostfix -= ApplySimulationToMapAfterSync;

            ApplyHostWeatherToMap();
        }
        else
        {
            CacheSynchronizedVariables();

            Game.Patches.MapInstance.DeserializeSyncVars.OnPrefix += CacheHostWeatherOnSync;
            Game.Patches.MapInstance.DeserializeSyncVars.OnPostfix += ApplySimulationToMapAfterSync;
        }
    }

    private void ApplySimulationToMapAfterSync(MapInstance instance, NetworkReader reader, bool arg3, int arg4) => ApplySimulationToMap();

    private void CacheHostWeatherOnSync(MapInstance instance, NetworkReader reader, bool initialState, int initialPosition)
    {
        long mask = (long)reader.ReadULong();

        if ((mask & 8L) == 0L)
            return;

        if ((mask & 1L) != 0L)
            reader.Position += sizeof(byte);

        if ((mask & 2L) != 0L)
            reader.Position += sizeof(byte);

        if ((mask & 4L) != 0L)
            reader.Position += sizeof(int);

        lastWeatherState = instance._isWeatherEnabled;
        cachedWeatherState = reader.ReadBool();
    }

    private void ApplyHostWeatherToMap()
    {
        MapInstance mapInstance = MapInstance;

        if (!mapInstance)
            return;

        mapInstance._isWeatherEnabled = cachedWeatherState;
    }

    public void ApplySimulationToMap()
    {
        MapInstance mapInstance = MapInstance;

        if (!mapInstance)
            return;

        mapInstance._isWeatherEnabled = lastWeatherState;
    }

    private void CacheSynchronizedVariables()
    {
        MapInstance mapInstance = MapInstance;

        if (!mapInstance)
            return;

        lastWeatherState = cachedWeatherState = mapInstance._isWeatherEnabled;
    }
}

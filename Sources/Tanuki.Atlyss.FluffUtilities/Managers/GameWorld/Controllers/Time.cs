using Mirror;

namespace Tanuki.Atlyss.FluffUtilities.Managers.GameWorld.Controllers;

public sealed class Time
{
    private MapInstance MapInstance => Player._mainPlayer._playerMapInstance;
    private GameWorldManager GameWorldManager => GameWorldManager._current;

    private bool hostSync = true;
    public int cachedTime;
    public WorldTime cachedWorldTime;
    public ClockSetting cachedClockSetting;

    public bool HostSync => hostSync;

    internal Time() =>
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;

    public void SetHostSyncState(bool active)
    {
        if (hostSync == active)
            return;

        if (!Player._mainPlayer || Player._mainPlayer.Network_isHostPlayer)
            return;

        hostSync = active;

        if (active)
        {
            Game.Patches.GameWorldManager.Server_DayNightCycleRuntime.OnPostfix -= SimulateLocalDayNightCycle;
            Game.Patches.MapInstance.DeserializeSyncVars.OnPostfix -= ApplyLocalTimeToMapAfterSync;
            Game.Patches.MapInstance.DeserializeSyncVars.OnPrefix -= CacheHostTimeOnSync;

            ApplyHostTimeToMap();
        }
        else
        {
            CacheSynchronizedTime();

            Game.Patches.GameWorldManager.Server_DayNightCycleRuntime.OnPostfix += SimulateLocalDayNightCycle;
            Game.Patches.MapInstance.DeserializeSyncVars.OnPostfix += ApplyLocalTimeToMapAfterSync;
            Game.Patches.MapInstance.DeserializeSyncVars.OnPrefix += CacheHostTimeOnSync;
        }
    }

    private void OnStopClient() => SetHostSyncState(true);

    private void SimulateLocalDayNightCycle()
    {
        GameWorldManager gameWorldManager = GameWorldManager;

        if (gameWorldManager._dayNightCycleInterval <= 0)
            return;

        ref float currentDayNightCycleBuffer = ref Game.Accessors.GameWorldManager._currentDayNightCycleBuffer(gameWorldManager);

        if (currentDayNightCycleBuffer < gameWorldManager._dayNightCycleInterval)
        {
            currentDayNightCycleBuffer += UnityEngine.Time.deltaTime;
            return;
        }

        currentDayNightCycleBuffer = 0;

        gameWorldManager._timeDisplay++;

        if (gameWorldManager._timeDisplay > 11)
        {
            if (gameWorldManager._clockSetting == ClockSetting.AM && gameWorldManager._worldTime == WorldTime.DAY)
                gameWorldManager._clockSetting = ClockSetting.PM;
            else if (gameWorldManager._clockSetting == ClockSetting.PM && gameWorldManager._worldTime == WorldTime.NIGHT)
                gameWorldManager._clockSetting = ClockSetting.AM;

            if (gameWorldManager._timeDisplay > 12)
                gameWorldManager._timeDisplay = 1;
        }
        else
        {
            if (gameWorldManager._timeDisplay == 6 && gameWorldManager._clockSetting == ClockSetting.AM)
                gameWorldManager._worldTime = WorldTime.DAY;
            else if (gameWorldManager._timeDisplay == 8 && gameWorldManager._clockSetting == ClockSetting.PM)
                gameWorldManager._worldTime = WorldTime.NIGHT;
        }

        ApplyLocalTimeToMap();
    }

    public void ApplyLocalTimeToMap()
    {
        MapInstance mapInstance = MapInstance;

        if (!mapInstance)
            return;

        GameWorldManager gameWorldManager = GameWorldManager;

        mapInstance._instanceWorldTime = gameWorldManager._worldTime;
        mapInstance._instanceClockSetting = gameWorldManager._clockSetting;
        mapInstance._instanceTime = gameWorldManager._timeDisplay;
    }

    private void ApplyHostTimeToMap()
    {
        MapInstance mapInstance = MapInstance;

        if (!mapInstance)
            return;

        mapInstance._instanceWorldTime = cachedWorldTime;
        mapInstance._instanceClockSetting = cachedClockSetting;
        mapInstance._instanceTime = cachedTime;
    }

    private void CacheSynchronizedTime()
    {
        MapInstance mapInstance = MapInstance;

        if (!mapInstance)
            return;

        cachedWorldTime = mapInstance._instanceWorldTime;
        cachedClockSetting = mapInstance._instanceClockSetting;
        cachedTime = mapInstance._instanceTime;
    }

    private void ApplyLocalTimeToMapAfterSync(MapInstance instance, NetworkReader reader, bool arg3, int arg4) => ApplyLocalTimeToMap();

    private void CacheHostTimeOnSync(MapInstance instance, NetworkReader reader, bool initialState, int initialPosition)
    {
        long mask = (long)reader.ReadULong();

        if ((mask & 1L) != 0L)
            cachedWorldTime = (WorldTime)reader.ReadByte();

        if ((mask & 2L) != 0L)
            cachedClockSetting = (ClockSetting)reader.ReadByte();

        if ((mask & 4L) != 0L)
            cachedTime = reader.ReadInt();
    }
}

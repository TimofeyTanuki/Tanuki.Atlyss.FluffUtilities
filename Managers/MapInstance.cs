using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class MapInstance
{
    public static MapInstance Instance;

    private WorldTime CachedNetworkWorldTime;
    private int CachedNetworkInstanceTime;
    private ClockSetting CachedNetworkClockSetting;
    private bool CachedNetworkIsWeatherEnabled;
    private bool CachedIsWeatherEnabled;
    private global::MapInstance CachedMapInstance;
    public MapInstanceVisuals CachedMapVisuals;

    private bool HostTime = false;
    private bool HostWeather = false;

    public bool IsHostTime => HostTime;
    public bool IsHostWeather => HostWeather;

    public MapInstance() =>
        Game.Patches.Player.OnStartAuthority_Postfix.OnInvoke += OnStartAuthority_Postfix_OnInvoke;

    private void OnStartAuthority_Postfix_OnInvoke()
    {
        HostTime = HostWeather = true;

        if (!Player._mainPlayer._isHostPlayer)
            Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;
    }

    public static void Initialize() => Instance ??= new MapInstance();

    private void OnStopClient_Prefix_OnInvoke()
    {
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= OnStopClient_Prefix_OnInvoke;

        FollowHost();
    }

    private void HandleDayNightCycle(ref bool ShouldAllow)
    {
        RefreshMapInstanceCache();

        ShouldAllow = false;

        GameWorldManager GameWorldManager = GameWorldManager._current;

        if (GameWorldManager._dayNightCycleInterval <= 0f)
            return;

        if (Game.Accessors.GameWorldManager._currentDayNightCycleBuffer(GameWorldManager) < GameWorldManager._dayNightCycleInterval)
        {
            Game.Accessors.GameWorldManager._currentDayNightCycleBuffer(GameWorldManager) += Time.deltaTime;
            return;
        }

        Game.Accessors.GameWorldManager._currentDayNightCycleBuffer(GameWorldManager) = 0f;

        GameWorldManager._timeDisplay++;

        if (GameWorldManager._timeDisplay > 11 && GameWorldManager._clockSetting == ClockSetting.AM && GameWorldManager._worldTime == WorldTime.DAY)
            GameWorldManager._clockSetting = ClockSetting.PM;
        else if (GameWorldManager._timeDisplay > 11 && GameWorldManager._clockSetting == ClockSetting.PM && GameWorldManager._worldTime == WorldTime.NIGHT)
            GameWorldManager._clockSetting = ClockSetting.AM;

        if (GameWorldManager._timeDisplay > 12)
            GameWorldManager._timeDisplay = 1;

        if (GameWorldManager._timeDisplay == 6 && GameWorldManager._clockSetting == ClockSetting.AM)
            GameWorldManager._worldTime = WorldTime.DAY;
        else if (GameWorldManager._timeDisplay == 8 && GameWorldManager._clockSetting == ClockSetting.PM)
            GameWorldManager._worldTime = WorldTime.NIGHT;

        UpdateClientTime();
    }

    public void Unload() =>
        FollowHost();

    public void UpdateClientTime()
    {
        RefreshMapInstanceCache();

        if (!CachedMapInstance)
            return;

        GameWorldManager GameWorldManager = GameWorldManager._current;

        CachedMapInstance._instanceWorldTime = GameWorldManager._worldTime;
        CachedMapInstance._instanceClockSetting = GameWorldManager._clockSetting;
        CachedMapInstance._instanceTime = GameWorldManager._timeDisplay;
    }

    public void UpdateClientWeather() =>
        CachedMapInstance._isWeatherEnabled = CachedIsWeatherEnabled;

    private void DeserializeSyncVars_OnBefore(global::MapInstance MapInstance, NetworkReader NetworkReader, bool initialState, ref bool ShouldAllow)
    {
        long Mask = (long)NetworkReader.ReadULong();

        if ((Mask & 1L) != 0L)
            CachedNetworkWorldTime = (WorldTime)NetworkReader.ReadByte();

        if ((Mask & 2L) != 0L)
            CachedNetworkClockSetting = (ClockSetting)NetworkReader.ReadByte();

        if ((Mask & 4L) != 0L)
            CachedNetworkInstanceTime = NetworkReader.ReadInt();

        if ((Mask & 8L) != 0L)
            CachedNetworkIsWeatherEnabled = NetworkReader.ReadBool();
    }

    private void RefreshMapInstanceCache()
    {
        if (Player._mainPlayer._playerMapInstance == CachedMapInstance)
            return;

        CachedMapInstance = Player._mainPlayer._playerMapInstance;
        CachedMapVisuals = Game.Accessors.MapInstance._mapVisuals(CachedMapInstance);
        CachedMapVisuals._weatherIntervalBuffer = 60;
    }

    private void HandleWeather(global::MapInstance MapInstance, ref bool ShouldAllow)
    {
        RefreshMapInstanceCache();

        if (!CachedMapVisuals._weatherParticleSystem)
            return;

        if (CachedMapVisuals._chanceForWeather <= 0)
            return;

        float CurrentWeatherIntervalBuffer = Game.Accessors.MapInstance._currentWeatherIntervalBuffer(MapInstance);

        if (CurrentWeatherIntervalBuffer < CachedMapVisuals._weatherIntervalBuffer)
        {
            CurrentWeatherIntervalBuffer += Time.deltaTime;
            Game.Accessors.MapInstance._currentWeatherIntervalBuffer(MapInstance) = CurrentWeatherIntervalBuffer;
            return;
        }

        Game.Accessors.MapInstance._currentWeatherIntervalBuffer(MapInstance) = 0;

        if (CachedMapVisuals._chanceForWeather < Random.Range(0, 100))
            return;

        CachedIsWeatherEnabled = !MapInstance._isWeatherEnabled;
        MapInstance._isWeatherEnabled = CachedIsWeatherEnabled;
    }

    private void RefreshCaching()
    {
        if (Player._mainPlayer._isHostPlayer)
            return;

        if (!(HostTime && HostWeather))
        {
            Game.Patches.MapInstance.DeserializeSyncVars.OnBefore -= DeserializeSyncVars_OnBefore;
            Game.Patches.MapInstance.DeserializeSyncVars.OnBefore += DeserializeSyncVars_OnBefore;
            return;
        }

        Game.Patches.MapInstance.DeserializeSyncVars.OnBefore -= DeserializeSyncVars_OnBefore;
    }

    public void FollowHostTime()
    {
        if (HostTime)
            return;

        HostTime = true;

        RefreshCaching();

        Game.Patches.GameWorldManager.Server_DayNightCycleRuntime_Prefix.OnInvoke -= HandleDayNightCycle;
        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter -= HandleTimeOverride;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;

        MapInstance._instanceWorldTime = CachedNetworkWorldTime;
        MapInstance._instanceClockSetting = CachedNetworkClockSetting;
        MapInstance._instanceTime = CachedNetworkInstanceTime;
    }

    public void FollowClientTime()
    {
        if (Player._mainPlayer._isHostPlayer)
            return;

        if (!HostTime)
            return;

        Main.Instance.Patcher.Use(
            typeof(Game.Patches.MapInstance.DeserializeSyncVars),
            typeof(Game.Patches.GameWorldManager.Server_DayNightCycleRuntime_Prefix)
        );

        HostTime = false;

        RefreshCaching();

        Game.Patches.GameWorldManager.Server_DayNightCycleRuntime_Prefix.OnInvoke += HandleDayNightCycle;
        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter += HandleTimeOverride;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;

        CachedNetworkWorldTime = MapInstance._instanceWorldTime;
        CachedNetworkClockSetting = MapInstance._instanceClockSetting;
        CachedNetworkInstanceTime = MapInstance._instanceTime;
    }

    private void HandleTimeOverride(global::MapInstance MapInstance, NetworkReader NetworkReader, bool InitialState, bool Cancelled) =>
        UpdateClientTime();

    public void FollowHostWeather()
    {
        if (HostWeather)
            return;

        HostWeather = true;

        RefreshCaching();

        Game.Patches.MapInstance.Handle_InstanceRuntime_Prefix.OnInvoke -= HandleWeather;
        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter -= HandleWeatherOverride;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;

        MapInstance._isWeatherEnabled = CachedIsWeatherEnabled;
    }

    private void HandleWeatherOverride(global::MapInstance MapInstance, NetworkReader NetworkReader, bool InitialState, bool Cancelled) =>
        UpdateClientWeather();

    public void FollowClientWeather()
    {
        if (Player._mainPlayer._isHostPlayer)
            return;

        if (!HostWeather)
            return;

        Main.Instance.Patcher.Use(
            typeof(Game.Patches.MapInstance.Handle_InstanceRuntime_Prefix),
            typeof(Game.Patches.MapInstance.DeserializeSyncVars)
        );

        HostWeather = false;

        RefreshCaching();

        Game.Patches.MapInstance.Handle_InstanceRuntime_Prefix.OnInvoke += HandleWeather;
        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter += HandleWeatherOverride;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;

        CachedIsWeatherEnabled = MapInstance._isWeatherEnabled;
    }

    public void FollowHost()
    {
        Game.Patches.MapInstance.DeserializeSyncVars.OnBefore -= DeserializeSyncVars_OnBefore;

        FollowHostTime();
        FollowHostWeather();
        RefreshCaching();
    }
}
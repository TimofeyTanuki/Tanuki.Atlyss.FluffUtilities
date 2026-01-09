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

    public void Load()
    {
        Main.Instance.Patcher.Use(
            typeof(Game.Patches.MapInstance.DeserializeSyncVars),
            typeof(Game.Patches.GameWorldManager.Server_DayNightCycleRuntime_Prefix),
            typeof(Game.Patches.MapInstance.Handle_InstanceRuntime_Prefix)
        );
    }

    private void OnStopClient_Prefix_OnInvoke()
    {
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= OnStopClient_Prefix_OnInvoke;

        FollowHost();
    }

    private void HandleDayNightCycleOverride(ref bool ShouldAllow)
    {
        ShouldAllow = false;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;
        if (!MapInstance)
            return;

        SetLocalTime(MapInstance);
    }

    public void Unload() =>
        FollowHost();

    private void SetLocalTime(global::MapInstance MapInstance)
    {
        GameWorldManager GameWorldManager = GameWorldManager._current;

        MapInstance._instanceWorldTime = GameWorldManager._worldTime;
        MapInstance._instanceClockSetting = GameWorldManager._clockSetting;
        MapInstance._instanceTime = GameWorldManager._timeDisplay;
    }

    private void SetLocalWeather(global::MapInstance MapInstance) =>
        MapInstance._isWeatherEnabled = CachedIsWeatherEnabled;

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

    private void HandleWeatherOverride(global::MapInstance MapInstance, ref bool ShouldAllow)
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

        Game.Patches.GameWorldManager.Server_DayNightCycleRuntime_Prefix.OnInvoke -= HandleDayNightCycleOverride;
        Game.Patches.MapInstance.Handle_InstanceRuntime_Prefix.OnInvoke -= HandleWeatherOverride;
        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter -= HandleTimeOverride;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;

        MapInstance._instanceWorldTime = CachedNetworkWorldTime;
        MapInstance._instanceClockSetting = CachedNetworkClockSetting;
        MapInstance._instanceTime = CachedNetworkInstanceTime;
        MapInstance._isWeatherEnabled = CachedNetworkIsWeatherEnabled;
    }

    public void FollowClientTime()
    {
        if (Player._mainPlayer._isHostPlayer)
            return;

        if (!HostTime)
            return;

        HostTime = false;

        RefreshCaching();

        Game.Patches.GameWorldManager.Server_DayNightCycleRuntime_Prefix.OnInvoke += HandleDayNightCycleOverride;
        Game.Patches.MapInstance.Handle_InstanceRuntime_Prefix.OnInvoke += HandleWeatherOverride;
        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter += HandleTimeOverride;

        global::MapInstance MapInstance = Player._mainPlayer._playerMapInstance;

        CachedNetworkWorldTime = MapInstance._instanceWorldTime;
        CachedNetworkClockSetting = MapInstance._instanceClockSetting;
        CachedNetworkInstanceTime = MapInstance._instanceTime;
        CachedNetworkIsWeatherEnabled = MapInstance._isWeatherEnabled;
    }

    private void HandleTimeOverride(global::MapInstance MapInstance, NetworkReader NetworkReader, bool InitialState, bool Cancelled) =>
        SetLocalTime(MapInstance);

    public void FollowHostWeather()
    {
        if (HostWeather)
            return;

        HostWeather = true;

        RefreshCaching();

        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter += HandleWeatherOverride;
    }

    private void HandleWeatherOverride(global::MapInstance MapInstance, NetworkReader NetworkReader, bool InitialState, bool Cancelled) =>
        SetLocalWeather(MapInstance);

    public void FollowClientWeather()
    {
        if (Player._mainPlayer._isHostPlayer)
            return;

        if (!HostWeather)
            return;

        HostWeather = false;

        RefreshCaching();

        Game.Patches.MapInstance.DeserializeSyncVars.OnAfter -= HandleWeatherOverride;
    }

    public void FollowHost()
    {
        Game.Patches.MapInstance.DeserializeSyncVars.OnBefore -= DeserializeSyncVars_OnBefore;

        FollowHostTime();
        FollowHostWeather();
        RefreshCaching();
    }
}
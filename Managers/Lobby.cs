using Steamworks;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class Lobby
{
    public static Lobby Instance;
    public CSteamID LobbySteamID = CSteamID.Nil;
    public CSteamID LobbyOwnerSteamID = CSteamID.Nil;
    public CSteamID PlayerSteamID = CSteamID.Nil;
    public const string LobbyMemberDataKey_Version = $"{PluginInfo.GUID}.ver";

    public bool PluginDataSent = false;

    private Lobby()
    {
        Game.Patches.Player.OnStartAuthority_Postfix.OnInvoke += OnStartAuthority_Postfix_OnInvoke;
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;
    }

    public static void Initialize() => Instance ??= new();

    public void Load()
    {
        if (!Configuration.Instance.General.OtherPluginUserNotificationOnJoin.Value)
            return;

        Game.Managers.Player.OnPlayerInitialized += Player_OnPlayerInitialized;
    }

    private void Player_OnPlayerInitialized(Player Player)
    {
        if (!Player._mainPlayer)
            return;

        if (Player._mainPlayer.netId >= Player.netId)
            return;

        if (!ulong.TryParse(Player._steamID, out ulong SteamID))
            return;

        string Version = GetUserPluginVersion(new(SteamID));

        if (string.IsNullOrEmpty(Version))
            return;

        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "General.OtherPluginUserMessage",
                Player.netId,
                Player._nickname,
                string.IsNullOrEmpty(Player._globalNickname) ?
                    string.Empty : Main.Instance.Translate("General.OtherPluginUserMessage.GlobalNickname", Player._globalNickname),
                Version
            )
        );
    }

    public void Unload()
    {
        if (!Configuration.Instance.General.OtherPluginUserNotificationOnJoin.Value)
            return;

        Game.Managers.Player.OnPlayerInitialized -= Player_OnPlayerInitialized;
    }

    public void Reload()
    {
        Unload();
        Load();

        if (!Player._mainPlayer)
            return;

        if (AtlyssNetworkManager._current._soloMode)
            return;

        ApplySteamLobbyPluginData();
    }

    private string GetUserPluginVersion(CSteamID SteamID) =>
        SteamMatchmaking.GetLobbyMemberData(LobbySteamID, SteamID, LobbyMemberDataKey_Version);

    private void ApplySteamLobbyPluginData()
    {
        if (Configuration.Instance.General.HideUsagePresenceFromNonUserHosts.Value &&
            !CheckPluginUserHost() &&
            PluginDataSent)
        {

            SteamMatchmaking.SetLobbyMemberData(LobbySteamID, LobbyMemberDataKey_Version, string.Empty);
            PluginDataSent = false;

            return;
        }

        SteamMatchmaking.SetLobbyMemberData(LobbySteamID, LobbyMemberDataKey_Version, PluginInfo.Version);
        PluginDataSent = true;
    }

    private bool CheckPluginUserHost()
    {
        if (LobbyOwnerSteamID == PlayerSteamID)
            return true;

        return !string.IsNullOrEmpty(GetUserPluginVersion(LobbyOwnerSteamID));
    }

    private void OnStartAuthority_Postfix_OnInvoke(Player Player)
    {
        if (AtlyssNetworkManager._current._soloMode)
            return;

        LobbySteamID = new CSteamID(SteamLobby._current._currentLobbyID);
        LobbyOwnerSteamID = SteamMatchmaking.GetLobbyOwner(LobbySteamID);
        PlayerSteamID = Player._mainPlayer._isHostPlayer ? LobbyOwnerSteamID : new CSteamID(ulong.Parse(Player._mainPlayer._steamID));

        ApplySteamLobbyPluginData();
    }

    private void OnStopClient_Prefix_OnInvoke()
    {
        LobbySteamID = LobbyOwnerSteamID = CSteamID.Nil;
        PluginDataSent = false;
    }
}
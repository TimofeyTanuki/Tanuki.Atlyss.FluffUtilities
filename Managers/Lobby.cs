using Mirror;
using Steamworks;
using System.Collections;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class Lobby
{
    public static Lobby Instance;
    public CSteamID LobbyID = CSteamID.Nil;
    public CSteamID PlayerID = CSteamID.Nil;

    public const string LobbyMemberDataKey_Version = $"{PluginInfo.ID}.ver";

    private Lobby()
    {
        Game.Events.Player.OnStartAuthority_Postfix.OnInvoke += OnStartAuthority_Postfix_OnInvoke;
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;

    }
    public static void Initialize()
    {
        if (Instance is not null)
            return;

        Instance = new();
    }
    public void Load()
    {
        if (!Configuration.Instance.General.Plugin_ShowOtherPluginUserMessageOnJoin.Value)
            return;

        Game.Main.Instance.Patch(
            typeof(Game.Events.Player.DeserializeSyncVars_Postfix)
        );

        Game.Events.Player.DeserializeSyncVars_Postfix.OnInvoke += DeserializeSyncVars_Postfix_OnInvoke;
    }

    private void DeserializeSyncVars_Postfix_OnInvoke(Player Player, NetworkReader NetworkReader, bool InitialState)
    {
        if (!InitialState)
            return;

        if (Player._mainPlayer is null)
            return;

        if (Player._mainPlayer.netIdentity.netId >= Player.netIdentity.netId)
            return;

        Main.Instance.StartCoroutine(CheckPlayerPlugin(Player));
    }
    private IEnumerator CheckPlayerPlugin(Player Player)
    {
        string Version = SteamMatchmaking.GetLobbyMemberData(new(ulong.Parse(Player._steamID)), LobbyID, LobbyMemberDataKey_Version);

        if (string.IsNullOrEmpty(Version))
            yield break;

        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "General.OtherPluginUserMessage",
                Player.netId,
                Player._nickname,
                string.IsNullOrEmpty(Player._globalNickname) ? Main.Instance.Translate("General.OtherPluginUserMessage.GlobalNickname", Player._globalNickname) : string.Empty,
                Version
            )
        );
    }
    public void Unload()
    {
        if (!Configuration.Instance.General.Plugin_ShowOtherPluginUserMessageOnJoin.Value)
            return;

        Game.Events.Player.DeserializeSyncVars_Postfix.OnInvoke -= DeserializeSyncVars_Postfix_OnInvoke;
    }
    private void OnStartAuthority_Postfix_OnInvoke()
    {
        if (AtlyssNetworkManager._current._soloMode)
            return;

        LobbyID = new CSteamID(SteamLobby._current._currentLobbyID);
        PlayerID = new CSteamID(ulong.Parse(Player._mainPlayer._steamID));

        SteamMatchmaking.SetLobbyMemberData(LobbyID, LobbyMemberDataKey_Version, PluginInfo.Version);
    }
    private void OnStopClient_Prefix_OnInvoke() =>
        LobbyID = CSteamID.Nil;
}
using Steamworks;
using System.Collections;
using UnityEngine;

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

        Game.Main.Instance.Patch(typeof(Game.Events.Player.Awake_Postfix));

        Game.Events.Player.Awake_Postfix.OnInvoke += Awake_Postfix_OnInvoke;
    }
    public void Unload()
    {
        if (!Configuration.Instance.General.Plugin_ShowOtherPluginUserMessageOnJoin.Value)
            return;

        Game.Events.Player.Awake_Postfix.OnInvoke -= Awake_Postfix_OnInvoke;
    }
    private void Awake_Postfix_OnInvoke(Player Player)
    {
        if (!Player._mainPlayer)
            return;

        Main.Instance.StartCoroutine(CheckPlayerPlugin(Player));
    }
    private IEnumerator CheckPlayerPlugin(Player Player)
    {
        while (Player)
        {
            if (Player._currentPlayerCondition != PlayerCondition.CONNECTING)
                break;

            yield return new WaitForSeconds(0.3f);
        }

        if (!Player)
            yield break;

        if (Player.isLocalPlayer)
            yield break;

        if (!ulong.TryParse(Player._steamID, out ulong SteamID))
            yield break;

        string Version = SteamMatchmaking.GetLobbyMemberData(LobbyID, new(SteamID), LobbyMemberDataKey_Version);

        if (string.IsNullOrEmpty(Version))
            yield break;

        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "General.OtherPluginUserMessage",
                Player.netId,
                Player._nickname,
                string.IsNullOrEmpty(Player._globalNickname) ? string.Empty : Main.Instance.Translate("General.OtherPluginUserMessage.GlobalNickname", Player._globalNickname),
                Version
            )
        );

        yield break;
    }
    private void OnStartAuthority_Postfix_OnInvoke()
    {
        if (AtlyssNetworkManager._current._soloMode)
            return;

        LobbyID = new CSteamID(SteamLobby._current._currentLobbyID);
        PlayerID = Player._mainPlayer._isHostPlayer ? SteamMatchmaking.GetLobbyOwner(LobbyID) : new CSteamID(ulong.Parse(Player._mainPlayer._steamID));

        SteamMatchmaking.SetLobbyMemberData(LobbyID, LobbyMemberDataKey_Version, PluginInfo.Version);
    }
    private void OnStopClient_Prefix_OnInvoke() => LobbyID = CSteamID.Nil;
}
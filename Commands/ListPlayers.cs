using Steamworks;
using System.Collections;
using System.Text;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class ListPlayers : ICommand
{
    public bool Execute(string[] Arguments)
    {
        Main.Instance.StartCoroutine(ListPlayersTask());
        return false;
    }

    private IEnumerator ListPlayersTask()
    {
        StringBuilder StringBuilder = new();
        byte Count = 0;

        foreach (Player Player in Game.Managers.Player.Instance.Players.Values)
        {
            ulong.TryParse(Player._steamID, out ulong SteamID);

            Count++;

            string LobbyMemberDataKey_Version =
                Player.isLocalPlayer ?
                    PluginInfo.Version
                    :
                    SteamID == 0 ?
                        string.Empty
                        :
                        SteamMatchmaking.GetLobbyMemberData(Managers.Lobby.Instance.LobbySteamID, new(SteamID), Managers.Lobby.LobbyMemberDataKey_Version);

            StringBuilder.Append(
                Main.Instance.Translate(
                    "Commands.ListPlayers.Entry",
                    Main.Instance.Translate(Player._isHostPlayer ? "Commands.ListPlayers.NetID.Host" : "Commands.ListPlayers.NetID.Client", Player.netIdentity.netId),
                    Player._nickname,
                    Player._nickname != Player._globalNickname && !string.IsNullOrEmpty(Player._globalNickname) ? Main.Instance.Translate("Commands.ListPlayers.GlobalNickname", Player._globalNickname) : string.Empty,
                    SteamID,
                    string.IsNullOrEmpty(Player._mapName) ? Main.Instance.Translate("Commands.ListPlayers.NoMap") : Player._mapName,
                    string.IsNullOrEmpty(LobbyMemberDataKey_Version) ? string.Empty : Main.Instance.Translate("Commands.ListPlayers.FluffUtilities", LobbyMemberDataKey_Version)
                )
            );
        }

        StringBuilder.Insert(0, Main.Instance.Translate("Commands.ListPlayers.Header", Count));
        ChatBehaviour._current.New_ChatMessage(StringBuilder.ToString());

        yield break;
    }
}
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
        Player[] Players = UnityEngine.Object.FindObjectsOfType<Player>();
        StringBuilder StringBuilder = new(Main.Instance.Translate("Commands.ListPlayers.Header", Players.Length));

        foreach (Player Player in Players)
        {
            if (!Player)
                continue;

            string LobbyMemberDataKey_Version = Player.isLocalPlayer ? PluginInfo.Version : SteamMatchmaking.GetLobbyMemberData(Managers.Lobby.Instance.LobbyID, new(ulong.Parse(Player._steamID)), Managers.Lobby.LobbyMemberDataKey_Version);
            StringBuilder.Append(
                Main.Instance.Translate(
                    "Commands.ListPlayers.Entry",
                    Main.Instance.Translate(Player._isHostPlayer ? "Commands.ListPlayers.NetID.Host" : "Commands.ListPlayers.NetID.Client", Player.netIdentity.netId),
                    Player._nickname,
                    Player._nickname != Player._globalNickname && !string.IsNullOrEmpty(Player._globalNickname) ? Main.Instance.Translate("Commands.ListPlayers.GlobalNickname", Player._globalNickname) : string.Empty,
                    Player._steamID,
                    string.IsNullOrEmpty(Player._mapName) ? Main.Instance.Translate("Commands.ListPlayers.NoMap") : Player._mapName,
                    string.IsNullOrEmpty(LobbyMemberDataKey_Version) ? string.Empty : Main.Instance.Translate("Commands.ListPlayers.FluffUtilities", LobbyMemberDataKey_Version)
                )
            );
        }

        ChatBehaviour._current.New_ChatMessage(StringBuilder.ToString());

        yield break;
    }
}
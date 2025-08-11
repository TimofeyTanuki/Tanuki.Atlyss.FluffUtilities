using System.Text;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class ListPlayers : ICommand
{
    public bool Execute(string[] Arguments)
    {
        Player[] Players = UnityEngine.Object.FindObjectsOfType<Player>();
        StringBuilder StringBuilder = new(Main.Instance.Translate("Commands.ListPlayers.Header", Players.Length));

        foreach (Player Player in Players)
            StringBuilder.Append(
                Main.Instance.Translate(
                    "Commands.ListPlayers.Entry",
                    Main.Instance.Translate(Player._isHostPlayer ? "Commands.ListPlayers.NetID.Host" : "Commands.ListPlayers.NetID.Client", Player.netIdentity.netId),
                    Player._nickname,
                    Player._nickname != Player._globalNickname && !string.IsNullOrEmpty(Player._globalNickname) ? Main.Instance.Translate("Commands.ListPlayers.GlobalNickname", Player._globalNickname) : string.Empty,
                    Player._steamID,
                    string.IsNullOrEmpty(Player._mapName) ? Main.Instance.Translate("Commands.ListPlayers.NoMap") : Player._mapName
                )
            );

        ChatBehaviour._current.New_ChatMessage(StringBuilder.ToString());

        return false;
    }
}
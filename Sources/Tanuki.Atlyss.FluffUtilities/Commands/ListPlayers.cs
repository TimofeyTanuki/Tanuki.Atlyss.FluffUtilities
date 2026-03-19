using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Game.Data.Player;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class ListPlayers : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;
    private static readonly Game.Providers.Player playerProvider;
    private static readonly Network.Providers.SteamLobby steamLobbyProvider;

    static ListPlayers()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
        playerProvider = Game.Tanuki.Instance.Providers.Player;
        steamLobbyProvider = Network.Tanuki.Instance.Providers.SteamLobby;
    }

    public void Execute(IContext context)
    {
        List<Entry> playerEntries = [];

        foreach (Entry entry in playerProvider.PlayerEntries.Values)
        {
            if (entry.SteamId == CSteamID.Nil)
                continue;

            playerEntries.Add(entry);
        }

        Main.Instance.StartCoroutine(DisplayPlayers(playerEntries));
    }

    private IEnumerator DisplayPlayers(List<Entry> playerEntries)
    {
        StringBuilder stringBuilder = new();

        foreach (Entry playerEntry in playerEntries)
        {
            Player player = playerEntry.Player;

            string netId = translationSet.Translate(player._isHostPlayer ? "Commands.ListPlayers.NetID.Host" : "Commands.ListPlayers.NetID.Client", player.netIdentity.netId);

            string pluginVersion =
                playerEntry.Player.isLocalPlayer ?
                    PluginInfo.VERSION
                    :
                    SteamMatchmaking.GetLobbyMemberData(steamLobbyProvider.SteamId, playerEntry.SteamId, "cc8615a7-47a4-4321-be79-11e36887b64a.ver"); // NOT IMPLEMENTED

            pluginVersion = string.IsNullOrEmpty(pluginVersion) ? string.Empty : Main.Instance.Translate("Commands.ListPlayers.FluffUtilities", pluginVersion);

            string globalNickname =
                player._nickname != player._globalNickname && !string.IsNullOrEmpty(player._globalNickname) ?
                    Main.Instance.Translate("Commands.ListPlayers.GlobalNickname", player._globalNickname)
                    :
                    string.Empty;

            string mapName = string.IsNullOrEmpty(player._mapName) ? Main.Instance.Translate("Commands.ListPlayers.NoMap") : player._mapName;

            stringBuilder.Append(
                translationSet.Translate(
                    "Commands.ListPlayers.Entry",
                    netId,
                    player._nickname,
                    globalNickname,
                    playerEntry.SteamId.m_SteamID,
                    mapName,
                    pluginVersion
                )
            );
        }

        stringBuilder.Insert(0, Main.Instance.Translate("Commands.ListPlayers.Header", playerEntries.Count));
        chatManager.SendClientMessage(stringBuilder.ToString());

        yield break;
    }
}
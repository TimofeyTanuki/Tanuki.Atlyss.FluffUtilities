using Steamworks;
using System.Collections.Generic;
using Tanuki.Atlyss.FluffUtilities.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Services;

public sealed class ClientHello
{
    private static Network.Routers.Packets packetRouter = null!;

    private readonly Packets.ClientHello clientHelloPacket;

    private readonly Dictionary<Player, Packets.ClientHello> clientHelloData = [];

    public IReadOnlyDictionary<Player, Packets.ClientHello> ClientHelloData => clientHelloData;

    private bool waitForFirstLoad = false;

    public ClientHello()
    {
        Game.Patches.Player.OnStartAuthority.OnPostfix += OnStartAuthority;
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;

        Game.Tanuki.OnInitialized += OnGameInitialized;
        Network.Tanuki.OnInitialized += OnNetworkInitialized;

        clientHelloPacket = new()
        {
            Version = PluginInfo.VERSION
        };
    }

    private void SetWaitForFirstLoadState(bool state)
    {
        if (waitForFirstLoad == state)
            return;

        if (waitForFirstLoad)
            Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix += OnFirstLoad;
        else
            Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix -= OnFirstLoad;

        waitForFirstLoad = state;
    }

    private void OnFirstLoad()
    {
        SetWaitForFirstLoadState(false);

        packetRouter.SendPacketToLobbyChat(clientHelloPacket);
    }

    private void OnGameInitialized()
    {
        Game.Tanuki.Instance.Providers.Player.OnPlayerLoaded += SendClientHelloToLoadedPlayer;
        Game.Tanuki.Instance.Providers.Player.OnPlayerRemoved += RemoveClientHello;
    }

    private void RemoveClientHello(Player player) =>
        clientHelloData.Remove(player);

    private void SendClientHelloToLoadedPlayer(Player player)
    {
        if (player.isLocalPlayer)
            return;

        if (!ulong.TryParse(player._steamID, out ulong steamId))
            return;

        packetRouter.SendPacketToUser(new(steamId), clientHelloPacket, out EResult _);
    }

    private void OnNetworkInitialized()
    {
        Network.Tanuki network = Network.Tanuki.Instance;
        packetRouter = network.Routers.Packet;

        network.Registers.Packets.Register<Packets.ClientHello>();
        network.Managers.Packets.AddHandler<Packets.ClientHello>(HandleClientInfoPacket);
    }

    private void HandleClientInfoPacket(CSteamID sender, Packets.ClientHello clientHello)
    {
        Player? player = Game.Tanuki.Instance.Providers.Player.FindBySteamId(sender);

        if (player is null)
            return;

        if (player.isLocalPlayer)
            return;

        clientHelloData[player] = clientHello;

        if (waitForFirstLoad)
            return;

        Chat.AddTranslatedMessage(
            "Services.ClientHello.JoinMessage",
            player.netId,
            player._nickname,
            string.IsNullOrEmpty(player._globalNickname) ?
                string.Empty : Main.Translate("Services.ClientHello.JoinMessage.GlobalNickname", player._globalNickname),
            clientHello.Version
        );
    }

    private void OnStartAuthority(Player player)
    {
        if (AtlyssNetworkManager._current._soloMode)
            return;

        Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix += OnFirstLoad;
    }

    private void OnStopClient()
    {
        Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix -= OnFirstLoad;

        SetWaitForFirstLoadState(true);
        clientHelloData.Clear();
    }
}

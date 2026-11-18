using Steamworks;
using System;
using System.Collections.Generic;
using Tanuki.Atlyss.FluffUtilities.Helpers;
using Tanuki.Atlyss.FluffUtilities.Extensions;

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
            Version = PluginInfo.VERSION,
            Random = $"{UnityEngine.Random.Range(0, int.MaxValue)}"
        };

        Console.WriteLine($"My clientHelloPacket:\nver {clientHelloPacket.Version}\nrand {clientHelloPacket.Random}");
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

        Console.WriteLine("OnLoadScreenDisable");
    }

    private void OnGameInitialized()
    {
        Console.WriteLine($"OnGameInitialized");

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

        packetRouter.SendPacketToUser(new CSteamID(steamId), clientHelloPacket, out EResult _);
    }

    private void OnNetworkInitialized()
    {
        Network.Tanuki network = Network.Tanuki.Instance;
        packetRouter = network.Routers.Packet;

        network.Registers.Packets.Register<Packets.ClientHello>();
        network.Managers.Packets.AddHandler<Packets.ClientHello>(HandleClientInfoPacket);
    }

    private void HandleClientInfoPacket(CSteamID sender, Packets.ClientHello clientInfo)
    {
        if (!Network.Tanuki.Instance.Providers.SteamLobby.OwnerSteamId.Equals(sender))
            return;

        Player? player = Game.Tanuki.Instance.Providers.Player.FindBySteamId(sender);

        if (player is null || !player)
            return;

        clientHelloData[player] = clientInfo;

        if (waitForFirstLoad)
            return;

        Console.WriteLine($"PID HandleClientInfoPacket: {sender} - {clientInfo.Version} - {clientInfo.Random}");

        Chat.AddTranslatedMessage(
            "Services.ClientHello.JoinMessage",
            player.netId,
            player._nickname,
            string.IsNullOrEmpty(player._globalNickname) ?
                string.Empty : Main.Translate("Services.ClientHello.JoinMessage.GlobalNickname", player._globalNickname),
            clientInfo.Version
        );
    }

    private void OnStartAuthority(Player player)
    {
        if (!AtlyssNetworkManager._current._soloMode)
            return;

        Network.Tanuki.Instance.Routers.Packet.SendPacketToLobbyChat(clientHelloPacket);
        Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix += OnFirstLoad;
    }

    private void OnStopClient()
    {
        Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix -= OnFirstLoad;
        Console.WriteLine("PID OnStopClient");

        SetWaitForFirstLoadState(true);

        clientHelloData.Clear();
    }
}

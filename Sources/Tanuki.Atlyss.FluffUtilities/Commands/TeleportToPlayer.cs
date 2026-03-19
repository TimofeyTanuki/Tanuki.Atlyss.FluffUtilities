using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Game.Extensions;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class TeleportToPlayer : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;
    private static readonly Game.Providers.Player playerProvider;

    private static bool teleportBetweenScenes = false;
    private static Vector3 targetPosition;

    static TeleportToPlayer()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
        playerProvider = Game.Tanuki.Instance.Providers.Player;

        Game.Patches.LoadSceneManager.Init_LoadScreenDisable.OnPostfix += HandleTeleportBetweenScenes;
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPostfix += OnAtlyssNetworkManagerStopClient;
    }

    private static void OnAtlyssNetworkManagerStopClient() =>
        teleportBetweenScenes = false;

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;

        if (arguments.Count == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.TeleportToPlayer.NicknameNotSpecified"));
            return;
        }

        Player? targetPlayer = playerProvider.FindByFlexibleInput(string.Join(" ", arguments));
        if (targetPlayer is null)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.TeleportToPlayer.PlayerNotFound"));
            return;
        }

        Player player = Player._mainPlayer;
        if (player == targetPlayer)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.TeleportToPlayer.Self"));
            return;
        }

        targetPosition = targetPlayer.transform.position;

        if (player._mapName == targetPlayer._mapName)
        {
            TeleportToTargetPosition();
            return;
        }

        if (string.IsNullOrEmpty(targetPlayer._mapName))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.TeleportToPlayer.EmptyMapName", targetPlayer._mapName));
            return;
        }

        foreach (KeyValuePair<string, ScriptableMapData> scriptableMapData in Game.Accessors.GameManager._cachedScriptableMapDatas(GameManager._current))
        {
            if (scriptableMapData.Value._mapCaptionTitle != targetPlayer._mapName)
                continue;

            if (!player._waypointAttunements.Contains(scriptableMapData.Key))
                break;

            //Managers.FreeCamera.Instance.Disable();

            teleportBetweenScenes = true;
            player.Cmd_SceneTransport(scriptableMapData.Value._subScene, scriptableMapData.Value._spawnPointTag, ZoneDifficulty.NORMAL);

            return;
        }

        chatManager.SendClientMessage(translationSet.Translate("Commands.TeleportToPlayer.SubSceneNotFound", targetPlayer._mapName));
    }

    private static void HandleTeleportBetweenScenes()
    {
        if (!teleportBetweenScenes)
            return;

        teleportBetweenScenes = false;

        if (!Player._mainPlayer)
            return;

        TeleportToTargetPosition();
    }

    private static void TeleportToTargetPosition()
    {
        Player player = Player._mainPlayer;

        player._pMove.Teleport(targetPosition);
        player._pVisual.Cmd_PlayTeleportEffect();
    }
}

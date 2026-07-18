using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;
using Tanuki.Atlyss.Game.Extensions;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class TeleportToPlayer : ICommand
{
    private static bool teleportBetweenScenes = false;
    private static Vector3 targetPosition;

    static TeleportToPlayer()
    {
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
            Chat.AddTranslatedMessage("Commands.TeleportToPlayer.NicknameNotSpecified");
            return;
        }

        Player? targetPlayer = Game.Tanuki.Instance.Providers.Player.FindByFlexibleInput(string.Join(" ", arguments));
        if (targetPlayer is null)
        {
            Chat.AddTranslatedMessage("Commands.TeleportToPlayer.PlayerNotFound");
            return;
        }

        Player player = Player._mainPlayer;
        if (player == targetPlayer)
        {
            Chat.AddTranslatedMessage("Commands.TeleportToPlayer.Self");
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
            Chat.AddTranslatedMessage("Commands.TeleportToPlayer.EmptyMapName", targetPlayer._mapName);
            return;
        }

        foreach (KeyValuePair<string, ScriptableMapData> scriptableMapData in Game.Accessors.GameManager._cachedScriptableMapDatas(GameManager._current))
        {
            if (scriptableMapData.Value._mapCaptionTitle != targetPlayer._mapName)
                continue;

            if (!player._waypointAttunements.Contains(scriptableMapData.Key))
                break;

            Main.Instance.Managers.FreeCamera.SetState(false);

            teleportBetweenScenes = true;
            player.Cmd_SceneTransport(scriptableMapData.Value._subScene, scriptableMapData.Value._spawnPointTag, ZoneDifficulty.NORMAL);

            return;
        }

        Chat.AddTranslatedMessage("Commands.TeleportToPlayer.SubSceneNotFound", targetPlayer._mapName);
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

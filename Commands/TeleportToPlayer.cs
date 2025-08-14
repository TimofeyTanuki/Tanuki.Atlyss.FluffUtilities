using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class TeleportToPlayer : ICommand, IDisposable
{
    private Vector3 Target;
    private bool TeleportingBetweenScenes = false;
    public TeleportToPlayer() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += ResetTeleportationBetweenScenes;

    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TeleportToPlayer.NicknameNotSpecified"));
            return false;
        }

        string Nickname = string.Join(" ", Arguments).ToLower();

        foreach (Player Player in UnityEngine.Object.FindObjectsOfType<Player>())
        {
            if (Player.isLocalPlayer)
                continue;

            if (!Player._nickname.ToLower().Contains(Nickname))
                continue;

            Target = Player.transform.position;

            if (Player._mainPlayer._mapName != Player._mapName)
            {
                if (string.IsNullOrEmpty(Player._mapName))
                {
                    ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TeleportToPlayer.EmptyMapName", Player._mapName));
                    return false;
                }

                foreach (KeyValuePair<string, ScriptableMapData> ScriptableMapData in Game.Fields.GameManager.Instance.CachedScriptableMapDatas)
                {
                    if (ScriptableMapData.Value._mapCaptionTitle != Player._mapName)
                        continue;

                    if (!Player._mainPlayer._waypointAttunements.Contains(ScriptableMapData.Key))
                        break;

                    Managers.FreeCamera.Instance.Disable();

                    Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke += Teleport;
                    TeleportingBetweenScenes = true;
                    Player._mainPlayer.Cmd_SceneTransport(ScriptableMapData.Value._subScene, ScriptableMapData.Value._spawnPointTag, ZoneDifficulty.NORMAL);
                    return false;
                }

                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TeleportToPlayer.SubSceneNotFound", Player._mapName));
            }
            else
                Teleport();

            return false;
        }

        ChatBehaviour._current.New_ChatMessage(Main.Instance.Translation.Translate("Commands.TeleportToPlayer.PlayerNotFound"));
        return false;
    }

    private void ResetTeleportationBetweenScenes()
    {
        if (!TeleportingBetweenScenes)
            return;

        TeleportingBetweenScenes = false;
        Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke -= Teleport;
    }
    private void Teleport()
    {
        if (TeleportingBetweenScenes)
        {
            TeleportingBetweenScenes = false;
            Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke -= Teleport;
        }

        Player._mainPlayer._pMove.Teleport(Target);
        Player._mainPlayer._pVisual.Cmd_PlayTeleportEffect();
    }
    public void Dispose()
    {
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= ResetTeleportationBetweenScenes;
        ResetTeleportationBetweenScenes();
    }
}
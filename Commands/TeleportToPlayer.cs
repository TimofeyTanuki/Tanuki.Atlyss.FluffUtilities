using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class TeleportToPlayer : ICommand
{
    private Vector3 LastPosition;
    public void Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TeleportToPlayer.NicknameNotSpecified"));
            return;
        }

        string Nickname = string.Join(" ", Arguments).ToLower();

        foreach (Player Player in Object.FindObjectsOfType<Player>())
        {
            if (Player.netIdentity.isLocalPlayer)
                continue;

            if (!Player._nickname.ToLower().Contains(Nickname))
                continue;

            LastPosition = Player.transform.position;
            if (Player._mainPlayer._mapName != Player._mapName)
            {
                if (string.IsNullOrEmpty(Player._mapName))
                {
                    ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TeleportToPlayer.EmptyMapName", Player._mapName));
                    return;
                }

                foreach (Portal Portal in Object.FindObjectsOfType<Portal>())
                {
                    if (Portal._scenePortal._portalCaptionTitle != Player._mapName)
                        continue;

                    Managers.FreeCamera.Instance.Disable();

                    Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke += Init_LoadScreenDisable_After;
                    Player._mainPlayer.Cmd_SceneTransport(Portal._scenePortal._subScene, Portal._scenePortal._spawnPointTag, ZoneDifficulty.NORMAL);
                    return;
                }

                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TeleportToPlayer.SubSceneNotFound", Player._mapName));
                return;
            }

            Player._mainPlayer._pVisual.Cmd_PlayTeleportEffect();
            Player._mainPlayer._pMove.Teleport(LastPosition);
            return;
        }

        ChatBehaviour._current.New_ChatMessage(Main.Instance.Translation.Translate("Commands.TeleportToPlayer.PlayerNotFound"));
    }

    private void Init_LoadScreenDisable_After()
    {
        Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke -= Init_LoadScreenDisable_After;
        Player._mainPlayer._pMove.Teleport(LastPosition);
    }
}
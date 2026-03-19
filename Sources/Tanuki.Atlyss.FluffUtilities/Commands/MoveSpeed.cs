using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class MoveSpeed : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    private static bool state;

    static MoveSpeed()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;

        state = false;

        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnAtlyssNetworkManagerStopClient;
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;
        PlayerMove playerMove = player._pMove;

        if (arguments.Count == 0)
        {
            if (state)
            {
                playerMove._movSpeed = GameManager._current._statLogics._baseMoveSpeed;

                chatManager.SendClientMessage(translationSet.Translate("Commands.MoveSpeed.Reset"));
                player._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
            }
            else
                chatManager.SendClientMessage(translationSet.Translate("Commands.MoveSpeed.SpeedNotSpecified"));

            return;
        }

        if (!float.TryParse(arguments[0], out float speed))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.MoveSpeed.SpeedNotFloat"));
            return;
        }

        if (speed < 0)
            speed = -speed;

        state = true;
        playerMove._movSpeed = speed;
        chatManager.SendClientMessage(translationSet.Translate("Commands.MoveSpeed.Enabled", speed));
        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockoutSound);
    }

    private static void OnAtlyssNetworkManagerStopClient() => state = false;
}
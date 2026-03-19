using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Heal : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static Heal()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (!player._isHostPlayer)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Heal.NotHost"));
            return;
        }

        if (arguments.Count == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Heal.DeltaNotSpecified"));
            return;
        }

        if (!int.TryParse(arguments[0], out int Delta))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Heal.DeltaNotInteger"));
            return;
        }

        if (Delta < 0)
            player._statusEntity.Subtract_Health(-Delta);
        else
            player._statusEntity.Add_Health(Delta);

        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
    }
}
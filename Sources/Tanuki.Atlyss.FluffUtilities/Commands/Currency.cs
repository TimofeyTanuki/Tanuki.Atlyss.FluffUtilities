using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Currency : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static Currency()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        PlayerInventory playerInventory = Player._mainPlayer._pInventory;

        if (arguments.Count == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Currency.DeltaNotSpecified"));
            return;
        }

        if (!int.TryParse(arguments[0], out int delta))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Currency.DeltaNotInteger"));
            return;
        }

        if (delta < 0)
            playerInventory.Cmd_SubtractCurrency(-delta);
        else
            playerInventory.Cmd_AddCurrency(delta);
    }
}

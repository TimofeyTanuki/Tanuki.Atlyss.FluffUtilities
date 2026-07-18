using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Currency : ICommand
{
    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        PlayerInventory playerInventory = Player._mainPlayer._pInventory;

        if (arguments.Count == 0)
        {
            Chat.AddTranslatedMessage("Commands.Currency.DeltaNotSpecified");
            return;
        }

        if (!int.TryParse(arguments[0], out int delta))
        {
            Chat.AddTranslatedMessage("Commands.Currency.DeltaNotInteger");
            return;
        }

        if (delta < 0)
            playerInventory.Cmd_SubtractCurrency(-delta);
        else
            playerInventory.Cmd_AddCurrency(delta);
    }
}

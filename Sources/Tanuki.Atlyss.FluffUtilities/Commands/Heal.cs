using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Heal : ICommand
{
    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (!player._isHostPlayer)
        {
            Chat.AddTranslatedMessage("Commands.Heal.NotHost");
            return;
        }

        if (arguments.Count == 0)
        {
            Chat.AddTranslatedMessage("Commands.Heal.DeltaNotSpecified");
            return;
        }

        if (!int.TryParse(arguments[0], out int Delta))
        {
            Chat.AddTranslatedMessage("Commands.Heal.DeltaNotInteger");
            return;
        }

        if (Delta < 0)
            player._statusEntity.Subtract_Health(-Delta);
        else
            player._statusEntity.Add_Health(Delta);

        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
    }
}
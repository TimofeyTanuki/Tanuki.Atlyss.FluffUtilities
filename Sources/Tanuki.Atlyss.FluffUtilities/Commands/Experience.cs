using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Experience : ICommand
{
    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (!player._isHostPlayer)
        {
            Chat.AddTranslatedMessage("Commands.Experience.NotHost");
            return;
        }

        if (arguments.Count == 0)
        {
            Chat.AddTranslatedMessage("Commands.Experience.DeltaNotSpecified");
            return;
        }

        if (!int.TryParse(arguments[0], out int Delta))
        {
            Chat.AddTranslatedMessage("Commands.Experience.DeltaNotInteger");
            return;
        }

        PlayerStats playerStats = player._pStats;

        if (Delta < 0)
        {
            Delta = -Delta;

            int NewLevel = playerStats._currentLevel;
            int NewExperience = playerStats._currentExp;

            while (Delta > NewExperience && NewLevel != 1)
            {
                NewLevel--;
                Delta -= NewExperience;
                NewExperience = (int)GameManager._current._statLogics._experienceCurve.Evaluate(NewLevel);
            }

            NewExperience -= Delta;

            if (NewExperience < 0)
                NewExperience = 0;

            if (playerStats._currentLevel != NewLevel)
            {
                playerStats.Network_currentLevel = NewLevel;
                playerStats.Apply_StatCalculations();
            }

            playerStats.Network_currentExp = NewExperience;
        }
        else
            playerStats.Network_currentExp += Delta;

        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
    }
}
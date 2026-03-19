using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Experience : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static Experience()
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
            chatManager.SendClientMessage(translationSet.Translate("Commands.Experience.NotHost"));
            return;
        }

        if (arguments.Count == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Experience.DeltaNotSpecified"));
            return;
        }

        if (!int.TryParse(arguments[0], out int Delta))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Experience.DeltaNotInteger"));
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
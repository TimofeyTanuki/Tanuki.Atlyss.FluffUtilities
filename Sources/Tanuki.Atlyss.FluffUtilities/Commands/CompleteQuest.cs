using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class CompleteQuest : ICommand
{
    public void Execute(IContext context)
    {
        Player player = Player._mainPlayer;
        string questName = string.Join(" ", context.Arguments);

        if (questName.Length == 0)
        {
            Chat.AddTranslatedMessage("Commands.CompleteQuest.QuestNotSpecified");
            return;
        }

        PlayerQuesting playerQuesting = player._pQuest;
        List<QuestProgressStruct> questProgressStructs = playerQuesting._questProgressData;

        if (questProgressStructs.Count == 0)
        {
            Chat.AddTranslatedMessage("Commands.CompleteQuest.NoQuests");
            return;
        }

        if (questName == "*")
        {
            for (int index = questProgressStructs.Count - 1; index >= 0; index--)
            {
                questProgressStructs[index]._questComplete = true;
                playerQuesting.Client_CompleteQuest(index);
            }

            return;
        }

        for (int index = 0; index < questProgressStructs.Count; index++)
        {
            QuestProgressStruct questProgressStruct = questProgressStructs[index];

            if (questProgressStruct._questTag.IndexOf(questName, StringComparison.OrdinalIgnoreCase) < 0)
                continue;

            questProgressStruct._questComplete = true;
            playerQuesting.Client_CompleteQuest(index);

            return;
        }

        Chat.AddTranslatedMessage("Commands.CompleteQuest.QuestNotFound", questName);
    }
}

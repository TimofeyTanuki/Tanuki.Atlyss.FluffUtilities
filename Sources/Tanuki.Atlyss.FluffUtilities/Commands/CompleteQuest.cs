using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class CompleteQuest : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static CompleteQuest()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
    }

    public void Execute(IContext context)
    {
        Player player = Player._mainPlayer;
        string questName = string.Join(" ", context.Arguments);

        if (questName.Length == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.CompleteQuest.QuestNotSpecified"));
            return;
        }

        PlayerQuesting playerQuesting = player._pQuest;
        List<QuestProgressStruct> questProgressStructs = playerQuesting._questProgressData;

        if (questProgressStructs.Count == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.CompleteQuest.NoQuests"));
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

        chatManager.SendClientMessage(translationSet.Translate("Commands.CompleteQuest.QuestNotFound", questName));
    }
}

using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Core.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class GetQuest : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static GetQuest()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
    }

    public void Execute(IContext context)
    {
        string questName = string.Join(" ", context.Arguments);

        if (questName.Length == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.GetQuest.QuestNotSpecified"));
            return;
        }

        Dictionary<string, ScriptableQuest> cachedScriptableQuests = Game.Accessors.GameManager._cachedScriptableQuests(GameManager._current);

        if (!cachedScriptableQuests.TryGetValueFlexible(questName, out ScriptableQuest scriptableQuest, false, StringComparison.OrdinalIgnoreCase))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.GetQuest.QuestNotFound", questName));
            return;
        }

        PlayerQuesting playerQuesting = Player._mainPlayer._pQuest;

        if (playerQuesting.Check_HasQuest(scriptableQuest))
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.GetQuest.QuestAlreadyActive", scriptableQuest._questName));
            return;
        }

        if (playerQuesting._questProgressData.Count >= playerQuesting._questLogLimit)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.GetQuest.ActiveQuestsLimit"));
            return;
        }

        playerQuesting.Accept_Quest(scriptableQuest);
    }
}
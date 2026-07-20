using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Core.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class GetQuest : ICommand
{
    private IReadOnlyDictionary<string, ScriptableQuest> CachedScriptableQuests => Game.Accessors.GameManager._cachedScriptableQuests(GameManager._current);

    public void Execute(IContext context)
    {
        string questName = string.Join(" ", context.Arguments);

        if (questName.Length == 0)
        {
            Chat.AddTranslatedMessage("Commands.GetQuest.QuestNotSpecified");
            return;
        }

        if (!CachedScriptableQuests.TryGetValueFlexible(questName, out ScriptableQuest scriptableQuest, false, StringComparison.OrdinalIgnoreCase))
        {
            Chat.AddTranslatedMessage("Commands.GetQuest.QuestNotFound", questName);
            return;
        }

        PlayerQuesting playerQuesting = Player._mainPlayer._pQuest;

        if (playerQuesting.Check_HasQuest(scriptableQuest))
        {
            Chat.AddTranslatedMessage("Commands.GetQuest.QuestAlreadyActive", scriptableQuest._questName);
            return;
        }

        if (playerQuesting._questProgressData.Count >= playerQuesting._questLogLimit)
        {
            Chat.AddTranslatedMessage("Commands.GetQuest.ActiveQuestsLimit");
            return;
        }

        playerQuesting.Accept_Quest(scriptableQuest);
    }
}
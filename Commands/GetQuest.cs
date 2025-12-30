using System.Collections.Generic;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class GetQuest : ICommand
{
    public bool Execute(string[] Arguments)
    {
        string QuestName = string.Join(" ", Arguments);

        if (QuestName.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.GetQuest.QuestNotSpecified"));
            return false;
        }

        ScriptableQuest ScriptableQuest = GameManager._current.Locate_Quest(QuestName);

        if (!ScriptableQuest)
        {
            foreach (KeyValuePair<string, ScriptableQuest> CachedScriptableQuest in Game.Fields.GameManager.CachedScriptableQuests)
            {
                if (CachedScriptableQuest.Key.IndexOf(QuestName, System.StringComparison.InvariantCultureIgnoreCase) < 0)
                    continue;

                ScriptableQuest = CachedScriptableQuest.Value;

                break;
            }
        }

        if (!ScriptableQuest)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.GetQuest.QuestNotFound", QuestName));
            return false;
        }

        if (Player._mainPlayer._pQuest.Check_HasQuest(ScriptableQuest))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.GetQuest.QuestAlreadyActive", ScriptableQuest._questName));
            return false;
        }

        if (Player._mainPlayer._pQuest._questProgressData.Count >= Player._mainPlayer._pQuest._questLogLimit)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.GetQuest.ActiveQuestsLimit"));
            return false;
        }

        Player._mainPlayer._pQuest.Accept_Quest(ScriptableQuest);

        return false;
    }
}
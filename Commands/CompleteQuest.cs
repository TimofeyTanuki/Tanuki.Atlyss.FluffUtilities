using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class CompleteQuest : ICommand
{
    public bool Execute(string[] Arguments)
    {
        string QuestName = string.Join(" ", Arguments);

        if (QuestName.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.CompleteQuest.QuestNotSpecified"));
            return false;
        }

        if (Player._mainPlayer._pQuest._questProgressData.Count == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.CompleteQuest.NoQuests"));
            return false;
        }

        if (QuestName == "*")
        {
            for (int i = Player._mainPlayer._pQuest._questProgressData.Count - 1; i >= 0; i--)
            {
                Player._mainPlayer._pQuest._questProgressData[i]._questComplete = true;
                Player._mainPlayer._pQuest.Client_CompleteQuest(i);
            }

            return false;
        }

        QuestProgressStruct QuestProgressStruct;
        for (int i = 0; i < Player._mainPlayer._pQuest._questProgressData.Count; i++)
        {
            QuestProgressStruct = Player._mainPlayer._pQuest._questProgressData[i];

            if (QuestProgressStruct._questTag.IndexOf(QuestName, StringComparison.InvariantCultureIgnoreCase) < 0)
                continue;

            QuestProgressStruct._questComplete = true;
            Player._mainPlayer._pQuest.Client_CompleteQuest(i);
            return false;
        }

        ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.CompleteQuest.QuestNotFound", QuestName));
        return false;
    }
}
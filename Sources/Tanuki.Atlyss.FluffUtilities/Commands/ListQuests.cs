using System;
using System.Collections.Generic;
using System.Linq;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class ListQuests : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    private static readonly IReadOnlyDictionary<string, ScriptableQuest> cachedScriptableItems;

    static ListQuests()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;

        cachedScriptableItems = Game.Accessors.GameManager._cachedScriptableQuests(GameManager._current);
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;

        if (arguments.Count > 0)
            DisplaySearch(string.Join(" ", arguments));
        else
            DisplayAll();
    }

    private void DisplayAll() =>
        chatManager.SendClientMessage(
            translationSet.Translate(
                "Commands.ListQuests.All",
                string.Join(
                    translationSet.Translate("Commands.ListQuests.Separator"),
                    cachedScriptableItems.Keys
                        .OrderBy(x => x)
                        .Select(x => x)
                )
            )
        );

    private void DisplaySearch(string match)
    {
        List<string> matches =
            cachedScriptableItems.GetHighlightedKeys(
                match,
                translationSet.Translate("Commands.ListQuests.Search.Match"),
                StringComparison.OrdinalIgnoreCase
            );

        chatManager.SendClientMessage(
            matches.Count > 0 ?
                translationSet.Translate(
                    "Commands.ListQuests.Search",
                    match,
                    string.Join(
                        translationSet.Translate("Commands.ListQuests.Separator"),
                        matches
                    )
                )
                :
                translationSet.Translate("Commands.ListQuests.Search.QuestsNotFound", match)
        );
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class ListItems : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    private static readonly IReadOnlyDictionary<string, ScriptableItem> cachedScriptableItems;

    static ListItems()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;

        cachedScriptableItems = Game.Accessors.GameManager._cachedScriptableItems(GameManager._current);
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
                "Commands.ListItems.All",
                string.Join(
                    translationSet.Translate("Commands.ListItems.Separator"),
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
                translationSet.Translate("Commands.ListItems.Search.Match"),
                StringComparison.OrdinalIgnoreCase
            );

        chatManager.SendClientMessage(
            matches.Count > 0 ?
                translationSet.Translate(
                    "Commands.ListItems.Search",
                    match,
                    string.Join(
                        translationSet.Translate("Commands.ListItems.Separator"),
                        matches
                    )
                )
                :
                translationSet.Translate("Commands.ListItems.Search.ItemsNotFound", match)
        );
    }
}
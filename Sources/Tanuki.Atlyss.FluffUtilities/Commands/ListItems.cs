using System;
using System.Collections.Generic;
using System.Linq;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class ListItems : ICommand
{
    private IReadOnlyDictionary<string, ScriptableItem> CachedScriptableItems => Game.Accessors.GameManager._cachedScriptableItems(GameManager._current);

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;

        if (arguments.Count > 0)
            DisplaySearch(string.Join(" ", arguments));
        else
            DisplayAll();
    }

    private void DisplayAll() =>
        Chat.AddTranslatedMessage(
            "Commands.ListItems.All",
            string.Join(
                Main.Translate("Commands.ListItems.Separator"),
                CachedScriptableItems.Keys
                    .OrderBy(x => x)
                    .Select(x => x)
            )
        );

    private void DisplaySearch(string match)
    {
        List<string> matches =
            CachedScriptableItems.GetHighlightedKeys(
                match,
                Main.Translate("Commands.ListItems.Search.Match"),
                StringComparison.OrdinalIgnoreCase
            );

        if (matches.Count > 0)
            Chat.AddTranslatedMessage(
                "Commands.ListItems.Search",
                match,
                string.Join(
                    Main.Translate("Commands.ListItems.Separator"),
                    matches
                )
            );
        else
            Chat.AddTranslatedMessage("Commands.ListItems.Search.ItemsNotFound", match);
    }
}
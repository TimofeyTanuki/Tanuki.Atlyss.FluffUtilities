using System;
using System.Collections.Generic;
using System.Linq;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class ListQuests : ICommand
{
    private IReadOnlyDictionary<string, ScriptableQuest> CachedScriptableItems => Game.Accessors.GameManager._cachedScriptableQuests(GameManager._current);

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
            "Commands.ListQuests.All",
            string.Join(
                Main.Translate("Commands.ListQuests.Separator"),
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
                Main.Translate("Commands.ListQuests.Search.Match"),
                StringComparison.OrdinalIgnoreCase
            );

        Chat.AddTranslatedMessage(
            matches.Count > 0 ?
                Main.Translate(
                    "Commands.ListQuests.Search",
                    match,
                    string.Join(
                        Main.Translate("Commands.ListQuests.Separator"),
                        matches
                    )
                )
                :
                Main.Translate("Commands.ListQuests.Search.QuestsNotFound", match)
        );
    }
}
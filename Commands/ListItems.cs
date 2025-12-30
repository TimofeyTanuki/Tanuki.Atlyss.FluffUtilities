using System;
using System.Collections.Generic;
using System.Linq;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class ListItems : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length > 0)
            DisplaySearch(string.Join(" ", Arguments));
        else
            DisplayAll();

        return false;
    }

    private void DisplayAll() =>
        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "Commands.ListItems.All",
                string.Join(
                    Main.Instance.Translate("Commands.ListItems.Separator"),
                    Game.Fields.GameManager.CachedScriptableItems.Keys
                        .OrderBy(x => x)
                        .Select(x => x)
                )
            )
        );

    private void DisplaySearch(string Search)
    {
        List<string> Matches =
            Helpers.Commands.FormatDictionaryKeyMatches(
                Game.Fields.GameManager.CachedScriptableItems,
                Search,
                Main.Instance.Translate("Commands.ListItems.Search.Match"),
                StringComparison.InvariantCultureIgnoreCase
            );

        ChatBehaviour._current.New_ChatMessage(
            Matches.Count > 0 ?
                Main.Instance.Translate(
                    "Commands.ListItems.Search",
                    Search,
                    string.Join(
                        Main.Instance.Translate("Commands.ListItems.Separator"),
                        Matches
                    )
                )
                :
                Main.Instance.Translate("Commands.ListItems.Search.ItemsNotFound", Search)
        );
    }
}
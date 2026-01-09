using System;
using System.Collections.Generic;
using System.Linq;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class ListQuests : ICommand
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
                "Commands.ListQuests.All",
                string.Join(
                    Main.Instance.Translate("Commands.ListQuests.Separator"),
                    Game.Accessors.GameManager._cachedScriptableQuests(GameManager._current).Keys
                        .OrderBy(x => x)
                        .Select(x => x)
                )
            )
        );

    private void DisplaySearch(string Search)
    {
        List<string> Matches =
            Helpers.Commands.FormatDictionaryKeyMatches(
                Game.Accessors.GameManager._cachedScriptableQuests(GameManager._current),
                Search,
                Main.Instance.Translate("Commands.ListQuests.Search.Match"),
                StringComparison.InvariantCultureIgnoreCase
            );

        ChatBehaviour._current.New_ChatMessage(
            Matches.Count > 0 ?
                Main.Instance.Translate(
                    "Commands.ListQuests.Search",
                    Search,
                    string.Join(
                        Main.Instance.Translate("Commands.ListQuests.Separator"),
                        Matches
                    )
                )
                :
                Main.Instance.Translate("Commands.ListQuests.Search.QuestsNotFound", Search)
        );
    }
}
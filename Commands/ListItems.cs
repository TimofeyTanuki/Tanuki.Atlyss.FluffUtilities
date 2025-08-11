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
                    Game.Fields.GameManager.Instance.CachedScriptableItems.Values
                        .OrderBy(x => x._itemName)
                        .Select(x => x._itemName)
                )
            )
        );

    private void DisplaySearch(string Search)
    {
        SortedSet<string> Matches = [];

        string Match;
        foreach (ScriptableItem ScriptableItem in Game.Fields.GameManager.Instance.CachedScriptableItems.Values)
        {
            Match = ScriptableItem._itemName;
            if (!Match.Contains(Search))
                continue;

            Matches.Add(Match.Replace(Search, Main.Instance.Translate("Commands.ListItems.Search.Match", Search)));
        }

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
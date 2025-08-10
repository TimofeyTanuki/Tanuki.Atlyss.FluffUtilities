using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Currency : ICommand
{
    public void Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Currency.DeltaNotSpecified"));
            return;
        }

        if (!int.TryParse(Arguments[0], out int Delta))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Currency.DeltaNotInteger"));
            return;
        }

        if (Delta < 0)
            Player._mainPlayer._pInventory.Cmd_SubtractCurrency(-Delta);
        else
            Player._mainPlayer._pInventory.Cmd_AddCurrency(Delta);
    }
}
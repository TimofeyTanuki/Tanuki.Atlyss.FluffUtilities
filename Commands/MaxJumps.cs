using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class MaxJumps : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.MaxJumps.CountNotSpecified"));
            return false;
        }

        if (!int.TryParse(Arguments[0], out int Value))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.MaxJumps.CountNotInteger"));
            return false;
        }

        if (Value < 0)
            Value = -Value;

        Player._mainPlayer._pMove._maxJumps = Value;
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);

        return false;
    }
}
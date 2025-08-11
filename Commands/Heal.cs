using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Heal : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (!Player._mainPlayer._isHostPlayer)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Heal.NotHost"));
            return false;
        }

        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Heal.DeltaNotSpecified"));
            return false;
        }

        if (!int.TryParse(Arguments[0], out int Delta))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Heal.DeltaNotInteger"));
            return false;
        }

        if (Delta < 0)
            Player._mainPlayer._statusEntity.Subtract_Health(-Delta);
        else
            Player._mainPlayer._statusEntity.Add_Health(Delta);

        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);

        return false;
    }
}
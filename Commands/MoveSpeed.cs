using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class MoveSpeed : ICommand
{
    internal static bool Changed = false;

    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            if (Changed)
            {
                Changed = false;
                Player._mainPlayer._pMove._movSpeed = GameManager._current._statLogics._baseMoveSpeed;
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.MoveSpeed.Reset"));
                Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
            }
            else
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.MoveSpeed.SpeedNotSpecified"));

            return false;
        }

        if (!float.TryParse(Arguments[0], out float Speed))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.MoveSpeed.SpeedNotFloat"));
            return false;
        }

        if (Speed < 0)
            Speed = -Speed;

        Changed = true;
        Player._mainPlayer._pMove._movSpeed = Speed;
        ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.MoveSpeed.Enabled", Speed));
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);

        return false;
    }
}
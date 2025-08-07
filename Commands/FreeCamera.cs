using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class FreeCamera : ICommand
{
    public void Execute(string[] Arguments)
    {
        if (Managers.FreeCamera.Instance.Status)
        {
            Managers.FreeCamera.Instance.Disable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
        }
        else
        {
            Managers.FreeCamera.Instance.Enable(true);
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
        }
    }
}
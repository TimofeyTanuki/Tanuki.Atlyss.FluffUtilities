using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class FreeCamera : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Managers.FreeCamera.Instance.Status)
        {
            Managers.FreeCamera.Instance.Disable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
        }
        else
        {
            Managers.FreeCamera.Instance.Enable(
                Configuration.Instance.FreeCamera.LockCharacterControls.Value,
                Configuration.Instance.FreeCamera.SmoothLookMode.Value
            );
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
        }

        return false;
    }
}
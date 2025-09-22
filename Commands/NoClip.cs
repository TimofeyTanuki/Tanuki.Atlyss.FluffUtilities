using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class NoClip : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Managers.NoClip.Instance.Status)
        {
            Managers.NoClip.Instance.Disable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
        }
        else
        {
            Managers.NoClip.Instance.Enable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
        }

        return false;
    }
}
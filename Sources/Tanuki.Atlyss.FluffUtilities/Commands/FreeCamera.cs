using System;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class FreeCamera : ICommand
{
    public void Execute(IContext context)
    {
        /*
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
        */

        Console.WriteLine("Not implemented :(");
    }
}
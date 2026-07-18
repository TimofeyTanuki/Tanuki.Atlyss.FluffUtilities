using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class FreeCamera : ICommand
{
    public void Execute(IContext context) =>
        Main.Instance.Managers.FreeCamera.ToggleState();
}
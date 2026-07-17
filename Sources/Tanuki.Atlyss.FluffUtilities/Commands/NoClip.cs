using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class NoClip : ICommand
{
    public void Execute(IContext context) =>
        Main.Instance.Managers.NoClip.ToggleState();
}
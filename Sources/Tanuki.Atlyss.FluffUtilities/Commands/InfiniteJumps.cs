using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class InfiniteJumps : ICommand
{
    private static bool state;

    static InfiniteJumps()
    {
        state = false;

        Main.Instance.OnUnload += Disable;
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += Disable;
    }

    public void Execute(IContext context)
    {
        Player player = Player._mainPlayer;

        if (state)
        {
            Disable();
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockoutSound);
        }
        else
        {
            Enable();
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
        }
    }

    private static void Enable()
    {
        if (state)
            return;

        Game.Patches.PlayerMove.Init_Jump.OnPostfix += OnPlayerMoveInitJumpPostfix;

        state = true;
    }

    private static void Disable()
    {
        if (!state)
            return;

        Game.Patches.PlayerMove.Init_Jump.OnPostfix -= OnPlayerMoveInitJumpPostfix;

        state = false;
    }

    private static void OnPlayerMoveInitJumpPostfix(PlayerMove playerMove, float force, float forwardForce, float gravityMultiply, bool useAnim)
    {
        if (!playerMove.isLocalPlayer)
            return;

        playerMove._currentJumps = 0;
    }
}

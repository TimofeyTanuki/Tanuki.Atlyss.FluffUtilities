using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class NoSkillTimer : ICommand
{
    private static bool state;

    static NoSkillTimer()
    {
        state = false;

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

    private static void OnPlayerCastingCmdInitSkillPostfix(PlayerCasting playerCasting)
    {
        if (!playerCasting.isLocalPlayer)
            return;

        if (!playerCasting._currentCastSkill)
            return;

        playerCasting._cooldownTimer = 0;
        playerCasting._castTimer = float.MaxValue;

        if (!Player._mainPlayer._isHostPlayer)
            playerCasting.Cmd_CastInit();
    }

    private static void Enable()
    {
        if (state)
            return;

        state = true;
        Game.Patches.PlayerCasting.Cmd_InitSkill.OnPostfix += OnPlayerCastingCmdInitSkillPostfix;
    }

    private static void Disable()
    {
        if (!state)
            return;

        state = false;
        Game.Patches.PlayerCasting.Cmd_InitSkill.OnPostfix -= OnPlayerCastingCmdInitSkillPostfix;
    }
}

using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class NoSkillCooldown : ICommand
{
    private static bool state;

    static NoSkillCooldown()
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

    private static void OnPlayerCastingNewCooldownSlotPrefix(PlayerCasting playerCasting, ref ScriptableSkill setSkill, ref bool runOriginal)
    {
        if (!playerCasting.isLocalPlayer)
            return;

        runOriginal = false;
    }

    private static void Enable()
    {
        if (state)
            return;

        state = true;
        Game.Patches.PlayerCasting.New_CooldownSlot.OnPrefix += OnPlayerCastingNewCooldownSlotPrefix;
    }

    private static void Disable()
    {
        if (!state)
            return;

        state = false;
        Game.Patches.PlayerCasting.New_CooldownSlot.OnPrefix -= OnPlayerCastingNewCooldownSlotPrefix;
    }
}
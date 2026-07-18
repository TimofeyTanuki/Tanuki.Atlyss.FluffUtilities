using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Immortality : ICommand
{
    private static bool state;

    static Immortality()
    {
        state = false;

        Main.Instance.OnUnload += Disable;
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += Disable;
    }

    public void Execute(IContext context)
    {
        Player player = Player._mainPlayer;

        if (!player._isHostPlayer)
        {
            Chat.AddTranslatedMessage("Commands.Immortality.NotHost");
            return;
        }

        if (state)
        {
            Disable();
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockoutSound);
            Chat.AddTranslatedMessage("Commands.Immortality.Disabled");
        }
        else
        {
            Enable();
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
            Chat.AddTranslatedMessage("Commands.Immortality.Enabled");
        }
    }

    private static void Enable()
    {
        if (state)
            return;

        state = true;
        Game.Patches.StatusEntity.Take_Damage.OnPrefix += OnStatusEntityTakeDamagePrefix;
    }

    private static void Disable()
    {
        if (!state)
            return;

        state = false;
        Game.Patches.StatusEntity.Take_Damage.OnPrefix -= OnStatusEntityTakeDamagePrefix;
    }

    private static void OnStatusEntityTakeDamagePrefix(StatusEntity StatusEntity, ref DamageStruct DamageStruct, ref bool ShouldAllow)
    {
        if (!StatusEntity.isLocalPlayer)
            return;

        ShouldAllow = false;
        StatusEntity.Rpc_DisplayBlockHitEffect(StatusEntity, 1, false, DamageStruct._damageType == DamageType.Mind, DamageStruct._colliderHitPoint);
    }
}
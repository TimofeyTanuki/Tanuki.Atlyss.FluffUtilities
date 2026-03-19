using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Immortality : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    private static bool state;

    static Immortality()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;

        state = false;

        Main.Instance.OnUnload += Disable;
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += Disable;
    }

    public void Execute(IContext context)
    {
        Player player = Player._mainPlayer;

        if (!player._isHostPlayer)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Immortality.NotHost"));
            return;
        }

        if (state)
        {
            Disable();
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockoutSound);
            chatManager.SendClientMessage(translationSet.Translate("Commands.Immortality.Disabled"));
        }
        else
        {
            Enable();
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
            chatManager.SendClientMessage(translationSet.Translate("Commands.Immortality.Enabled"));
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
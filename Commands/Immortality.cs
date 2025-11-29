using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Immortality : ICommand, IDisposable
{

    private bool Status = false;
    public Immortality() =>
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += Disable;

    public bool Execute(string[] Arguments)
    {
        if (!Player._mainPlayer._isHostPlayer)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Immortality.NotHost"));
            return false;
        }

        if (Status)
        {
            Disable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Immortality.Disabled"));
        }
        else
        {
            Enable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Immortality.Enabled"));
        }

        return false;
    }
    private void Enable()
    {
        if (Status)
            return;

        Status = true;
        Main.Instance.Patcher.Use(typeof(Game.Patches.StatusEntity.Take_Damage_Prefix));
        Game.Patches.StatusEntity.Take_Damage_Prefix.OnInvoke += Subtract_Health_Before;
    }
    private void Disable()
    {
        if (!Status)
            return;

        Status = false;
        Game.Patches.StatusEntity.Take_Damage_Prefix.OnInvoke -= Subtract_Health_Before;
    }
    private void Subtract_Health_Before(StatusEntity StatusEntity, ref DamageStruct DamageStruct, ref bool ShouldAllow)
    {
        if (!StatusEntity.isLocalPlayer)
            return;

        ShouldAllow = false;
        StatusEntity.Rpc_DisplayBlockHitEffect(StatusEntity, 1, false, DamageStruct._damageType == DamageType.Mind, DamageStruct._colliderHitPoint);
    }
    public void Dispose()
    {
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= Disable;
        Disable();
    }
}
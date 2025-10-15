using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class NoSkillCooldown : ICommand, IDisposable
{
    private bool Status = false;
    public NoSkillCooldown() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += Disable;
    public bool Execute(string[] Arguments)
    {
        if (Status)
        {
            Disable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
        }
        else
        {
            Enable();
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
        }

        return false;
    }
    private void Enable()
    {
        if (Status)
            return;

        Status = true;
        Main.Instance.Patcher.Use(typeof(Game.Events.PlayerCasting.New_CooldownSlot_Prefix));
        Game.Events.PlayerCasting.New_CooldownSlot_Prefix.OnInvoke += New_CooldownSlot_Prefix_OnInvoke;
    }
    private void Disable()
    {
        if (!Status)
            return;

        Status = false;
        Game.Events.PlayerCasting.New_CooldownSlot_Prefix.OnInvoke -= New_CooldownSlot_Prefix_OnInvoke;
    }
    private void New_CooldownSlot_Prefix_OnInvoke(PlayerCasting PlayerCasting, ref ScriptableSkill ScriptableSkill, ref bool ShouldAllow)
    {
        if (!PlayerCasting.isLocalPlayer)
            return;

        ShouldAllow = false;
    }
    public void Dispose()
    {
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= Disable;
        Disable();
    }
}
using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class NoSkillCooldown : ICommand, IDisposable
{
    private bool Status = false;
    public NoSkillCooldown() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;
    public bool Execute(string[] Arguments)
    {
        if (Status)
        {
            Game.Events.PlayerCasting.New_CooldownSlot_Prefix.OnInvoke -= New_CooldownSlot_Prefix_OnInvoke;

            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
        }
        else
        {
            Game.Main.Instance.Patch(typeof(Game.Events.PlayerCasting.New_CooldownSlot_Prefix));
            Game.Events.PlayerCasting.New_CooldownSlot_Prefix.OnInvoke += New_CooldownSlot_Prefix_OnInvoke;

            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
        }

        Status = !Status;

        return false;
    }
    private void New_CooldownSlot_Prefix_OnInvoke(PlayerCasting PlayerCasting, ref ScriptableSkill ScriptableSkill, ref bool ShouldAllow) =>
        ShouldAllow = !PlayerCasting.netIdentity.isLocalPlayer;
    private void OnStopClient_Prefix_OnInvoke() =>
        Game.Events.PlayerCasting.New_CooldownSlot_Prefix.OnInvoke -= New_CooldownSlot_Prefix_OnInvoke;
    public void Dispose()
    {
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= OnStopClient_Prefix_OnInvoke;

        if (!Status)
            return;

        Game.Events.PlayerCasting.New_CooldownSlot_Prefix.OnInvoke -= New_CooldownSlot_Prefix_OnInvoke;
    }
}
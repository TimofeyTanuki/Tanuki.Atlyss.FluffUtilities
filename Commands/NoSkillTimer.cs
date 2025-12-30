using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class NoSkillTimer : ICommand, IDisposable
{
    private bool Status = false;

    public NoSkillTimer() =>
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += Disable;

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
        Main.Instance.Patcher.Use(typeof(Game.Patches.PlayerCasting.Cmd_InitSkill_Postfix));
        Game.Patches.PlayerCasting.Cmd_InitSkill_Postfix.OnInvoke += Cmd_InitSkill_Postfix_OnInvoke;
    }

    private void Cmd_InitSkill_Postfix_OnInvoke(PlayerCasting PlayerCasting)
    {
        if (!PlayerCasting.isLocalPlayer)
            return;

        if (!PlayerCasting._currentCastSkill)
            return;

        PlayerCasting._cooldownTimer = 0;
        PlayerCasting._castTimer = float.MaxValue;

        if (!Player._mainPlayer._isHostPlayer)
            PlayerCasting.Cmd_CastInit();
    }

    private void Disable()
    {
        if (!Status)
            return;

        Status = false;
        Game.Patches.PlayerCasting.Cmd_InitSkill_Postfix.OnInvoke -= Cmd_InitSkill_Postfix_OnInvoke;
    }

    public void Dispose()
    {
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= Disable;
        Disable();
    }
}
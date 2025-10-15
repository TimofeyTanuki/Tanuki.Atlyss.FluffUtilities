using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class InfiniteJumps : ICommand, IDisposable
{
    private bool Status = false;
    public InfiniteJumps() =>
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

        Main.Instance.Patcher.Use(typeof(Game.Events.PlayerMove.Init_Jump_Postfix));
        Game.Events.PlayerMove.Init_Jump_Postfix.OnInvoke += Init_Jump_Postfix_OnInvoke;

        Status = true;
    }
    private void Disable()
    {
        if (!Status)
            return;

        Game.Events.PlayerMove.Init_Jump_Postfix.OnInvoke -= Init_Jump_Postfix_OnInvoke;

        Status = false;
    }
    private void Init_Jump_Postfix_OnInvoke(PlayerMove PlayerMove, float Force, float ForwardForce, float GravityMultiply, bool UseAnim)
    {
        if (!PlayerMove.isLocalPlayer)
            return;

        PlayerMove._currentJumps = 0;
    }

    public void Dispose()
    {
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= Disable;

        if (Status)
            Disable();
    }
}
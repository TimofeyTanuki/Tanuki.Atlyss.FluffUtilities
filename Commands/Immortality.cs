using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Immortality : ICommand, IDisposable
{

    private bool Status = false;
    public Immortality() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;
    private void OnStopClient_Prefix_OnInvoke()
    {
        if (!Status)
            return;

        Status = false;
        Game.Events.StatusEntity.Subtract_Health_Prefix.OnInvoke -= Subtract_Health_Before;
    }

    public bool Execute(string[] Arguments)
    {
        if (!Player._mainPlayer._isHostPlayer)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Immortality.NotHost"));
            return false;
        }

        if (Status)
        {
            Game.Events.StatusEntity.Subtract_Health_Prefix.OnInvoke += Subtract_Health_Before;
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Immortality.Disabled"));
        }
        else
        {
            Game.Main.Instance.Patch(typeof(Game.Events.StatusEntity.Subtract_Health_Prefix));

            Game.Events.StatusEntity.Subtract_Health_Prefix.OnInvoke -= Subtract_Health_Before;
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Immortality.Enabled"));
        }

        Status = !Status;
        return false;
    }
    private void Subtract_Health_Before(StatusEntity StatusEntity, ref int Value)
    {
        if (!StatusEntity._isPlayer.isLocalPlayer)
            return;

        if (Status)
            Value = 0;
    }
    public void Dispose()
    {
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= OnStopClient_Prefix_OnInvoke;

        if (!Status)
            return;

        Game.Events.StatusEntity.Subtract_Health_Prefix.OnInvoke -= Subtract_Health_Before;
    }
}
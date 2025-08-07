using System;
using Tanuki.Atlyss.API.Commands;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class AutoPickup : ICommand, IDisposable
{
    private float Distance = 0;
    private bool Status = false;
    public AutoPickup() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;
    public void Execute(string[] Arguments)
    {
        if (Arguments.Length != 0)
        {
            if (float.TryParse(Arguments[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out Distance))
            {
                if (Distance > 0)
                {
                    if (!Status)
                        Game.Events.ItemObject.Enable_GroundCheckToVelocityZero_Postfix.OnInvoke += Enable_GroundCheckToVelocityZero;

                    Status = true;
                    ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.AutoPickup.Enabled", Distance));
                    Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
                    return;
                }
            }
            else
            {
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.AutoPickup.DistanceNotFloat"));
                return;
            }
        }

        if (Status)
        {
            Status = false;
            Game.Events.ItemObject.Enable_GroundCheckToVelocityZero_Postfix.OnInvoke -= Enable_GroundCheckToVelocityZero;
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.AutoPickup.Disabled"));
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockoutSound);
        }
        else
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.AutoPickup.DistanceNotSpecified"));
    }

    private void OnStopClient_Prefix_OnInvoke()
    {
        if (!Status)
            return;

        Status = false;
        Game.Events.ItemObject.Enable_GroundCheckToVelocityZero_Postfix.OnInvoke -= Enable_GroundCheckToVelocityZero;
    }

    private void Enable_GroundCheckToVelocityZero(ItemObject ItemObject)
    {
        if (ItemObject._isPickedUp)
            return;

        if (!ItemObject._canPickUp)
            return;

        if (Vector3.Distance(Player._mainPlayer.transform.position, ItemObject._interactableCollider.transform.position) > Distance)
            return;

        ItemObject.Init_PickupItem(Player._mainPlayer.netIdentity);
    }
    public void Dispose()
    {
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= OnStopClient_Prefix_OnInvoke;

        if (!Status)
            return;

        Game.Events.ItemObject.Enable_GroundCheckToVelocityZero_Postfix.OnInvoke -= Enable_GroundCheckToVelocityZero;
    }
}
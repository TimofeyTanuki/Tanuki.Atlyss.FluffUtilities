using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class AutoPickup : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    private static bool state;
    private static float distance;

    static AutoPickup()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;

        state = false;
        distance = 0;

        Main.Instance.OnUnload += Disable;
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += Disable;
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> Arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (Arguments.Count != 0)
        {
            if (float.TryParse(Arguments[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out distance))
            {
                if (distance > 0)
                {
                    Enable();
                    chatManager.SendClientMessage(translationSet.Translate("Commands.AutoPickup.Enabled", distance));
                    player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
                    return;
                }
            }
            else
            {
                chatManager.SendClientMessage(translationSet.Translate("Commands.AutoPickup.DistanceNotFloat"));
                return;
            }
        }

        if (state)
        {
            Disable();
            chatManager.SendClientMessage(translationSet.Translate("Commands.AutoPickup.Disabled"));
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockoutSound);
        }
        else
            chatManager.SendClientMessage(translationSet.Translate("Commands.AutoPickup.DistanceNotSpecified"));
    }

    private static void Enable()
    {
        if (state)
            return;

        state = true;
        Game.Patches.ItemObject.Enable_GroundCheckToVelocityZero.OnPostfix += OnGroundCheckToVelocityZeroPostfix;
    }

    private static void Disable()
    {
        if (!state)
            return;

        state = false;
        Game.Patches.ItemObject.Enable_GroundCheckToVelocityZero.OnPostfix -= OnGroundCheckToVelocityZeroPostfix;
    }

    private static void OnGroundCheckToVelocityZeroPostfix(ItemObject ItemObject)
    {
        if (!ItemObject._canPickUp)
            return;

        Player player = Player._mainPlayer;

        if (Vector3.Distance(player.transform.position, ItemObject._interactableCollider.transform.position) > distance)
            return;

        ItemObject.Init_PickupItem(player.netIdentity);
    }
}

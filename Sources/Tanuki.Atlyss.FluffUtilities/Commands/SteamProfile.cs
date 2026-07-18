using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Helpers;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class SteamProfile : ICommand
{
    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (arguments.Count == 0)
        {
            Chat.AddTranslatedMessage("Commands.SteamProfile.NicknameNotSpecified");
            return;
        }

        Player? targetPlayer = Game.Tanuki.Instance.Providers.Player.FindByFlexibleInput(string.Join(" ", arguments));

        if (targetPlayer is null)
        {
            Chat.AddTranslatedMessage("Commands.SteamProfile.PlayerNotFound");
            return;
        }

        Application.OpenURL(string.Format(Configuration.Instance.Commands.SteamProfile_LinkTemplate.Value, targetPlayer._steamID));
        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
    }
}

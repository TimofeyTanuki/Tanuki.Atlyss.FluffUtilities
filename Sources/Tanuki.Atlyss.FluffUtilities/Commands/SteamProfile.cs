using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class SteamProfile : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;
    private static readonly Game.Providers.Player playerProvider;

    static SteamProfile()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
        playerProvider = Game.Tanuki.Instance.Providers.Player;
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (arguments.Count == 0)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.SteamProfile.NicknameNotSpecified"));
            return;
        }

        Player? targetPlayer = playerProvider.FindByFlexibleInput(string.Join(" ", arguments));

        if (targetPlayer is null)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.SteamProfile.PlayerNotFound"));
            return;
        }

        Application.OpenURL(string.Format(Configuration.Instance.Commands.SteamProfile_LinkTemplate.Value, targetPlayer._steamID));
        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
    }
}

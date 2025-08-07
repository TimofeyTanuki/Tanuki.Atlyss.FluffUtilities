using System.Linq;
using Tanuki.Atlyss.API.Commands;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class SteamProfile : ICommand
{
    public void Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.SteamProfile.NicknameNotSpecified"));
            return;
        }

        string Nickname = string.Join(" ", Arguments.Select(x => x.ToLower()));
        Player[] Players = Object.FindObjectsOfType<Player>();

        foreach (Player Player in Players)
        {
            if (Player.isLocalPlayer)
                continue;

            if (!Player._nickname.ToLower().Contains(Nickname))
                continue;

            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.SteamProfile", Player._steamID));
            Application.OpenURL(string.Format(Configuration.Instance.Commands.SteamProfile_LinkTemplate.Value, Player._steamID));
            return;
        }

        ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.SteamProfile.PlayerNotFound"));
    }
}
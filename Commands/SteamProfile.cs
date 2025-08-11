using System.Linq;
using Tanuki.Atlyss.API.Commands;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class SteamProfile : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.SteamProfile.NicknameNotSpecified"));
            return false;
        }

        string Nickname = string.Join(" ", Arguments.Select(x => x.ToLower()));
        Player[] Players = Object.FindObjectsOfType<Player>();

        foreach (Player Player in Players)
        {
            if (Player.isLocalPlayer)
                continue;

            if (!Player._nickname.ToLower().Contains(Nickname))
                continue;

            Application.OpenURL(string.Format(Configuration.Instance.Commands.SteamProfile_LinkTemplate.Value, Player._steamID));
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
            return false;
        }

        ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.SteamProfile.PlayerNotFound"));

        return false;
    }
}
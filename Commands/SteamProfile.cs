using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;
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

        Player Player = global::Player.GetByAutoRecognition(string.Join(" ", Arguments));

        if (!Player)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.SteamProfile.PlayerNotFound"));
            return false;
        }

        Application.OpenURL(string.Format(Configuration.Instance.Commands.SteamProfile_LinkTemplate.Value, Player._steamID));
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);

        return false;
    }
}
using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Weather : ICommand
{
    Managers.GameWorld.Controllers.Weather WeatherController => Main.Instance.Managers.GameWorld.WeatherController;

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;

        if (arguments.Count == 0)
        {
            if (WeatherController.HostSync)
            {
                Chat.AddTranslatedMessage("Commands.Weather.StateNotSpecified");
                return;
            }

            WeatherController.SetHostSyncState(true);
            Chat.AddTranslatedMessage("Commands.Weather.HostSync");
            return;
        }

        if (!bool.AdvancedTryParse(arguments[0], out bool state))
        {
            Chat.AddTranslatedMessage("Commands.Weather.InvalidState");
            return;
        }

        string translatedState = Main.Translate(state ? "Commands.Weather.State.Enabled" : "Commands.Weather.State.Disabled");

        if (Player._mainPlayer._isHostPlayer)
            Chat.AddTranslatedMessage("Commands.Weather.Host", translatedState);
        else
        {
            WeatherController.SetHostSyncState(false);
            Chat.AddTranslatedMessage("Commands.Weather.Client", translatedState);
        }

        Player._mainPlayer._playerMapInstance._isWeatherEnabled = state;
    }
}
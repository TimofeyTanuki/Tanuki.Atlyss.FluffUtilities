using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.FluffUtilities.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Time : ICommand
{
    Managers.GameWorld.Controllers.Time TimeController => Main.Instance.Managers.GameWorld.TimeController;

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;

        if (arguments.Count == 0)
        {
            if (TimeController.HostSync)
            {
                Chat.AddTranslatedMessage("Commands.Time.TimeNotSpecified");
                return;
            }

            TimeController.SetHostSyncState(true);
            Chat.AddTranslatedMessage("Commands.Time.HostSync");
            return;
        }

        if (!byte.TryParse(arguments[0], out byte time))
        {
            Chat.AddTranslatedMessage("Commands.Time.InvalidTime");
            return;
        }

        ClockSetting clockSetting;
        if (arguments.Count > 1)
        {
            if (!Enum.TryParse(arguments[1], true, out clockSetting))
            {
                Chat.AddTranslatedMessage(
                    "Commands.Time.Meridiems",
                    string.Join(
                        Main.Translate("Commands.Time.Meridiems.Separator"),
                        Enum.GetNames(typeof(ClockSetting))
                    )
                );
                return;
            }
        }
        else
        {
            time %= 24;
            clockSetting = time < 12 ? ClockSetting.AM : ClockSetting.PM;
        }

        time %= 12;
        if (time == 0)
            time = 12;

        if (Player._mainPlayer._isHostPlayer)
            Chat.AddTranslatedMessage("Commands.Time.Host", time, clockSetting);
        else
        {
            TimeController.SetHostSyncState(false);
            Chat.AddTranslatedMessage("Commands.Time.Client", time, clockSetting);
        }

        GameWorldManager gameWorldManager = GameWorldManager._current;

        gameWorldManager._timeDisplay = time;
        gameWorldManager._clockSetting = clockSetting;
        gameWorldManager._worldTime = (clockSetting == ClockSetting.AM && time >= 6) || (clockSetting == ClockSetting.PM && time < 8) ? WorldTime.DAY : WorldTime.NIGHT;
        Game.Accessors.GameWorldManager._currentDayNightCycleBuffer(gameWorldManager) = 0f;

        TimeController.ApplySimulationToMap();
    }
}

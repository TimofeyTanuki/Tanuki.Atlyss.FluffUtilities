using System;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class TimeSet : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            if (Managers.MapInstance.Instance.IsHostTime)
            {
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TimeSet.TimeNotSpecified"));
                return false;
            }

            Managers.MapInstance.Instance.FollowHostTime();
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TimeSet.FollowingHostTime"));
            return false;
        }

        if (!byte.TryParse(Arguments[0], out byte Time))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TimeSet.InvalidTime"));
            return false;
        }

        ClockSetting ClockSetting;
        if (Arguments.Length > 1)
        {
            if (!Enum.TryParse(Arguments[1], true, out ClockSetting))
            {
                ChatBehaviour._current.New_ChatMessage(
                    Main.Instance.Translate(
                        "Commands.TimeSet.Meridiems",
                        string.Join(
                            Main.Instance.Translate("Commands.TimeSet.Meridiems.Separator"),
                            Enum.GetNames(typeof(ClockSetting))
                        )
                    )
                );
                return false;
            }
        }
        else
        {
            Time %= 24;
            ClockSetting = Time < 12 ? ClockSetting.AM : ClockSetting.PM;
        }

        Time %= 12;
        if (Time == 0)
            Time = 12;

        GameWorldManager._current._timeDisplay = Time;
        GameWorldManager._current._clockSetting = ClockSetting;
        GameWorldManager._current._worldTime = (ClockSetting == ClockSetting.AM && Time >= 6) || (ClockSetting == ClockSetting.PM && Time < 8) ? WorldTime.DAY : WorldTime.NIGHT;

        if (Player._mainPlayer._isHostPlayer)
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TimeSet.Host", Time, ClockSetting));
        else
        {
            Managers.MapInstance.Instance.FollowClientTime();
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.TimeSet.Client", Time, ClockSetting));
        }

        return false;
    }
}
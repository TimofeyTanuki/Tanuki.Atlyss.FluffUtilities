using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Experience : ICommand
{
    public void Execute(string[] Arguments)
    {
        if (!Player._mainPlayer._isHostPlayer)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Experience.NotHost"));
            return;
        }

        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Experience.DeltaNotSpecified"));
            return;
        }

        if (!int.TryParse(Arguments[0], out int Delta))
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Experience.DeltaNotInteger"));
            return;
        }

        if (Delta < 0)
        {
            Delta = -Delta;

            int NewLevel = Player._mainPlayer._pStats._currentLevel;
            int NewExperience = Player._mainPlayer._pStats._currentExp;

            while (Delta > NewExperience && NewLevel != 1)
            {
                NewLevel--;
                Delta -= NewExperience;
                NewExperience = (int)GameManager._current._statLogics._experienceCurve.Evaluate(NewLevel);
            }

            NewExperience -= Delta;
            if (NewExperience < 0)
                NewExperience = 0;

            if (Player._mainPlayer._pStats._currentLevel != NewLevel)
            {
                Player._mainPlayer._pStats.Network_currentLevel = NewLevel;
                Player._mainPlayer._pStats.Apply_StatCalculations();
            }

            Player._mainPlayer._pStats.Network_currentExp = NewExperience;
        }
        else
            Player._mainPlayer._pStats.Network_currentExp += Delta;

        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
    }
}
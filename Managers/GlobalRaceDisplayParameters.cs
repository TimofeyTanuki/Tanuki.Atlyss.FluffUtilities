using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class GlobalRaceDisplayParameters
{
    public static GlobalRaceDisplayParameters Instance;
    private bool
        Enabled = false,
        Reloaded = false;

    private GlobalRaceDisplayParameters() { }

    public static void Initialize() => Instance ??= new();

    public void Load()
    {
        Enabled = Configuration.Instance.PlayerAppearance.AllowParametersBeyondLimits.Value;
        if (!Enabled)
            return;

        if (Reloaded)
        {
            Override();
            return;
        }

        Main.Instance.Patcher.Use(typeof(Game.Patches.GameManager.Cache_ScriptableAssets_Postfix));
        Game.Patches.GameManager.Cache_ScriptableAssets_Postfix.OnInvoke += Override;
    }

    public void Unload()
    {
        Reloaded = true;

        if (!Enabled)
            return;

        Game.Patches.GameManager.Cache_ScriptableAssets_Postfix.OnInvoke -= Override;
    }

    public void Override()
    {
        Models.Configuration.GlobalRaceDisplayParameters GlobalRaceDisplayParameters = Configuration.Instance.GlobalRaceDisplayParameters;
        Vector2
            HeadWidth = GlobalRaceDisplayParameters.HeadWidth.AsVector2,
            MuzzleLength = GlobalRaceDisplayParameters.MuzzleLength.AsVector2,
            Height = GlobalRaceDisplayParameters.Height.AsVector2,
            Width = GlobalRaceDisplayParameters.Width.AsVector2,
            TorsoSize = GlobalRaceDisplayParameters.TorsoSize.AsVector2,
            BreastSize = GlobalRaceDisplayParameters.BreastSize.AsVector2,
            ArmsSize = GlobalRaceDisplayParameters.ArmsSize.AsVector2,
            BellySize = GlobalRaceDisplayParameters.BellySize.AsVector2,
            BottomSize = GlobalRaceDisplayParameters.BottomSize.AsVector2,
            VoicePitch = GlobalRaceDisplayParameters.VoicePitch.AsVector2;

        CharacterParamsGroup CharacterParamsGroup;
        foreach (ScriptablePlayerRace ScriptablePlayerRace in Game.Accessors.GameManager._cachedScriptableRaces(GameManager._current).Values)
        {
            CharacterParamsGroup = ScriptablePlayerRace._raceDisplayParams;
            CharacterParamsGroup._headWidthRange = HeadWidth;
            CharacterParamsGroup._headModRange = MuzzleLength;
            CharacterParamsGroup._heightRange = Height;
            CharacterParamsGroup._widthRange = Width;
            CharacterParamsGroup._torsoRange = TorsoSize;
            CharacterParamsGroup._boobRange = BreastSize;
            CharacterParamsGroup._armRange = ArmsSize;
            CharacterParamsGroup._bellyRange = BellySize;
            CharacterParamsGroup._bottomRange = BottomSize;
            CharacterParamsGroup._pitchRange = VoicePitch;
        }
    }
}
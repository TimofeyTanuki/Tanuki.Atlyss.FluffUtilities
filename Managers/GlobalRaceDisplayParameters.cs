namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class GlobalRaceDisplayParameters
{
    public static GlobalRaceDisplayParameters Instance;
    private GlobalRaceDisplayParameters() { }
    public static void Initialize() =>
        Instance ??= new();

    private bool Enabled, Reloaded = false;
    public void Load()
    {
        Enabled = Configuration.Instance.PlayerAppearance.DisableParametersCheck.Value;
        if (!Enabled)
            return;

        if (Reloaded)
        {
            Override();
            return;
        }

        Game.Main.Instance.Patch(typeof(Game.Events.GameManager.Cache_ScriptableAssets_Postfix));
        Game.Events.GameManager.Cache_ScriptableAssets_Postfix.OnInvoke += Override;
    }
    public void Unload()
    {
        Reloaded = true;

        if (!Enabled)
            return;

        Game.Events.GameManager.Cache_ScriptableAssets_Postfix.OnInvoke -= Override;
    }
    public void Override()
    {
        CharacterParamsGroup CharacterParamsGroup;
        foreach (ScriptablePlayerRace ScriptablePlayerRace in Game.Fields.GameManager.Instance.CachedScriptableRaces.Values)
        {
            CharacterParamsGroup = ScriptablePlayerRace._raceDisplayParams;
            CharacterParamsGroup._headWidthRange = Configuration.Instance.GlobalRaceDisplayParameters.HeadWidth.AsVector2;
            CharacterParamsGroup._headModRange = Configuration.Instance.GlobalRaceDisplayParameters.MuzzleLength.AsVector2;
            CharacterParamsGroup._heightRange = Configuration.Instance.GlobalRaceDisplayParameters.Height.AsVector2;
            CharacterParamsGroup._widthRange = Configuration.Instance.GlobalRaceDisplayParameters.Width.AsVector2;
            CharacterParamsGroup._torsoRange = Configuration.Instance.GlobalRaceDisplayParameters.TorsoSize.AsVector2;
            CharacterParamsGroup._boobRange = Configuration.Instance.GlobalRaceDisplayParameters.BreastSize.AsVector2;
            CharacterParamsGroup._armRange = Configuration.Instance.GlobalRaceDisplayParameters.ArmsSize.AsVector2;
            CharacterParamsGroup._bellyRange = Configuration.Instance.GlobalRaceDisplayParameters.BellySize.AsVector2;
            CharacterParamsGroup._bottomRange = Configuration.Instance.GlobalRaceDisplayParameters.BottomSize.AsVector2;
            CharacterParamsGroup._pitchRange = Configuration.Instance.GlobalRaceDisplayParameters.VoicePitch.AsVector2;
        }
    }
}
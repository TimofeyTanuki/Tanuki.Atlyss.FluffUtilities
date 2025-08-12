namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class PlayerAppearance
{
    public static PlayerAppearance Instance;
    private PlayerAppearance() { }
    public static void Initialize() =>
        Instance ??= new();

    private bool DisableParameterCheck;
    public void Load()
    {
        DisableParameterCheck = Configuration.Instance.PlayerAppearance.DisableParametersCheck.Value;
        if (DisableParameterCheck)
        {
            Game.Main.Instance.Patch(typeof(Game.Events.ScriptablePlayerRace.Init_ParamsCheck_Prefix));
            Game.Events.ScriptablePlayerRace.Init_ParamsCheck_Prefix.OnInvoke += Init_ParamsCheck_Prefix_OnInvoke;
        }
    }
    public void Unload()
    {
        if (DisableParameterCheck)
            Game.Events.ScriptablePlayerRace.Init_ParamsCheck_Prefix.OnInvoke -= Init_ParamsCheck_Prefix_OnInvoke;
    }
    public void Reload()
    {
        Unload();
        Load();
    }
    private void Init_ParamsCheck_Prefix_OnInvoke(PlayerAppearance_Profile PlayerAppearance, ref bool ShouldAllow) =>
        ShouldAllow = false;
    private void ApplyPlayerAppearanceStruct(ref PlayerAppearanceStruct PlayerAppearanceStruct)
    {
        Player._mainPlayer._pVisual.Network_playerAppearanceStruct = PlayerAppearanceStruct;
        Player._mainPlayer._pVisual.Apply_NetworkedCharacterDisplay();

        if (Player._mainPlayer._isHostPlayer)
            return;

        Player._mainPlayer._pVisual.Cmd_SendNew_PlayerAppearanceStruct(Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyBreastSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._boobWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._boobWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._boobWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyArmsSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._armWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._armWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._armWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyBellySize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._bellyWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._bellyWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._bellyWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyBottomSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._bottomWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._bottomWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._bottomWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyTorsoSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._torsoWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._torsoWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._torsoWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyMuzzleLength(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._muzzleWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._muzzleWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._muzzleWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyHeight(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._heightWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._heightWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._heightWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyWidth(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._widthWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._widthWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._widthWeight;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyHeadWidth(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._headWidth += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._headWidth = Player._mainPlayer._pVisual._playerAppearanceStruct._headWidth;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
    public void ModifyVoicePitch(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._voicePitch += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._voicePitch = Player._mainPlayer._pVisual._playerAppearanceStruct._voicePitch;

        ApplyPlayerAppearanceStruct(ref Player._mainPlayer._pVisual._playerAppearanceStruct);
    }
}
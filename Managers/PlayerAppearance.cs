using System.Collections;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class PlayerAppearance
{
    public static PlayerAppearance Instance;
    private bool AllowParametersBeyondLimits;
    private bool PendingNewPlayerAppearanceCommand;

    private PlayerAppearance() { }

    public static void Initialize() =>
        Instance ??= new();

    public void Load()
    {
        Main.Instance.Patcher.Use(typeof(Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay_Prefix));

        AllowParametersBeyondLimits = Configuration.Instance.PlayerAppearance.AllowParametersBeyondLimits.Value;
        PendingNewPlayerAppearanceCommand = false;

        if (AllowParametersBeyondLimits)
        {
            Main.Instance.Patcher.Use(typeof(Game.Patches.ScriptablePlayerRace.Init_ParamsCheck_Prefix));
            Game.Patches.ScriptablePlayerRace.Init_ParamsCheck_Prefix.OnInvoke += Init_ParamsCheck_Prefix_OnInvoke;
        }

        Game.Patches.Player.OnStartAuthority_Postfix.OnInvoke += OnStartAuthority_Postfix_OnInvoke;
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;
    }

    private void OnStopClient_Prefix_OnInvoke() =>
        Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay_Prefix.OnInvoke -= Apply_NetworkedCharacterDisplay_Prefix_OnInvoke_OnJoin;

    private void Apply_NetworkedCharacterDisplay_Prefix_OnInvoke_OnJoin(PlayerVisual PlayerVisual)
    {
        if (!PlayerVisual.isLocalPlayer)
            return;

        Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay_Prefix.OnInvoke -= Apply_NetworkedCharacterDisplay_Prefix_OnInvoke_OnJoin;

        PlayerAppearance_Profile PlayerAppearance_Profile = ProfileDataManager._current._characterFile._appearanceProfile;
        PlayerAppearanceStruct PlayerAppearanceStruct = new()
        {
            _setRaceTag = PlayerAppearance_Profile._setRaceTag,
            _textureID = PlayerAppearance_Profile._setTexture,
            _bodyColorHue = PlayerAppearance_Profile._setSkinColorProfile._hue,
            _bodyColorBrightness = PlayerAppearance_Profile._setSkinColorProfile._brightness,
            _bodyColorContrast = PlayerAppearance_Profile._setSkinColorProfile._contrast,
            _bodyColorSaturation = PlayerAppearance_Profile._setSkinColorProfile._saturation,
            _hairStyleHue = PlayerAppearance_Profile._hairColorProfile._hue,
            _hairStyleBrightness = PlayerAppearance_Profile._hairColorProfile._brightness,
            _hairStyleContrast = PlayerAppearance_Profile._hairColorProfile._contrast,
            _hairStyleSaturation = PlayerAppearance_Profile._hairColorProfile._saturation,
            _hairIsBodyColor = PlayerAppearance_Profile._hairIsBodyColor,
            _miscHue = PlayerAppearance_Profile._miscColorProfile._hue,
            _miscBrightness = PlayerAppearance_Profile._miscColorProfile._brightness,
            _miscContrast = PlayerAppearance_Profile._miscColorProfile._contrast,
            _miscSaturation = PlayerAppearance_Profile._miscColorProfile._saturation,
            _dyeIndex = PlayerAppearance_Profile._helmDyeIndex,
            _headWidth = PlayerAppearance_Profile._headWidth,
            _muzzleWeight = PlayerAppearance_Profile._muzzleWeight,
            _hairStyleID = PlayerAppearance_Profile._setHairStyle,
            _miscID = PlayerAppearance_Profile._setMisc,
            _mouthID = PlayerAppearance_Profile._setMouth,
            _earID = PlayerAppearance_Profile._setEar,
            _eyeID = PlayerAppearance_Profile._setEye,
            _voicePitch = PlayerAppearance_Profile._voicePitch,
            _heightWeight = PlayerAppearance_Profile._heightWeight,
            _widthWeight = PlayerAppearance_Profile._widthWeight,
            _torsoWeight = PlayerAppearance_Profile._torsoWeight,
            _displayBoobs = PlayerAppearance_Profile._displayBoobs,
            _boobWeight = PlayerAppearance_Profile._boobWeight,
            _armWeight = PlayerAppearance_Profile._armWeight,
            _bellyWeight = PlayerAppearance_Profile._bellyWeight,
            _bottomWeight = PlayerAppearance_Profile._bottomWeight,
            _isLeftHanded = PlayerAppearance_Profile._isLeftHanded,
            _tailID = PlayerAppearance_Profile._setTail,
            _hideHelm = PlayerAppearance_Profile._hideHelm,
            _hideCape = PlayerAppearance_Profile._hideCape,
            _hideChest = PlayerAppearance_Profile._hideChest,
            _hideLeggings = PlayerAppearance_Profile._hideLeggings,
            _hideVanityHelm = PlayerAppearance_Profile._hideVanityHelm,
            _hideVanityCape = PlayerAppearance_Profile._hideVanityCape,
            _hideVanityChest = PlayerAppearance_Profile._hideVanityChest,
            _hideVanityLeggings = PlayerAppearance_Profile._hideVanityLeggings
        };

        PlayerVisual.Cmd_SendNew_PlayerAppearanceStruct(PlayerAppearanceStruct);
    }

    private void OnStartAuthority_Postfix_OnInvoke(Player Player)
    {
        if (!AtlyssNetworkManager._current.isNetworkActive)
            return;

        if (Player._mainPlayer._isHostPlayer)
            return;

        Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay_Prefix.OnInvoke += Apply_NetworkedCharacterDisplay_Prefix_OnInvoke_OnJoin;
    }

    private IEnumerator NewPlayerAppearanceCommand()
    {
        yield return new WaitForSeconds(1);

        PendingNewPlayerAppearanceCommand = false;
        Player._mainPlayer._pVisual.Cmd_SendNew_PlayerAppearanceStruct(Player._mainPlayer._pVisual._playerAppearanceStruct);
        yield break;
    }

    public void Unload()
    {
        if (AllowParametersBeyondLimits)
            Game.Patches.ScriptablePlayerRace.Init_ParamsCheck_Prefix.OnInvoke -= Init_ParamsCheck_Prefix_OnInvoke;


        Game.Patches.Player.OnStartAuthority_Postfix.OnInvoke -= OnStartAuthority_Postfix_OnInvoke;
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke -= OnStopClient_Prefix_OnInvoke;
        Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay_Prefix.OnInvoke -= Apply_NetworkedCharacterDisplay_Prefix_OnInvoke_OnJoin;
    }

    public void Reload()
    {
        Unload();
        Load();
    }

    private void Init_ParamsCheck_Prefix_OnInvoke(PlayerAppearance_Profile PlayerAppearance, ref bool ShouldAllow) =>
        ShouldAllow = false;

    private void ApplyPlayerAppearanceStruct()
    {
        Player._mainPlayer._pVisual.Apply_NetworkedCharacterDisplay();

        if (Player._mainPlayer._isHostPlayer)
            return;

        if (PendingNewPlayerAppearanceCommand)
            return;

        PendingNewPlayerAppearanceCommand = true;
        Main.Instance.StartCoroutine(NewPlayerAppearanceCommand());
    }

    public void ModifyBreastSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._boobWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._boobWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._boobWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyArmsSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._armWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._armWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._armWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyBellySize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._bellyWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._bellyWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._bellyWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyBottomSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._bottomWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._bottomWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._bottomWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyTorsoSize(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._torsoWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._torsoWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._torsoWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyMuzzleLength(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._muzzleWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._muzzleWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._muzzleWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyHeight(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._heightWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._heightWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._heightWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyWidth(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._widthWeight += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._widthWeight = Player._mainPlayer._pVisual._playerAppearanceStruct._widthWeight;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyHeadWidth(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._headWidth += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._headWidth = Player._mainPlayer._pVisual._playerAppearanceStruct._headWidth;

        ApplyPlayerAppearanceStruct();
    }

    public void ModifyVoicePitch(float Delta, bool UpdateCharacterFile)
    {
        Player._mainPlayer._pVisual._playerAppearanceStruct._voicePitch += Delta;

        if (UpdateCharacterFile)
            ProfileDataManager._current._characterFile._appearanceProfile._voicePitch = Player._mainPlayer._pVisual._playerAppearanceStruct._voicePitch;

        ApplyPlayerAppearanceStruct();
    }
}
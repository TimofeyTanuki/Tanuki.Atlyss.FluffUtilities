using Nessie.ATLYSS.EasySettings;
using Nessie.ATLYSS.EasySettings.UIElements;
using System;
using System.Collections;
using Tanuki.Atlyss.Core.Managers;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers.Game.Controllers;

public sealed class PlayerAppearance
{
    private readonly Configuration pluginConfiguration = Configuration.Instance;
    private readonly Main plugin;
    private bool lastPlayerRaceParamsCheckState = true;
    private bool isPlayerAppearanceApplicationPending = false;

    private Hotkey HotkeyManager => Core.Tanuki.Instance.Managers.Hotkey;
    private Player CurrentPlayer => Player._mainPlayer;
    private PlayerVisual CurrentPlayerVisual => CurrentPlayer._pVisual;
    private PlayerAppearance_Profile CurrentPlayerAppearanceProfile => ProfileDataManager._current._characterFile._appearanceProfile;

    private Types.Configuration.Sections.Hotkeys ConfigurationHotkeysSection => pluginConfiguration.Hotkeys;

    private bool HotkeysAffectCharacterSave => pluginConfiguration.PlayerAppearance.HotkeysAffectCharacterSave.Value;

    internal PlayerAppearance()
    {
        plugin = Main.Instance;

        plugin.OnLoaded += OnPluginLoaded;
        plugin.OnUnload += OnPluginUnload;

        plugin.OnInitialized += OnPluginInitialized;

        Atlyss.Game.Patches.Player.OnStartAuthority.OnPostfix += OnStartAuthority;
        Atlyss.Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;
    }

    private void OnPluginInitialized() =>
        plugin.Managers.NessieEasySettings.OnModSettingsTabInitialized += OnModSettingsTabInitialized;

    private void OnSettingsUpdated() =>
        ReconfigurePlayerRaceParamsCheck();

    private void OnHotkeysUpdated()
    {
        DeregisterHotkeys();
        RegisterHotkeys();
    }

    private void OnPluginLoaded()
    {
        ReconfigurePlayerRaceParamsCheck();

        if (CurrentPlayer)
            RegisterHotkeys();
    }

    private void OnStartAuthority(Player player)
    {
        RegisterHotkeys();

        if (!AtlyssNetworkManager._current.isNetworkActive)
            return;

        if (player._isHostPlayer)
            return;

        Atlyss.Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay.OnPrefix += OnNetworkedCharacterDisplayAppliedFirstTime;
    }

    private void OnStopClient()
    {
        DisableFirstCharacterDisplayApplication();
        DeregisterHotkeys();
    }

    private void DisableFirstCharacterDisplayApplication() =>
        Atlyss.Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay.OnPrefix -= OnNetworkedCharacterDisplayAppliedFirstTime;

    private void OnNetworkedCharacterDisplayAppliedFirstTime(PlayerVisual playerVisual, ref bool runOriginal)
    {
        if (!playerVisual.isLocalPlayer)
            return;

        DisableFirstCharacterDisplayApplication();

        PlayerAppearance_Profile playerAppearanceProfile = ProfileDataManager._current._characterFile._appearanceProfile;
        PlayerAppearanceStruct PlayerAppearanceStruct = new()
        {
            _setRaceTag = playerAppearanceProfile._setRaceTag,
            _textureID = playerAppearanceProfile._setTexture,
            _bodyColorHue = playerAppearanceProfile._setSkinColorProfile._hue,
            _bodyColorBrightness = playerAppearanceProfile._setSkinColorProfile._brightness,
            _bodyColorContrast = playerAppearanceProfile._setSkinColorProfile._contrast,
            _bodyColorSaturation = playerAppearanceProfile._setSkinColorProfile._saturation,
            _hairStyleHue = playerAppearanceProfile._hairColorProfile._hue,
            _hairStyleBrightness = playerAppearanceProfile._hairColorProfile._brightness,
            _hairStyleContrast = playerAppearanceProfile._hairColorProfile._contrast,
            _hairStyleSaturation = playerAppearanceProfile._hairColorProfile._saturation,
            _hairIsBodyColor = playerAppearanceProfile._hairIsBodyColor,
            _miscHue = playerAppearanceProfile._miscColorProfile._hue,
            _miscBrightness = playerAppearanceProfile._miscColorProfile._brightness,
            _miscContrast = playerAppearanceProfile._miscColorProfile._contrast,
            _miscSaturation = playerAppearanceProfile._miscColorProfile._saturation,
            _dyeIndex = playerAppearanceProfile._helmDyeIndex,
            _headWidth = playerAppearanceProfile._headWidth,
            _muzzleWeight = playerAppearanceProfile._muzzleWeight,
            _hairStyleID = playerAppearanceProfile._setHairStyle,
            _miscID = playerAppearanceProfile._setMisc,
            _mouthID = playerAppearanceProfile._setMouth,
            _earID = playerAppearanceProfile._setEar,
            _eyeID = playerAppearanceProfile._setEye,
            _voicePitch = playerAppearanceProfile._voicePitch,
            _heightWeight = playerAppearanceProfile._heightWeight,
            _widthWeight = playerAppearanceProfile._widthWeight,
            _torsoWeight = playerAppearanceProfile._torsoWeight,
            _displayBoobs = playerAppearanceProfile._displayBoobs,
            _boobWeight = playerAppearanceProfile._boobWeight,
            _armWeight = playerAppearanceProfile._armWeight,
            _bellyWeight = playerAppearanceProfile._bellyWeight,
            _bottomWeight = playerAppearanceProfile._bottomWeight,
            _isLeftHanded = playerAppearanceProfile._isLeftHanded,
            _tailID = playerAppearanceProfile._setTail,
            _hideHelm = playerAppearanceProfile._hideHelm,
            _hideCape = playerAppearanceProfile._hideCape,
            _hideChest = playerAppearanceProfile._hideChest,
            _hideLeggings = playerAppearanceProfile._hideLeggings,
            _hideVanityHelm = playerAppearanceProfile._hideVanityHelm,
            _hideVanityCape = playerAppearanceProfile._hideVanityCape,
            _hideVanityChest = playerAppearanceProfile._hideVanityChest,
            _hideVanityLeggings = playerAppearanceProfile._hideVanityLeggings
        };

        playerVisual.Cmd_SendNew_PlayerAppearanceStruct(PlayerAppearanceStruct);
    }

    private void OnPluginUnload()
    {
        Atlyss.Game.Patches.Player.OnStartAuthority.OnPostfix -= OnStartAuthority;
        Atlyss.Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix -= OnStopClient;

        DeregisterHotkeys();
        SetPlayerRaceParamsCheck(false);
        DisableFirstCharacterDisplayApplication();
    }

    private void ReconfigurePlayerRaceParamsCheck() =>
        SetPlayerRaceParamsCheck(!pluginConfiguration.PlayerAppearance.AllowRaceParametersBeyondLimits.Value);

    private void SetPlayerRaceParamsCheck(bool state)
    {
        if (lastPlayerRaceParamsCheckState == state)
            return;

        lastPlayerRaceParamsCheckState = state;

        if (state)
            Atlyss.Game.Patches.ScriptablePlayerRace.Init_ParamsCheck.OnPrefix -= PreventPlayerRaceParamsCheck;
        else
            Atlyss.Game.Patches.ScriptablePlayerRace.Init_ParamsCheck.OnPrefix += PreventPlayerRaceParamsCheck;
    }

    private void PreventPlayerRaceParamsCheck(PlayerAppearance_Profile appearanceProfile, ref bool runOriginal) =>
        runOriginal = false;

    private IEnumerator DelayedPlayerAppearanceApplication()
    {
        yield return new WaitForSeconds(0.25f);

        isPlayerAppearanceApplicationPending = false;

        if (!CurrentPlayer)
            yield break;

        CurrentPlayer._pVisual.Cmd_SendNew_PlayerAppearanceStruct(CurrentPlayerVisual._playerAppearanceStruct);
    }

    private void ApplyCurrentPlayerAppearanceStruct()
    {
        CurrentPlayer._pVisual.Apply_NetworkedCharacterDisplay();

        //CurrentPlayer._pVisual.Send

        if (CurrentPlayer._isHostPlayer)
            return;

        if (isPlayerAppearanceApplicationPending)
            return;

        isPlayerAppearanceApplicationPending = true;

        Main.Instance.StartCoroutine(DelayedPlayerAppearanceApplication());
    }

    public void ModifyBreastSize(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._boobWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._boobWeight = CurrentPlayerVisual._playerAppearanceStruct._boobWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyArmSize(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._armWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._armWeight = CurrentPlayerVisual._playerAppearanceStruct._armWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyBellySize(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._bellyWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._bellyWeight = CurrentPlayerVisual._playerAppearanceStruct._bellyWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyBottomSize(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._bottomWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._bottomWeight = CurrentPlayerVisual._playerAppearanceStruct._bottomWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyHeight(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._heightWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._heightWeight = CurrentPlayerVisual._playerAppearanceStruct._heightWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyWidth(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._widthWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._widthWeight = CurrentPlayerVisual._playerAppearanceStruct._widthWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyTorsoSize(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._torsoWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._torsoWeight = CurrentPlayerVisual._playerAppearanceStruct._torsoWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyVoicePitch(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._voicePitch += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._voicePitch = CurrentPlayerVisual._playerAppearanceStruct._voicePitch;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyMuzzleLength(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._muzzleWeight += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._muzzleWeight = CurrentPlayerVisual._playerAppearanceStruct._muzzleWeight;

        ApplyCurrentPlayerAppearanceStruct();
    }

    public void ModifyHeadSize(float delta, bool updateCharacterFile)
    {
        CurrentPlayerVisual._playerAppearanceStruct._headWidth += delta;

        if (updateCharacterFile)
            CurrentPlayerAppearanceProfile._headWidth = CurrentPlayerVisual._playerAppearanceStruct._headWidth;

        ApplyCurrentPlayerAppearanceStruct();
    }

    private void OnModSettingsTabInitialized(SettingsTab settingsTab)
    {
        NessieEasySettings nessieEasySettings = plugin.Managers.NessieEasySettings;

        Types.Configuration.Sections.Hotkeys hotkeysSection = Configuration.Instance.Hotkeys;
        Types.Configuration.Sections.PlayerAppearance playerAppearanceSection = Configuration.Instance.PlayerAppearance;
        Types.Configuration.Sections.RaceParametersOverride raceParametersOverrideSection = Configuration.Instance.RaceParameters;

        settingsTab.AddHeader("Player appearance");

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddToggle("Allow race parameters beyond limits", playerAppearanceSection.AllowRaceParametersBeyondLimits),
            OnSettingsUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddToggle("Hotkeys affect character save", playerAppearanceSection.HotkeysAffectCharacterSave),
            OnSettingsUpdated
        );

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Head width • Decrease", hotkeysSection.ModifyHeadWidth.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Head width • Increase", hotkeysSection.ModifyHeadWidth.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider headWidthSlider =
            settingsTab.AddAdvancedSlider(
                "Head width • Step",
                hotkeysSection.ModifyHeadWidth.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.HeadWidth.Maximum.Value
            );

        headWidthSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyHeadWidth.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(headWidthSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Muzzle length • Decrease", hotkeysSection.ModifyMuzzleLength.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Muzzle length • Increase", hotkeysSection.ModifyMuzzleLength.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider muzzleLengthSlider =
            settingsTab.AddAdvancedSlider(
                "Muzzle length • Step",
                hotkeysSection.ModifyMuzzleLength.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.MuzzleLength.Maximum.Value
            );

        muzzleLengthSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyMuzzleLength.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(muzzleLengthSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Voice pitch • Decrease", hotkeysSection.ModifyVoicePitch.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Voice pitch • Increase", hotkeysSection.ModifyVoicePitch.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider voicePitchSlider =
            settingsTab.AddAdvancedSlider(
                "Voice pitch • Step",
                hotkeysSection.ModifyVoicePitch.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.VoicePitch.Maximum.Value
            );

        voicePitchSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyVoicePitch.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(voicePitchSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Height • Decrease", hotkeysSection.ModifyHeight.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Height • Increase", hotkeysSection.ModifyHeight.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider heightSlider =
            settingsTab.AddAdvancedSlider(
                "Height • Step",
                hotkeysSection.ModifyHeight.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.Height.Maximum.Value
            );

        heightSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyHeight.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(heightSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Width • Decrease", hotkeysSection.ModifyWidth.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Width • Increase", hotkeysSection.ModifyWidth.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider widthSlider =
            settingsTab.AddAdvancedSlider(
                "Width • Step",
                hotkeysSection.ModifyWidth.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.Width.Maximum.Value
            );

        widthSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyWidth.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(widthSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Torso size • Decrease", hotkeysSection.ModifyTorsoSize.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Torso size • Increase", hotkeysSection.ModifyTorsoSize.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider torsoSizeSlider =
            settingsTab.AddAdvancedSlider(
                "Torso size • Step",
                hotkeysSection.ModifyTorsoSize.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.TorsoSize.Maximum.Value
            );

        torsoSizeSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyTorsoSize.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(torsoSizeSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Breast size • Decrease", hotkeysSection.ModifyBreastSize.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Breast size • Increase", hotkeysSection.ModifyBreastSize.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider breastSizeSlider =
            settingsTab.AddAdvancedSlider(
                "Breast size • Step",
                hotkeysSection.ModifyBreastSize.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.BreastSize.Maximum.Value
            );

        breastSizeSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyBreastSize.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(breastSizeSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Arm size • Decrease", hotkeysSection.ModifyArmSize.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Arm size • Increase", hotkeysSection.ModifyArmSize.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider armSizeSlider =
            settingsTab.AddAdvancedSlider(
                "Arm size • Step",
                hotkeysSection.ModifyArmSize.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.ArmSize.Maximum.Value
            );

        armSizeSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyArmSize.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(armSizeSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Belly size • Decrease", hotkeysSection.ModifyBellySize.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Belly size • Increase", hotkeysSection.ModifyBellySize.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider bellySizeSlider =
            settingsTab.AddAdvancedSlider(
                "Belly size • Step",
                hotkeysSection.ModifyBellySize.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.BellySize.Maximum.Value
            );

        bellySizeSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyBellySize.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(bellySizeSlider, OnHotkeysUpdated);

        settingsTab.AddSpace();

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Bottom size • Decrease", hotkeysSection.ModifyBottomSize.Decrease),
            OnHotkeysUpdated
        );

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddKeyButton("Bottom size • Increase", hotkeysSection.ModifyBottomSize.Increase),
            OnHotkeysUpdated
        );

        AtlyssAdvancedSlider bottomSizeSlider =
            settingsTab.AddAdvancedSlider(
                "Bottom size • Step",
                hotkeysSection.ModifyBottomSize.Step.Value,
                0,
                raceParametersOverrideSection.BaseOverrideItem.BottomSize.Maximum.Value
            );

        bottomSizeSlider.OnValueChanged.AddListener(x => hotkeysSection.ModifyBottomSize.Step.Value = (float)Math.Round(x, 2));
        nessieEasySettings.AddPendingUpdateListener(bottomSizeSlider, OnHotkeysUpdated);
    }

    private void IncreaseHeadWidthHotkey() =>
        ModifyHeadSize(ConfigurationHotkeysSection.ModifyHeadWidth.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseHeadWidthHotkey() =>
        ModifyHeadSize(-ConfigurationHotkeysSection.ModifyHeadWidth.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseMuzzleLengthHotkey() =>
        ModifyMuzzleLength(ConfigurationHotkeysSection.ModifyMuzzleLength.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseMuzzleLengthHotkey() =>
        ModifyMuzzleLength(-ConfigurationHotkeysSection.ModifyMuzzleLength.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseVoicePitchHotkey() =>
        ModifyVoicePitch(ConfigurationHotkeysSection.ModifyVoicePitch.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseVoicePitchHotkey() =>
        ModifyVoicePitch(-ConfigurationHotkeysSection.ModifyVoicePitch.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseHeightHotkey() =>
        ModifyHeight(ConfigurationHotkeysSection.ModifyHeight.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseHeightHotkey() =>
        ModifyHeight(-ConfigurationHotkeysSection.ModifyHeight.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseWidthHotkey() =>
        ModifyWidth(ConfigurationHotkeysSection.ModifyWidth.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseWidthHotkey() =>
        ModifyWidth(-ConfigurationHotkeysSection.ModifyWidth.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseTorsoSizeHotkey() =>
        ModifyTorsoSize(ConfigurationHotkeysSection.ModifyTorsoSize.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseTorsoSizeHotkey() =>
        ModifyTorsoSize(-ConfigurationHotkeysSection.ModifyTorsoSize.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseBreastSizeHotkey() =>
        ModifyBreastSize(ConfigurationHotkeysSection.ModifyBreastSize.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseBreastSizeHotkey() =>
        ModifyBreastSize(-ConfigurationHotkeysSection.ModifyBreastSize.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseArmSizeHotkey() =>
        ModifyArmSize(ConfigurationHotkeysSection.ModifyArmSize.Step.Value, HotkeysAffectCharacterSave);

    private void DecreaseArmSizeHotkey() =>
        ModifyArmSize(-ConfigurationHotkeysSection.ModifyArmSize.Step.Value, HotkeysAffectCharacterSave);

    private void IncreaseBellySizeHotkey() =>
        ModifyBellySize(ConfigurationHotkeysSection.ModifyBellySize.Step.Value, pluginConfiguration.PlayerAppearance.HotkeysAffectCharacterSave.Value);

    private void DecreaseBellySizeHotkey() =>
        ModifyBellySize(-ConfigurationHotkeysSection.ModifyBellySize.Step.Value, pluginConfiguration.PlayerAppearance.HotkeysAffectCharacterSave.Value);

    private void IncreaseBottomSizeHotkey() =>
        ModifyBottomSize(ConfigurationHotkeysSection.ModifyBottomSize.Step.Value, pluginConfiguration.PlayerAppearance.HotkeysAffectCharacterSave.Value);

    private void DecreaseBottomSizeHotkey() =>
        ModifyBottomSize(-ConfigurationHotkeysSection.ModifyBottomSize.Step.Value, pluginConfiguration.PlayerAppearance.HotkeysAffectCharacterSave.Value);

    internal void RegisterHotkeys()
    {
        Types.Configuration.Sections.Hotkeys hotkeysSection = pluginConfiguration.Hotkeys;

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyHeadWidth.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseHeadWidthHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyHeadWidth.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseHeadWidthHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyMuzzleLength.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseMuzzleLengthHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyMuzzleLength.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseMuzzleLengthHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyVoicePitch.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseVoicePitchHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyVoicePitch.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseVoicePitchHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyHeight.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseHeightHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyHeight.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseHeightHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyWidth.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseWidthHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyWidth.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseWidthHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyTorsoSize.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseTorsoSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyTorsoSize.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseTorsoSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyBreastSize.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseBreastSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyBreastSize.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseBreastSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyArmSize.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseArmSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyArmSize.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseArmSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyBellySize.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseBellySizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyBellySize.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseBellySizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyBottomSize.Decrease.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            DecreaseBottomSizeHotkey
        );

        HotkeyManager.Register(
            [
                new(hotkeysSection.ModifyBottomSize.Increase.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            IncreaseBottomSizeHotkey
        );
    }

    internal void DeregisterHotkeys()
    {
        HotkeyManager.Deregister(IncreaseHeadWidthHotkey);
        HotkeyManager.Deregister(DecreaseHeadWidthHotkey);
        HotkeyManager.Deregister(IncreaseMuzzleLengthHotkey);
        HotkeyManager.Deregister(DecreaseMuzzleLengthHotkey);
        HotkeyManager.Deregister(IncreaseVoicePitchHotkey);
        HotkeyManager.Deregister(DecreaseVoicePitchHotkey);
        HotkeyManager.Deregister(IncreaseHeightHotkey);
        HotkeyManager.Deregister(DecreaseHeightHotkey);
        HotkeyManager.Deregister(IncreaseWidthHotkey);
        HotkeyManager.Deregister(DecreaseWidthHotkey);
        HotkeyManager.Deregister(IncreaseTorsoSizeHotkey);
        HotkeyManager.Deregister(DecreaseTorsoSizeHotkey);
        HotkeyManager.Deregister(IncreaseBreastSizeHotkey);
        HotkeyManager.Deregister(DecreaseBreastSizeHotkey);
        HotkeyManager.Deregister(IncreaseArmSizeHotkey);
        HotkeyManager.Deregister(DecreaseArmSizeHotkey);
        HotkeyManager.Deregister(IncreaseBellySizeHotkey);
        HotkeyManager.Deregister(DecreaseBellySizeHotkey);
        HotkeyManager.Deregister(IncreaseBottomSizeHotkey);
        HotkeyManager.Deregister(DecreaseBottomSizeHotkey);
    }
}

using Nessie.ATLYSS.EasySettings;
using System;

namespace Tanuki.Atlyss.FluffUtilities.Managers.Game.Controllers;

public sealed class PlayerAppearance
{
    private readonly Configuration configuration = Configuration.Instance;
    private readonly Main plugin;

    private bool lastPlayerRaceParamsCheckState = true;

    internal PlayerAppearance()
    {
        plugin = Main.Instance;

        plugin.OnLoaded += OnPluginLoaded;
        plugin.OnUnload += OnPluginUnload;

        plugin.OnInitialized += OnPluginInitialized;
    }

    private void OnPluginInitialized() =>
        plugin.Managers.NessieEasySettings.OnModSettingsTabInitialized += OnModSettingsTabInitialized;

    private void OnSettingsUpdated() =>
        ReconfigurePlayerRaceParamsCheck();

    private void OnModSettingsTabInitialized(SettingsTab settingsTab)
    {
        NessieEasySettings nessieEasySettings = plugin.Managers.NessieEasySettings;

        Types.Configuration.Sections.Hotkeys hotkeysSection = Configuration.Instance.Hotkeys;
        Types.Configuration.Sections.PlayerAppearance playerAppearanceSection = Configuration.Instance.PlayerAppearance;

        settingsTab.AddHeader("Player appearance");

        nessieEasySettings.AddPendingUpdateListener(
            settingsTab.AddToggle("Allow race parameters beyond limits", playerAppearanceSection.AllowRaceParametersBeyondLimits),
            OnSettingsUpdated
        );
    }

    private void OnPluginLoaded()
    {
        ReconfigurePlayerRaceParamsCheck();

        Atlyss.Game.Patches.Player.OnStartAuthority.OnPostfix += OnStartAuthority;
        Atlyss.Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;
    }

    private void OnStopClient() => DisableFirstCharacterDisplayApplication();

    private void OnStartAuthority(Player player)
    {
        if (!AtlyssNetworkManager._current.isNetworkActive)
            return;

        if (Player._mainPlayer._isHostPlayer)
            return;

        Atlyss.Game.Patches.PlayerVisual.Apply_NetworkedCharacterDisplay.OnPrefix += OnNetworkedCharacterDisplayAppliedFirstTime;
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

        SetPlayerRaceParamsCheck(false);

        DisableFirstCharacterDisplayApplication();
    }

    private void ReconfigurePlayerRaceParamsCheck() =>
        SetPlayerRaceParamsCheck(!configuration.PlayerAppearance.AllowRaceParametersBeyondLimits.Value);

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
}

using BepInEx;
using System.Collections;
using Tanuki.Atlyss.FluffUtilities.Managers;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities;

[BepInPlugin("cc8615a7-47a4-4321-be79-11e36887b64a", "Tanuki.Atlyss.FluffUtilities", "1.0.4")]
[BepInDependency("9c00d52e-10b8-413f-9ee4-bfde81762442", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("EasySettings", BepInDependency.DependencyFlags.HardDependency)]
[BepInProcess("ATLYSS.exe")]
public class Main : Core.Plugins.Plugin
{
    internal static Main Instance;
    private bool Reloaded = false;
    private bool UsageAnnounced = true;

    internal void Awake()
    {
        Instance = this;
        Configuration.Initialize();
        Configuration.Instance.Load(Config);

        PlayerAppearance.Initialize();
        GlobalRaceDisplayParameters.Initialize();
        FreeCamera.Initialize();
        Hotkey.Initialize();
        NessieEasySettings.Initialize();
    }
    protected override void Load()
    {
        Logger.LogInfo("Tanuki.Atlyss.FluffUtilities by Timofey Tanuki / tanu.su");

        if (Reloaded)
        {
            Config.Reload();
            Configuration.Instance.Load(Config);
        }

        Game.Main.Instance.Patch(
            typeof(Game.Events.ItemObject.Enable_GroundCheckToVelocityZero_Postfix),
            typeof(Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix),
            typeof(Game.Events.AtlyssNetworkManager.OnStopClient_Prefix),
            typeof(Game.Events.Player.OnStartAuthority_Postfix)
        );

        PlayerAppearance.Instance.Load();
        GlobalRaceDisplayParameters.Instance.Load();
        FreeCamera.Instance.Reload();

        Game.Events.Player.OnStartAuthority_Postfix.OnInvoke += OnStartAuthority_Postfix_OnInvoke;
        Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke += Init_LoadScreenDisable_Postfix_OnInvoke;

        ReloadHotkeys();
    }
    internal void ReloadHotkeys()
    {
        Hotkey.Instance.Reset();

        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeadWidth(
                    Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeadWidth(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_HeadWidth.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyMuzzleLength(
                    Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyMuzzleLength(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_MuzzleLength.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_Height.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeight(
                    Configuration.Instance.Hotkeys.PlayerAppearance_Height.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_Height.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_Height.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeight(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_Height.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_Height.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_Width.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyWidth(
                    Configuration.Instance.Hotkeys.PlayerAppearance_Width.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_Width.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_Width.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyWidth(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_Width.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_Width.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyTorsoSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyTorsoSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_TorsoSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBreastSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBreastSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_BreastSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyArmsSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyArmsSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_ArmsSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBellySize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBellySize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_BellySize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBottomSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBottomSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_BottomSize.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyVoicePitch(
                    Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyVoicePitch(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.Step.Value,
                    Configuration.Instance.Hotkeys.PlayerAppearance_VoicePitch.UpdateCharacterFile.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.FreeCamera_Toggle_Default.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                if (FreeCamera.Instance.Status && FreeCamera.Instance.CharacterControlsDisabled)
                    FreeCamera.Instance.Disable();
                else
                    FreeCamera.Instance.Enable(true);
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.FreeCamera_Toggle_WithControls.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                if (FreeCamera.Instance.Status && !FreeCamera.Instance.CharacterControlsDisabled)
                    FreeCamera.Instance.Disable();
                else
                    FreeCamera.Instance.Enable(false);
            }
        );

        Hotkey.Instance.Reload();
    }
    private void Init_LoadScreenDisable_Postfix_OnInvoke()
    {
        if (UsageAnnounced)
            return;

        if (Player._mainPlayer._isHostPlayer)
            return;

        UsageAnnounced = true;

        StartCoroutine(AnnounceUsage());
    }
    private void OnStartAuthority_Postfix_OnInvoke() =>
        UsageAnnounced = false;
    private IEnumerator AnnounceUsage()
    {
        yield return new WaitForSeconds(0.1f);
        Player._mainPlayer._pVisual.Cmd_PoofSmokeEffect();
        yield return new WaitForSeconds(0.2f);
        Player._mainPlayer._pVisual.Cmd_VanitySparkleEffect();
        yield return new WaitForSeconds(0.2f);
        Player._mainPlayer._pVisual.Cmd_PlayTeleportEffect();
    }
    protected override void Unload()
    {
        Reloaded = true;

        Game.Events.Player.OnStartAuthority_Postfix.OnInvoke -= OnStartAuthority_Postfix_OnInvoke;
        Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke -= Init_LoadScreenDisable_Postfix_OnInvoke;

        PlayerAppearance.Instance.Unload();
        GlobalRaceDisplayParameters.Instance.Unload();
        Hotkey.Instance.Reset();
    }
}
using BepInEx;
using System.Collections;
using Tanuki.Atlyss.FluffUtilities.Managers;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities;

[BepInPlugin("cc8615a7-47a4-4321-be79-11e36887b64a", "Tanuki.Atlyss.FluffUtilities", "1.0.19")]
[BepInDependency("9c00d52e-10b8-413f-9ee4-bfde81762442", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("EasySettings", BepInDependency.DependencyFlags.HardDependency)]
public class Main : Core.Plugins.Plugin
{
    internal static Main Instance;

    private bool Reloaded = false;
    internal Patching.Patcher Patcher = new();

    internal void Awake()
    {
        Instance = this;
        Configuration.Initialize();
        Configuration.Instance.Load(Config);

        Lobby.Initialize();
        PlayerAppearance.Initialize();
        GlobalRaceDisplayParameters.Initialize();
        FreeCamera.Initialize();
        NoClip.Initialize();
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

        Patcher.Use(
            typeof(Game.Events.ItemObject.Enable_GroundCheckToVelocityZero_Postfix),
            typeof(Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix),
            typeof(Game.Events.AtlyssNetworkManager.OnStopClient_Prefix),
            typeof(Game.Events.Player.OnStartAuthority_Postfix),
            typeof(Game.Events.PlayerMove.Client_LocalPlayerControl_Prefix)
        );

        PlayerAppearance.Instance.Load();
        GlobalRaceDisplayParameters.Instance.Load();
        Lobby.Instance.Load();
        NoClip.Instance.Reload();
        FreeCamera.Instance.Reload();

        if (Configuration.Instance.General.PresenceEffectsOnJoin.Value)
            Game.Events.Player.OnStartAuthority_Postfix.OnInvoke += OnStartAuthority_Postfix_OnInvoke;

        ReloadHotkeys();
    }
    internal void ReloadHotkeys()
    {
        Hotkey.Instance.Reset();

        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeadWidth(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeadWidth(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeadWidth.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyMuzzleLength(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyMuzzleLength(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyMuzzleLength.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeight(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyHeight(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyHeight.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyWidth(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyWidth(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyWidth.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyTorsoSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyTorsoSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyTorsoSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBreastSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBreastSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBreastSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyArmsSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyArmsSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyArmsSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBellySize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBellySize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBellySize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBottomSize(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyBottomSize(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyBottomSize.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Increase.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyVoicePitch(
                    Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Decrease.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                PlayerAppearance.Instance.ModifyVoicePitch(
                    -Configuration.Instance.Hotkeys.PlayerAppearance_ModifyVoicePitch.Step.Value,
                    Configuration.Instance.PlayerAppearance.HotkeysUpdateCharacterSave.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.FreeCamera.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                if (FreeCamera.Instance.Status)
                {
                    FreeCamera.Instance.Disable();
                    return;
                }

                FreeCamera.Instance.Enable(
                    Configuration.Instance.FreeCamera.LockCharacterControls.Value,
                    Configuration.Instance.FreeCamera.SmoothLookMode.Value
                );
            }
        );
        Hotkey.Instance.BindAction(
            Configuration.Instance.Hotkeys.NoClip_Toggle.Value,
            delegate
            {
                if (!Player._mainPlayer)
                    return;

                if (NoClip.Instance.Status)
                    NoClip.Instance.Disable();
                else
                    NoClip.Instance.Enable();
            }
        );
    }
    private void Init_LoadScreenDisable_Postfix_OnInvoke()
    {
        Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke -= Init_LoadScreenDisable_Postfix_OnInvoke;

        if (Player._mainPlayer._isHostPlayer)
            return;

        StartCoroutine(Plugin_ShowUsagePresenceOnJoinLobby_Effects());
    }
    private void OnStartAuthority_Postfix_OnInvoke()
    {
        if (Player._mainPlayer._isHostPlayer)
            return;

        Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke += Init_LoadScreenDisable_Postfix_OnInvoke;
    }
    private IEnumerator Plugin_ShowUsagePresenceOnJoinLobby_Effects()
    {
        yield return new WaitForSeconds(0.25f);
        Player._mainPlayer._pVisual.Cmd_PoofSmokeEffect();
        yield return new WaitForSeconds(0.3f);
        Player._mainPlayer._pVisual.Cmd_PoofSmokeEffect();
        yield return new WaitForSeconds(0.3f);
        Player._mainPlayer._pVisual.Cmd_VanitySparkleEffect();
    }
    protected override void Unload()
    {
        Reloaded = true;

        if (Configuration.Instance.General.PresenceEffectsOnJoin.Value)
        {
            Game.Events.Player.OnStartAuthority_Postfix.OnInvoke -= OnStartAuthority_Postfix_OnInvoke;
            Game.Events.LoadSceneManager.Init_LoadScreenDisable_Postfix.OnInvoke -= Init_LoadScreenDisable_Postfix_OnInvoke;
        }

        StopAllCoroutines();
        Lobby.Instance.Unload();
        PlayerAppearance.Instance.Unload();
        GlobalRaceDisplayParameters.Instance.Unload();
        Hotkey.Instance.Reset();

        Patcher.UnuseAll();
    }
}
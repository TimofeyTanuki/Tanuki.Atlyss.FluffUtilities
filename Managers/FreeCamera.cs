using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class FreeCamera
{
    public static FreeCamera Instance;
    private Components.FreeCamera Component;
    public bool Status = false;
    public bool CharacterControlsDisabled = false;
    public KeyCode Forward, Right, Backward, Left, Up, Down;
    public float Speed, ScrollSpeedAdjustmentStep;

    private FreeCamera() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;

    public static void Initialize() => Instance ??= new();
    public void Reload()
    {
        Forward = Configuration.Instance.Hotkeys.FreeCamera_Forward.Value;
        Right = Configuration.Instance.Hotkeys.FreeCamera_Right.Value;
        Backward = Configuration.Instance.Hotkeys.FreeCamera_Backward.Value;
        Left = Configuration.Instance.Hotkeys.FreeCamera_Left.Value;
        Up = Configuration.Instance.Hotkeys.FreeCamera_Up.Value;
        Down = Configuration.Instance.Hotkeys.FreeCamera_Down.Value;

        Speed = Configuration.Instance.FreeCamera.Speed.Value;
        ScrollSpeedAdjustmentStep = Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value;
    }
    public void Enable(bool DisableCharacterControls)
    {
        NoClip.Instance.Disable();

        Component ??= CameraFunction._current._mainCamera.gameObject.AddComponent<Components.FreeCamera>();

        if (!Status)
            Component.Speed = Speed;

        Status = true;

        CharacterControlsDisabled = DisableCharacterControls;
        if (DisableCharacterControls)
        {
            Player._mainPlayer._pMove.enabled = false;
            Player._mainPlayer._pCombat.enabled = false;
        }

        Component.Rotation = CameraFunction._current._mainCamera.transform.rotation.eulerAngles;
        Component.enabled = true;

        CameraFunction._current.enabled = false;
        CameraCollision._current.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Disable()
    {
        if (!Status)
            return;

        if (CharacterControlsDisabled)
        {
            Player._mainPlayer._pMove.enabled = true;
            Player._mainPlayer._pCombat.enabled = true;
            Player._mainPlayer._pCasting.Init_SkillLibrary();
        }

        CameraFunction._current.enabled = true;
        CameraCollision._current.enabled = true;

        CameraFunction._current._mainCamera.transform.rotation = CameraFunction._current.transform.localRotation;
        CameraFunction._current.CameraReset_Lerp();

        Status = false;
        Component.enabled = false;
    }
    private void OnStopClient_Prefix_OnInvoke()
    {
        if (Status)
            Disable();

        if (!Component)
            return;

        Object.Destroy(Component);
        Component = null;
    }
}
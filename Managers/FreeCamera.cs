using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class FreeCamera
{
    public static FreeCamera Instance;
    private Components.FreeCamera Component;
    public KeyCode Forward, Right, Backward, Left, Up, Down;
    public float ScrollSpeedAdjustmentStep;
    public bool Status = false;
    private bool LockCharacterControls = false;

    private FreeCamera() =>
        Game.Patches.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;

    public static void Initialize() => Instance ??= new();

    public void Reload()
    {
        Forward = Configuration.Instance.Hotkeys.FreeCamera_Forward.Value;
        Right = Configuration.Instance.Hotkeys.FreeCamera_Right.Value;
        Backward = Configuration.Instance.Hotkeys.FreeCamera_Backward.Value;
        Left = Configuration.Instance.Hotkeys.FreeCamera_Left.Value;
        Up = Configuration.Instance.Hotkeys.FreeCamera_Up.Value;
        Down = Configuration.Instance.Hotkeys.FreeCamera_Down.Value;

        ScrollSpeedAdjustmentStep = Configuration.Instance.FreeCamera.ScrollSpeedAdjustmentStep.Value;
    }

    public void Enable(bool LockCharacterControls, bool SmoothLook)
    {
        NoClip.Instance.Disable();

        Component ??= CameraFunction._current._mainCamera.gameObject.AddComponent<Components.FreeCamera>();

        if (!Status)
        {

            Component.Speed = Configuration.Instance.FreeCamera.Speed.Value;
            Status = true;
        }


        this.LockCharacterControls = LockCharacterControls;
        if (LockCharacterControls)
            Game.Patches.PlayerMove.Client_LocalPlayerControl_Prefix.OnInvoke += Client_LocalPlayerControl_Prefix_OnInvoke;

        Component.SmoothLookMode = SmoothLook;
        if (SmoothLook)
            Component.SmoothLookModeInterpolation = Configuration.Instance.FreeCamera.SmoothLookModeInterpolation.Value;

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

        if (LockCharacterControls)
        {
            Game.Patches.PlayerMove.Client_LocalPlayerControl_Prefix.OnInvoke -= Client_LocalPlayerControl_Prefix_OnInvoke;
        }

        CameraFunction._current.enabled = true;
        CameraCollision._current.enabled = true;

        Status = false;
        Component.enabled = false;
    }

    private void Client_LocalPlayerControl_Prefix_OnInvoke(PlayerMove PlayerMove, ref bool ShouldAllow)
    {
        if (!PlayerMove.isLocalPlayer)
            return;

        ShouldAllow = false;
    }

    private void OnStopClient_Prefix_OnInvoke()
    {
        Disable();

        if (!Component)
            return;

        Object.Destroy(Component);
        Component = null;
    }
}
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class NoClip
{
    public static NoClip Instance;

    private Components.NoClip Component;
    public bool Status = false;
    public KeyCode Forward, Right, Backward, Left, Up, Down, AlternativeSpeedKey;
    public float Speed, AlternativeSpeed;

    private NoClip() =>
        Game.Events.AtlyssNetworkManager.OnStopClient_Prefix.OnInvoke += OnStopClient_Prefix_OnInvoke;

    public static void Initialize() => Instance = new();
    public void Reload()
    {
        Forward = Configuration.Instance.Hotkeys.NoClip_Forward.Value;
        Right = Configuration.Instance.Hotkeys.NoClip_Right.Value;
        Backward = Configuration.Instance.Hotkeys.NoClip_Backward.Value;
        Left = Configuration.Instance.Hotkeys.NoClip_Left.Value;
        Up = Configuration.Instance.Hotkeys.NoClip_Up.Value;
        Down = Configuration.Instance.Hotkeys.NoClip_Down.Value;

        Speed = Configuration.Instance.NoClip.Speed.Value;
        AlternativeSpeedKey = Configuration.Instance.Hotkeys.NoClip_AlternativeSpeedKey.Value;
        AlternativeSpeed = Configuration.Instance.NoClip.AlternativeSpeed.Value;
    }
    public void Enable()
    {
        if (FreeCamera.Instance.Status)
        {
            CameraCollision._current.transform.localPosition = CameraCollision._current.dollyDir * CameraCollision._current.distance;
            FreeCamera.Instance.Disable();
        }

        Component ??= Player._mainPlayer._pMove.transform.gameObject.AddComponent<Components.NoClip>();

        Status = true;

        Player._mainPlayer._pMove.enabled = false;
        CameraCollision._current.enabled = false;

        Component.enabled = true;
    }
    public void Disable()
    {
        if (!Status)
            return;

        Player._mainPlayer._pMove.enabled = true;
        CameraCollision._current.enabled = true;
        Player._mainPlayer._pCasting.Init_SkillLibrary();

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
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

public sealed class NoClip
{
    private bool status = false;
    private Components.NoClip? component;

    public bool Status => status;

    internal NoClip() =>
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;

    private void OnStopClient()
    {
        Disable();

        if (component is null)
            return;

        UnityEngine.Object.Destroy(component.gameObject);
        component = null;
    }

    public void Enable()
    {
        if (!Player._mainPlayer || status)
            return;

        if (component is null)
        {
            component = Player._mainPlayer.gameObject.AddComponent<Components.NoClip>();
            Reconfigure();
        }

        status = true;

        Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix += PreventLocalPlayerControl;
        Player._mainPlayer._pMove._playerController.enabled = false;
        CameraCollision._current.enabled = false;
        component.enabled = true;
    }

    public void Disable()
    {
        if (!Player._mainPlayer || !status)
            return;

        status = false;

        Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix -= PreventLocalPlayerControl;
        Player._mainPlayer._pMove._airTime = 0;
        Player._mainPlayer._pMove._playerController.enabled = true;
        CameraCollision._current.enabled = true;
        component?.enabled = false;
    }

    private void PreventLocalPlayerControl(PlayerMove instance, ref bool runOriginal)
    {
        if (!instance.isLocalPlayer)
            return;

        runOriginal = false;
    }

    public void Reconfigure()
    {
        if (component is null)
            return;

        Configuration configuration = Configuration.instance;

        Types.Configuration.Sections.NoClip noClipSection = configuration.NoClip;

        component.Forward = noClipSection.Move_Forward.Value;
        component.Right = noClipSection.Move_Right.Value;
        component.Backward = noClipSection.Move_Backward.Value;
        component.Left = noClipSection.Move_Left.Value;
        component.Up = noClipSection.Move_Up.Value;
        component.Down = noClipSection.Move_Down.Value;
        component.AlternativeSpeedKey = noClipSection.AlternativeSpeedKey.Value;

        component.BaseSpeed = noClipSection.BaseSpeed.Value;
        component.AlternativeBaseSpeed = noClipSection.AlternativeBaseSpeed.Value;
    }

    internal void Toggle()
    {
        if (status)
            Disable();
        else
            Enable();
    }
}

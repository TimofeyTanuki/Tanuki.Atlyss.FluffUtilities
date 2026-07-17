namespace Tanuki.Atlyss.FluffUtilities.Managers;

public sealed class FreeCamera
{
    private readonly Configuration pluginConfiguration;
    private Core.Managers.Hotkey HotkeyManager => Core.Tanuki.Instance.Managers.Hotkey;
    private Components.FreeCamera? FreeCameraComponent => Components.FreeCamera.GetOrCreate();
    public bool CurrentState => FreeCameraComponent?.enabled ?? false;

    internal FreeCamera() => pluginConfiguration = Configuration.Instance;

    internal void Reconfigure() => FreeCameraComponent?.Reconfigure();

    internal void RegisterHotkeys() =>
        HotkeyManager.Register(
            [
                new(pluginConfiguration.Hotkeys.FreeCamera_Toggle.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            ToggleState
        );

    internal void DeregisterHotkeys() => HotkeyManager.Deregister(ToggleState);

    public void SetState(bool state)
    {
        if (state)
            Main.Instance.Managers.NoClip.SetState(false);

        FreeCameraComponent?.enabled = state;
    }

    public void ToggleState() => SetState(!CurrentState);
}

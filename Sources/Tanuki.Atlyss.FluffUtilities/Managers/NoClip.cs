namespace Tanuki.Atlyss.FluffUtilities.Managers;

public sealed class NoClip
{
    private readonly Configuration pluginConfiguration;
    private Core.Managers.Hotkey HotkeyManager => Core.Tanuki.Instance.Managers.Hotkey;

    private Components.NoClip? NoClipComponent => Components.NoClip.GetOrCreate();
    public bool CurrentState => NoClipComponent?.enabled ?? false;

    internal NoClip() => pluginConfiguration = Configuration.Instance;

    internal void Reconfigure() => NoClipComponent?.Reconfigure();

    internal void RegisterHotkeys() =>
        HotkeyManager.Register(
            [
                new(pluginConfiguration.Hotkeys.NoClip_Toggle.Value, Core.Types.Managers.Hotkey.EKeyState.Pressed)
            ],
            ToggleState
        );

    internal void DeregisterHotkeys() => HotkeyManager.Deregister(ToggleState);

    public void SetState(bool state)
    {
        if (state)
            Main.Instance.Managers.FreeCamera.SetState(false);

        NoClipComponent?.enabled = state;
    }

    public void ToggleState() => SetState(!CurrentState);
}

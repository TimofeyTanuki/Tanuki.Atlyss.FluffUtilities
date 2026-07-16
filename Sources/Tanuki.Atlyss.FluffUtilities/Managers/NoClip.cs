namespace Tanuki.Atlyss.FluffUtilities.Managers;

public sealed class NoClip
{
    private Components.NoClip? NoClipComponent => Components.NoClip.GetOrCreate();
    public bool CurrentState => NoClipComponent?.enabled ?? false;

    internal NoClip() { }

    public void Reconfigure() => NoClipComponent?.Reconfigure();

    public void SetState(bool state) => NoClipComponent?.enabled = state;

    public void ToggleState() => SetState(!CurrentState);
}

namespace Tanuki.Atlyss.FluffUtilities.Types;

public sealed class Managers
{
    private FluffUtilities.Managers.NoClip noClip = null!;
    private FluffUtilities.Managers.FreeCamera freeCamera = null!;
    internal FluffUtilities.Managers.NessieEasySettings NessieEasySettings = null!;

    public FluffUtilities.Managers.NoClip NoClip { get => noClip; internal set => noClip = value; }
    public FluffUtilities.Managers.FreeCamera FreeCamera { get => freeCamera; internal set => freeCamera = value; }

    internal Managers() { }
}

namespace Tanuki.Atlyss.FluffUtilities.Types.Main;

public sealed class Managers
{
    private FluffUtilities.Managers.NoClip noClip = null!;
    private FluffUtilities.Managers.FreeCamera freeCamera = null!;
    private FluffUtilities.Managers.GameWorld.Manager gameWorld = null!;
    internal FluffUtilities.Managers.NessieEasySettings NessieEasySettings = null!;

    public FluffUtilities.Managers.NoClip NoClip { get => noClip; internal set => noClip = value; }
    public FluffUtilities.Managers.FreeCamera FreeCamera { get => freeCamera; internal set => freeCamera = value; }
    public FluffUtilities.Managers.GameWorld.Manager GameWorld { get => gameWorld; internal set => gameWorld = value; }

    internal Managers() { }
}

namespace Tanuki.Atlyss.FluffUtilities.Types.Main;

public sealed class Services
{
    private FluffUtilities.Services.ClientHello clientInfo = null!;

    public FluffUtilities.Services.ClientHello ClientInfo { get => clientInfo; internal set => clientInfo = value; }

    internal Services() { }
}

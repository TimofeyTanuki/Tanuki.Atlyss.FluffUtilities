namespace Tanuki.Atlyss.FluffUtilities.Packets;

public sealed class ClientHello : Network.Bases.JsonObjectPacket<ClientHello>
{
    public string Version = null!;
    public string Random = null!;
}

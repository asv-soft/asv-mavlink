namespace Asv.Mavlink;

public interface IMavlinkFieldWriter
{
    void Write(MavlinkFieldInfo field, object value);
}
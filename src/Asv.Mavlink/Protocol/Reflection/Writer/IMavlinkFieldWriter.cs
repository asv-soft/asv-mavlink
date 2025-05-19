using System;

namespace Asv.Mavlink;

public interface IMavlinkFieldWriter
{
    void Write(MavlinkFieldInfo field, object value);
}



public class CallbackMavlinkFieldWriter(Action<MavlinkFieldInfo, object> write) : IMavlinkFieldWriter
{
    private readonly Action<MavlinkFieldInfo,object> _write = write ?? throw new ArgumentNullException(nameof(write));

    public void Write(MavlinkFieldInfo field, object value)
    {
        _write(field, value);
    }
}
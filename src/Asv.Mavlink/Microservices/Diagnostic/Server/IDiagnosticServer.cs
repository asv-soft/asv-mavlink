using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink.Diagnostic.Server;

public interface IDiagnosticServer:IMavlinkMicroserviceServer
{
    bool IsEnabled { get; set; }
    Task Send(string name, float value,CancellationToken cancel = default);
    Task Send(string name, int value, CancellationToken cancel = default);
    ValueTask Send(string name, ushort arrayId, float[] data, CancellationToken cancel = default);
    ValueTask Send(ushort address, byte version, byte type, sbyte[] value,CancellationToken cancel = default);
}
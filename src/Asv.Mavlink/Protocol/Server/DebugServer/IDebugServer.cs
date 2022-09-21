using System.Threading.Tasks;

namespace Asv.Mavlink.Server
{
    public interface IDebugServer
    {
        int MaxDebugFloatArrayNameLength { get; }
        int MaxDebugFloatArrayDataLength { get; }
        int MaxMemoryVectLength { get; }
        int MaxNamedValueKeyLength { get; }

        Task SendDebugFloatArray(string name, ushort arrayId, float[] data);
        Task SendMemoryVect(ushort address, byte version, byte type, sbyte[] value);
        Task SendNamedValueFloat(string name, float value);
        Task SendNamedValueInteger(string name, int value);
    }
}
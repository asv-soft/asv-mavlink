using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public interface IServerRxParam<TValue> : IRxEditableValue<TValue>
    {
        void Write(TValue value);
    }
}

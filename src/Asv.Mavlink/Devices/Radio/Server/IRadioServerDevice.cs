using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;



public interface IRadioServerDevice
{
    void Start();
    IRxEditableValue<string> AudioName { get; }
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    IParamsServerEx Params { get; }
    IAudioService Audio { get; }
    IAsvRadioServerEx Radio { get; }
    
}

#nullable enable
namespace Asv.Mavlink;

public interface IRadioClientDevice:IClientDevice
{
    ICommandClient Command { get; }
    IParamsClientEx Params { get; }
    IAsvRadioClientEx Radio { get; }
}

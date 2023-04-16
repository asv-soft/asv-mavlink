namespace Asv.Mavlink;

public interface IGbsClientDevice:IClientDevice
{
    IParamsClientEx Params { get; }
    ICommandClient Command { get; }
    IAsvGbsExClient Gbs { get; }
}
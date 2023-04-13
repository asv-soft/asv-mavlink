namespace Asv.Mavlink;

public interface IGbsClientDevice:IClientDevice
{
    ICommandClient Command { get; }
    IAsvGbsExClient Gbs { get; }
}
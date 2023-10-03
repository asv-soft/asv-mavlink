namespace Asv.Mavlink;

public interface ISdrClientDevice:IClientDevice
{
    IAsvSdrClientEx Sdr { get; }
    ICommandClient Command { get; }
    IMissionClientEx Missions { get; }
    IParamsClientEx Params { get; }
}
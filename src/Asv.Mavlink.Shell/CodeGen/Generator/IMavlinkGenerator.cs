namespace Asv.Mavlink.Shell
{
    public interface IMavlinkGenerator
    {
        string Generate(string template, MavlinkProtocolModel model);
    }
}

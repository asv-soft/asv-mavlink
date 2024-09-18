using ManyConsole;

namespace Asv.Mavlink.Shell;

public class MavlinkV1V2Converter : ConsoleCommand
{
    private string _file = "adsb.json";

    public MavlinkV1V2Converter()
    {
        IsCommand("mav-conv", "Generate virtual ADSB Vehicle");
        HasOption("cfg=", $"File path with ADSB config. Will be created if not exist. Default '{_file}'",
            x => _file = x);

    }

    public override int Run(string[] remainingArguments)
    {
        return 0;
    }
}

public class MavlinkV1Encoder
{
    
}
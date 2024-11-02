using System.IO.Abstractions.TestingHelpers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpServerEx))]
public class FtpServerExTest(ITestOutputHelper log) : ServerTestBase<FtpServerEx>(log)
{
    private MockFileSystem _fs = new();
    private MavlinkFtpServerExConfig _config = new()
    {
        NetworkId = 0,
        BurstReadChunkDelayMs = 100,
        RootDirectory = "root"
    };

    protected override FtpServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new FtpServerEx(new FtpServer(identity,_config,core), _config,_fs );
    }
    
}
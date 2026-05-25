using System;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Server;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(DiagnosticServer))]
public class DiagnosticServerTest(ITestOutputHelper log) : ServerTestBase<DiagnosticServer>(log)
{
    private readonly DiagnosticServerConfig _config = new()
    {
        MaxSendIntervalMs = 100,
        IsEnabled = true
    };

    protected override DiagnosticServer CreateServer(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    [Fact]
    public async Task Server_TrySendNameLengthMoreThanMaxLength_Fail()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await Server.Send("1234567891011", (float)1.0, Xunit.TestContext.Current.CancellationToken));
    }

    [Fact]
    public void Server_TrySendWhenDisabled_Fail()
    {
        //Arrange
        Server.IsEnabled = false;
        var result = Server.Send("diagnostic", (float)1.0, Xunit.TestContext.Current.CancellationToken).IsCompleted;
        const float element = (float)1.0;
        float[] floatArr = [element, element, element, element];
        
        //Act
        var resultFloat = Server.Send("diagnostic", 1, Xunit.TestContext.Current.CancellationToken).IsCompleted;
        var resultFloatArray = Server.Send("name", 1, floatArr, Xunit.TestContext.Current.CancellationToken).IsCompleted;
        var resultByte = Server.Send(1, 1, 1, [1, 1, 1, 1], Xunit.TestContext.Current.CancellationToken).IsCompleted;
        
        //Assert
        Assert.True(result);
        Assert.True(resultFloat);
        Assert.True(resultFloatArray);
        Assert.True(resultByte);
        
    }
}

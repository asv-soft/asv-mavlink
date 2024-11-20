using System;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Server;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(DiagnosticServer))]
public class DiagnosticServerTest(ITestOutputHelper log) : ServerTestBase<DiagnosticServer>(log)
{
    private readonly DiagnosticServerConfig _config = new()
    {
        MaxSendIntervalMs = 100,
        IsEnabled = true
    };

    protected override DiagnosticServer CreateClient(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    [Fact]
    public async Task Server_TrySendNameLengthMoreThanMaxLength_Fail()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await Server.Send("1234567891011", (float)1.0));
    }

    [Fact]
    public void Server_TrySendWhenDisabled_Fail()
    {
        //Arrange
        Server.IsEnabled = false;
        var result = Server.Send("diagnostic", (float)1.0).IsCompleted;
        const float element = (float)1.0;
        float[] floatArr = [element, element, element, element];
        
        //Act
        var resultFloat = Server.Send("diagnostic", 1).IsCompleted;
        var resultFloatArray = Server.Send("name", 1, floatArr).IsCompleted;
        var resultByte = Server.Send(1, 1, 1, [1, 1, 1, 1]).IsCompleted;
        
        //Assert
        Assert.True(result);
        Assert.True(resultFloat);
        Assert.True(resultFloatArray);
        Assert.True(resultByte);
        
    }
}
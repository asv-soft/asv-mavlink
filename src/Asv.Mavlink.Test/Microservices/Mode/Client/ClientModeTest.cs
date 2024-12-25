using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;



[TestSubject(typeof(ModeClient))]
public class ClientModeTest(ITestOutputHelper log) : ClientTestBase<ModeClient>(log)
{
    private readonly CommandProtocolConfig _command = new()
    {
        CommandTimeoutMs = 10,
        CommandAttempt = 3
    };
    private readonly HeartbeatClientConfig _heartbeat = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 5,
        PrintStatisticsToLogDelayMs = 100,
        PrintLinkStateToLog = true
    };
    
    protected override ModeClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new ModeClient(new HeartbeatClient(identity, _heartbeat, core), new CommandClient(identity,_command, core), Px4Mode.Unknown,Px4Mode.AllModes);
    }

    [Fact]
    public void ClientMode_Ctor_NullArguments()
    {
        var conn = Protocol.Create(x => { }).CreateVirtualConnection();
        var core = new CoreServices(conn.Client);
        var identity = new MavlinkClientIdentity(1, 2, 3, 4);
        
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => new ModeClient(null,new CommandClient(identity, _command, core), Px4Mode.Unknown, Px4Mode.AllModes));
        Assert.Throws<ArgumentNullException>(() => new ModeClient(new HeartbeatClient(identity,_heartbeat,core),null, Px4Mode.Unknown, Px4Mode.AllModes));
        Assert.Throws<ArgumentNullException>(() => new ModeClient(new HeartbeatClient(identity,_heartbeat,core),new CommandClient(identity, _command, core), null, Px4Mode.AllModes));
        Assert.Throws<ArgumentNullException>(() => new ModeClient(new HeartbeatClient(identity,_heartbeat,core),new CommandClient(identity, _command, core), Px4Mode.Unknown, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            
    }

    [Fact]
    public async Task SetMode_Send_packet_with_normal_args_Success()
    {
        var client = Client;
        Assert.Equal(client.CurrentMode.CurrentValue, Px4Mode.Unknown);
        
        foreach (var mode in client.AvailableModes.Where(x=>x.InternalMode == false))
        {
            var tcs = new TaskCompletionSource<CommandLongPayload>();
            Link.Server.OnRxMessage.FilterByType<CommandLongPacket>().Subscribe(x =>
            {
                tcs.TrySetResult(x.Payload);
                Time.Advance(TimeSpan.FromMilliseconds(_command.CommandTimeoutMs * _command.CommandAttempt + 100));
            });
            await Assert.ThrowsAsync<TimeoutException>(() => client.SetMode(mode));
            
            await tcs.Task;
            mode.GetCommandLongArgs(out var baseMode, out var customMode, out var subMode); 
            Assert.Equal(tcs.Task.Result.Param1,baseMode);
            Assert.Equal(tcs.Task.Result.Param2,customMode);
            Assert.Equal(tcs.Task.Result.Param3,subMode);
            Assert.Equal(MavCmd.MavCmdDoSetMode,tcs.Task.Result.Command);
        }
    }
    
    [Fact]
    public async Task SetMode_Set_internal_mode_Fail()
    {
        var client = Client;
        Assert.Equal(client.CurrentMode.CurrentValue, Px4Mode.Unknown);
        
        foreach (var mode in client.AvailableModes.Where(x=>x.InternalMode == true))
        {
            await Assert.ThrowsAsync<NotSupportedException>(() => client.SetMode(mode));
        }
    }

    
}
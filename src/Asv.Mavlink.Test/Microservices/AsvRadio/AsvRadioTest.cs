using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Xunit;

namespace Asv.Mavlink.Test;

public class AsvRadioTest
{
   
    [Fact]
    public async Task Server_send_status_and_client_listen_it()
    {
        var link = new VirtualMavlinkConnection();
        var client = new AsvRadioClient(link.Client, new MavlinkClientIdentity(1,1,2,2),new PacketSequenceCalculator());
        var server = new AsvRadioServer(link.Server, new MavlinkIdentity(2,2), new AsvRadioServerConfig(), new PacketSequenceCalculator());
        
        var tcs = new TaskCompletionSource<AsvRadioStatusPayload>();
        var cancel = new CancellationTokenSource();
        cancel.CancelAfter(TimeSpan.FromSeconds(10));
        await using var c1 = cancel.Token.Register(()=>tcs.TrySetCanceled(), false);
        
        client.Status
            .Skip(3) // this is for skip first 3 messages
            .Subscribe(x =>
        {
            tcs.TrySetResult(x);
        });
        
        var origin = new AsvRadioStatusPayload
        {
            Freq = (uint)Random.Shared.Next(10_000_000, 1_000_000_000),
            TxLevel = Random.Shared.Next(-100, 10),
            RxEstimatedLevel = Random.Shared.Next(-100, 10),
            RfMode = AsvRadioRfModeFlag.AsvRadioRfModeFlagRxOnAir | AsvRadioRfModeFlag.AsvRadioRfModeFlagTxOnAir,
            RxLevel = Random.Shared.Next(-100, 10),
            Modulation = AsvRadioModulation.AsvRadioModulationAm,
        };
        
        server.Set(x=>
        {
            x.RxEstimatedLevel = origin.RxEstimatedLevel;
            x.Freq = origin.Freq;
            x.RfMode = origin.RfMode;
            x.Modulation = origin.Modulation;
            x.TxLevel = origin.TxLevel;
            x.RxLevel = origin.RxLevel;
        });
        server.Start();
        
        var result = await tcs.Task;
        Assert.Equal(origin.Freq, result.Freq);
        Assert.Equal(origin.TxLevel, result.TxLevel);
        Assert.Equal(origin.RxEstimatedLevel, result.RxEstimatedLevel);
        Assert.Equal(origin.RfMode, result.RfMode);
        Assert.Equal(origin.RxLevel, result.RxLevel);
        Assert.Equal(origin.Modulation, result.Modulation);
        
        
    }
    [Fact]
    public async Task Client_Request_Capabilities()
    {
        var link = new VirtualMavlinkConnection();
        var client = new AsvRadioClient(link.Client, new MavlinkClientIdentity(1,1,2,2),new PacketSequenceCalculator());
        var server = new AsvRadioServer(link.Server, new MavlinkIdentity(2,2), new AsvRadioServerConfig(), new PacketSequenceCalculator());
        


        var origin = new AsvRadioCapabilitiesResponsePayload
        {
            MaxRfFreq = (uint)Random.Shared.Next(10_000_000, 1_000_000_000),
            MinRfFreq = (uint)Random.Shared.Next(10_000_000, 1_000_000_000),
            MaxTxPower = Random.Shared.Next(-100, 10),
            MinTxPower = Random.Shared.Next(-100, 10),
            MaxRxPower = Random.Shared.Next(-100, 10),
            MinRxPower = Random.Shared.Next(-100, 10),
        };
        Random.Shared.NextBytes(origin.SupportedModulation);
        
        server.OnCapabilitiesRequest.Subscribe( x =>
        {
            Assert.Equal(client.Identity.TargetSystemId, x.TargetSystem);
            Assert.Equal(client.Identity.TargetComponentId, x.TargetComponent);
            server.SendCapabilitiesResponse(arg =>
            {
                origin.CopyTo(arg);
            });
        });
        var result = await client.RequestCapabilities();
        Assert.Equal(origin.MaxRfFreq, result.MaxRfFreq);
        Assert.Equal(origin.MinRfFreq, result.MinRfFreq);
        Assert.Equal(origin.MaxTxPower, result.MaxTxPower);
        Assert.Equal(origin.MinTxPower, result.MinTxPower);
        Assert.Equal(origin.MaxRxPower, result.MaxRxPower);
        Assert.Equal(origin.MinRxPower, result.MinRxPower);
        Assert.Equal(origin.SupportedModulation, result.SupportedModulation);
        
    }

    [Fact]
    public async Task Client_Request_CodecOptions()
    {
        var link = new VirtualMavlinkConnection();
        var client = new AsvRadioClient(link.Client, new MavlinkClientIdentity(1,1,2,2),new PacketSequenceCalculator());
        var server = new AsvRadioServer(link.Server, new MavlinkIdentity(2,2), new AsvRadioServerConfig(), new PacketSequenceCalculator());
        
        var origin = new AsvRadioCodecCapabilitiesResponsePayload
        {
            Skip = 0,
        };
        Enum.GetValues<AsvAudioCodec>().CopyTo(origin.Codecs,0);
        origin.Count = (byte)origin.Codecs.Length;
        origin.All = (byte)origin.Codecs.Length;
        
        server.OnCodecCapabilitiesRequest.Subscribe( x =>
        {
            Assert.Equal(client.Identity.TargetSystemId, x.TargetSystem);
            Assert.Equal(client.Identity.TargetComponentId, x.TargetComponent);
            server.SendCodecCapabilitiesRequest(arg =>
            {
                origin.CopyTo(arg);
            });
        });
        var result = await client.RequestCodecCapabilities(origin.Skip,origin.Count);
        foreach (var codec in origin.Codecs)
        {
            if (result.Codecs.Contains(codec))
            {
                continue;
            }
            Assert.True(false, $"Codec {codec} not found in result");
        }
        Assert.Equal(origin.Count, result.Count);
        Assert.Equal(origin.Skip, result.Skip);
        Assert.Equal(origin.All, result.All);
        
            
    }
}
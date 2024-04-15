using System;
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
        var server = new AsvRadioServer(link.Server, new MavlinkIdentity(2,2), new AsvRadioServerConfig(), new PacketSequenceCalculator(),Scheduler.Default);
        
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
            RfFreq = (uint)Random.Shared.Next(10_000_000, 1_000_000_000),
            TxPower = Random.Shared.Next(-100, 10),
            RxEstimatedPower = Random.Shared.Next(-100, 10),
            RfMode = AsvRadioRfModeFlag.AsvRadioRfModeFlagRxOnAir | AsvRadioRfModeFlag.AsvRadioRfModeFlagTxOnAir,
            RxPower = Random.Shared.Next(-100, 10),
            RfModulation = AsvRadioModulation.AsvRadioModulationAm | AsvRadioModulation.AsvRadioModulationFm,
            Codec = AsvAudioCodec.AsvAudioCodecUnknown | AsvAudioCodec.AsvAudioCodecOpus,
            CodecCfg = (byte)Random.Shared.Next(0, 255),
        };
        
        server.Set(x=>
        {
            x.RxEstimatedPower = origin.RxEstimatedPower;
            x.RfFreq = origin.RfFreq;
            x.RfMode = origin.RfMode;
            x.RfModulation = origin.RfModulation;
            x.TxPower = origin.TxPower;
            x.RxPower = origin.RxPower;
            x.Codec = origin.Codec;
            x.CodecCfg = origin.CodecCfg;
        });
        server.Start();
        
        var result = await tcs.Task;
        Assert.Equal(origin.RfModulation, result.RfModulation);
        Assert.Equal(origin.RfFreq, result.RfFreq);
        Assert.Equal(origin.TxPower, result.TxPower);
        Assert.Equal(origin.RxEstimatedPower, result.RxEstimatedPower);
        Assert.Equal(origin.RxPower, result.RxPower);
        Assert.Equal(origin.Codec, result.Codec);
        Assert.Equal(origin.CodecCfg, result.CodecCfg);
        
    }
    [Fact]
    public async Task Client_Request_Capabilities()
    {
        var link = new VirtualMavlinkConnection();
        var client = new AsvRadioClient(link.Client, new MavlinkClientIdentity(1,1,2,2),new PacketSequenceCalculator());
        var server = new AsvRadioServer(link.Server, new MavlinkIdentity(2,2), new AsvRadioServerConfig(), new PacketSequenceCalculator(),Scheduler.Default);
        


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
        var server = new AsvRadioServer(link.Server, new MavlinkIdentity(2,2), new AsvRadioServerConfig(), new PacketSequenceCalculator(),Scheduler.Default);
        
        var origin = new AsvRadioCodecCfgResponsePayload
        {
            TargetCodec = AsvAudioCodec.AsvAudioCodecOpus,
        };
        Random.Shared.NextBytes(origin.SupportedCfg);
        
        server.OnCodecCfgRequest.Subscribe( x =>
        {
            Assert.Equal(client.Identity.TargetSystemId, x.TargetSystem);
            Assert.Equal(client.Identity.TargetComponentId, x.TargetComponent);
            server.SendCodecCfgResponse(arg =>
            {
                origin.CopyTo(arg);
            });
        });
        var result = await client.RequestCodecOptions(AsvAudioCodec.AsvAudioCodecOpus);
        Assert.Equal(origin.TargetCodec, result.TargetCodec);
        Assert.Equal(origin.SupportedCfg, result.SupportedCfg);
            
    }
}
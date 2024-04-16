using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using DynamicData;
using DynamicData.Binding;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvAudioTest
{
    private readonly ITestOutputHelper _output;

    public AsvAudioTest(ITestOutputHelper output)
    {
        _output = output;
    }


    [Theory]
    [InlineData(1)]
    [InlineData(16)]
    [InlineData(127)]
    [InlineData(AsvAudioHelper.MaxPacketStreamData)]
    [InlineData(AsvAudioHelper.MaxPacketStreamData + 1)]
    [InlineData(256)]
    [InlineData(1024)]
    [InlineData(byte.MaxValue * AsvAudioHelper.MaxPacketStreamData)]
    public async Task All_devices_send_status(int dataLength)
    {
        var link = new VirtualMavlinkConnection();
        var rawPart = new RawCodecFactoryPart();
        var cfg = new AudioServiceConfig();
        var factory = new CompositeAudioCodecFactory(new []{rawPart});
        var device1 = new AudioService(factory, link.Client, new MavlinkIdentity(1,1), new PacketSequenceCalculator(), cfg);
        var device2 = new AudioService(factory, link.Server, new MavlinkIdentity(2,2), new PacketSequenceCalculator(), cfg);
        
        device1.GoOnline("device1", AsvAudioCodec.AsvAudioCodecRaw8000Mono , true, true);
        device2.GoOnline("device2", AsvAudioCodec.AsvAudioCodecRaw8000Mono, true, true);

        await Task.Delay(2000);
        
        using var subscribe1 = device1.Devices.BindToObservableList(out var device1List).Subscribe();
        using var subscribe2 = device2.Devices.BindToObservableList(out var device2List).Subscribe();
        
        Assert.Equal(1, device1List.Count);
        Assert.Equal(1, device2List.Count);
        var audio1 = device1List.Items.First();
        var audio2 = device2List.Items.First();

        var tcs = new TaskCompletionSource();
        var cancel = new CancellationTokenSource();
        cancel.CancelAfter(TimeSpan.FromSeconds(10));
        await using var c1 = cancel.Token.Register(()=>tcs.TrySetCanceled(), false);
        
        var data = new byte[dataLength];
        Random.Shared.NextBytes(data);
        
        device2.OnReceiveAudio += (device, bytes, dataSize) =>
        {
            Assert.Equal(data.Length, dataSize);
            for (var i = 0; i < dataSize; i++)
            {
                Assert.Equal(data[i], bytes[i]);    
            }
            tcs.SetResult();
        };
        
        await audio1.SendAudio(data, data.Length, default);
        await tcs.Task;
        
        
    }
}
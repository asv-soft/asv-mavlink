using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class RadioDeviceTest
{
    private void Create(IAudioCodecFactory factory, AsvRadioCapabilities cap, out IRadioClientDevice clientDev, out IRadioServerDevice serverDev)
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var clientSeq = new PacketSequenceCalculator();
        clientDev = new RadioClientDevice(factory, link.Client, clientId, clientSeq, new RadioClientDeviceConfig(), Scheduler.Default);
            
        var serverSeq = new PacketSequenceCalculator();
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        serverDev = new RadioServerDevice(link.Server,serverSeq, serverId,  new RadioServerDeviceConfig(),  Scheduler.Default, Array.Empty<IMavParamTypeMetadata>(), new MavParamByteWiseEncoding(), new InMemoryConfiguration(),factory,cap);
        serverDev.Start();
    }
    
    [Fact]
    public async Task Test1()
    {
        /*var cap = new AsvRadioCapabilitiesBuilder()
            .SetFrequencyHz(0,uint.MaxValue)
            .SetSupportedModulations(AsvRadioModulation.AsvRadioModulationAm,AsvRadioModulation.AsvRadioModulationFm)
            .SetReferencePowerDbm(-100,10)
            .SetTxPowerDbm(-100,10)
            .Build();
        IAudioCodecFactory factory = new RawCodecFactoryPart();
        Create(factory, cap,out var clientDev,out var serverDev);
        
        serverDev.Radio.EnableRadio = async (freq,modulation,refRxPower,txPower,codec, cancel) =>
        {
            Assert.Equal(0.0,freq);
            Assert.Equal(AsvRadioModulation.AsvRadioModulationAm,modulation);
            Assert.Equal(-100,refRxPower);
            Assert.Equal(-100,txPower);
            Assert.Equal(AsvAudioCodec.AsvAudioCodecRaw8000Mono,codec);
            await Task.Delay(1000, cancel);
            return MavResult.MavResultAccepted;
        };
        
        await clientDev.Radio.EnableRadio(0, AsvRadioModulation.AsvRadioModulationAm, -100, -100, AsvAudioCodec.AsvAudioCodecRaw8000Mono);

        await serverDev.Radio.CustomMode.FirstAsync(x => x == AsvRadioCustomMode.AsvRadioCustomModeOnair);
        */
        
        
    }
}
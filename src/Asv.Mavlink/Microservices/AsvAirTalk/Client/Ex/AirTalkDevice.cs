using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAirTalk;

namespace Asv.Mavlink;

public interface IAirTalkDevice
{
    ushort FullId { get; }
    Task SendAudio(byte[] pcmRawAudioData, int dataSize, CancellationToken cancel);
}

public class AirTalkDevice : DisposableOnceWithCancel, IAirTalkDevice
{
    private readonly Lazy<IAudioCodec,AsvAirTalkCodec> _codec;
    private readonly byte _systemId;
    private readonly byte _componentId;
    private readonly IAirTalkClient _client;

    public AirTalkDevice(Lazy<IAudioCodec,AsvAirTalkCodec> codec, byte systemId, byte componentId , IAirTalkClient client)
    {
        _codec = codec;
        _systemId = systemId;
        _componentId = componentId;
        _client = client;
        FullId = MavlinkHelper.ConvertToFullId(systemId,componentId);
    }
    
    public ushort FullId { get; }
   
    public async Task SendAudio(byte[] pcmRawAudioData, int dataSize, CancellationToken cancel)
    {
        var encodedData = ArrayPool<byte>.Shared.Rent(_codec.Value.MaxEncodedSize);
        try
        {
            _codec.Value.Encode(pcmRawAudioData,dataSize,encodedData,_codec.Value.MaxEncodedSize,out var encodedSize);
            var fullPackets = encodedSize / AsvAirTalkHelper.MaxPacketStreamData;
            var lastPacketSize = encodedSize % AsvAirTalkHelper.MaxPacketStreamData;
            byte packetIndex = 0;
            for (int i = 0; i < fullPackets; i++)
            {
                await _client.SendAudioStream(_ =>
                {
                   
                    _.Payload.SequenceNumber = packetIndex;
                    _.Payload.DataSzie = AsvAirTalkHelper.MaxPacketStreamData;
                    Array.Copy(encodedData,packetIndex * AsvAirTalkHelper.MaxPacketStreamData,_.Payload.Data,0,AsvAirTalkHelper.MaxPacketStreamData);
                }, cancel);
                packetIndex++;
            }
            if (lastPacketSize > 0)
            {
                await _client.SendAudioStream(_ =>
                {
                  
                    _.Payload.SequenceNumber = packetIndex;
                    _.Payload.DataSzie = (byte)lastPacketSize;
                    Array.Copy(encodedData,packetIndex * AsvAirTalkHelper.MaxPacketStreamData,_.Payload.Data,0,lastPacketSize);
                }, cancel);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(encodedData);            
        }
        
    }
}
using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class DebugServer: MavlinkMicroserviceServer, IDebugServer
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkIdentity _identity;
        private readonly int _maxDebugFloatArrayNameLength = new DebugFloatArrayPayload().Name.Length;
        private readonly int _maxDebugFloatArrayDataLength = new DebugFloatArrayPayload().Data.Length;
        private readonly int _maxMemoryVectLength = new MemoryVectPayload().Value.Length;
        private readonly int _maxNamedValueKeyLength = new NamedValueFloatPayload().Name.Length;
        private readonly DateTime _bootTime;

        public DebugServer(
            IMavlinkV2Connection connection,
            IPacketSequenceCalculator seq,
            MavlinkIdentity identity,
            TimeProvider? timeProvider = null,
            IScheduler? rxScheduler = null,
            ILoggerFactory? logFactory = null) 
            : base("DEBUG", connection, identity, seq, timeProvider, rxScheduler,logFactory)
        {
            _connection = connection;
            _seq = seq;
            _identity = identity;
            _bootTime = DateTime.Now;
        }

        public int MaxDebugFloatArrayNameLength => _maxDebugFloatArrayNameLength;
        public int MaxDebugFloatArrayDataLength => _maxDebugFloatArrayDataLength;
        public int MaxMemoryVectLength => _maxMemoryVectLength;
        public int MaxNamedValueKeyLength => _maxNamedValueKeyLength;


        public Task SendDebugFloatArray(string name, ushort arrayId, float[] data)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            if (name.Length > _maxDebugFloatArrayNameLength)
            {
                throw new ArgumentException($"Name '{name}' is too long for parameter name (max size {_maxDebugFloatArrayNameLength})", nameof(name));
            }

            if (data.Length > _maxDebugFloatArrayDataLength)
            {
                throw new ArgumentException($"Data is too long (max size {_maxDebugFloatArrayDataLength})", nameof(name));
            }
            return InternalSend<DebugFloatArrayPacket>(packet =>
            {
                packet.Payload.TimeUsec = (uint)(DateTime.Now - _bootTime).TotalMilliseconds;
                packet.Payload.ArrayId = arrayId;
                MavlinkTypesHelper.SetString(packet.Payload.Name, name);
                data.CopyTo(packet.Payload.Data,0);
            });
        }

        public Task SendMemoryVect(ushort address, byte version, byte type, sbyte[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length > _maxMemoryVectLength)
            {
                throw new ArgumentException($"Value '{value}' is too long (max size {_maxMemoryVectLength})", nameof(value));
            }
           
            return InternalSend<MemoryVectPacket>(packet =>
            {
                value.CopyTo(packet.Payload.Value, 0);
                packet.Payload.Address = address;
                packet.Payload.Type = type;
                packet.Payload.Ver = version;
            });
        }

        public Task SendNamedValueFloat(string name, float value)
        {
            if (name.Length > _maxNamedValueKeyLength)
            {
                throw new ArgumentException($"Name '{name}' is too long for parameter name (max size {_maxNamedValueKeyLength})", nameof(name));
            }
            return InternalSend<NamedValueFloatPacket>(packet =>
            {
                MavlinkTypesHelper.SetString(packet.Payload.Name, name);
                packet.Payload.TimeBootMs = (uint)(DateTime.Now - _bootTime).TotalMilliseconds;
                packet.Payload.Value = value;
            });
        }

        public Task SendNamedValueInteger(string name, int value)
        {
            if (name.Length > _maxNamedValueKeyLength)
            {
                throw new ArgumentException($"Name '{name}' is too long for parameter name (max size {_maxNamedValueKeyLength})", nameof(name));
            }
            return InternalSend<NamedValueIntPacket>(packet =>
            {
                MavlinkTypesHelper.SetString(packet.Payload.Name, name);
                packet.Payload.TimeBootMs = (uint)(DateTime.Now - _bootTime).TotalMilliseconds;
                packet.Payload.Value = value;
            });
        }

       
    }
}

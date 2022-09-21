// MIT License
//
// Copyright (c) 2018 Alexey Voloshkevich Cursir ltd. (https://github.com/asvol)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// This code was generate by tool Asv.Mavlink.Shell version 1.0.0

using System;
using Asv.IO;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.V2.Development
{

    public static class DevelopmentHelper
    {
        public static void RegisterDevelopmentDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new MissionChecksumPacket());
        }
    }

#region Enums


#endregion

#region Messages

    /// <summary>
    /// Checksum for the current mission, rally points or geofence plan (a GCS can use this checksum to determine if it has a matching plan definition).
    ///         This message must be broadcast following any change to a plan (immediately after the MISSION_ACK that completes the plan upload sequence).
    ///         It may also be requested using MAV_CMD_REQUEST_MESSAGE, where param 2 indicates the plan type for which the hash is required.
    ///         The checksum must be calculated on the autopilot, but may also be calculated by the GCS.
    ///         The checksum uses the same CRC32 algorithm as MAVLink FTP (https://mavlink.io/en/services/ftp.html#crc32-implementation).
    ///         It is run over each item in the plan in seq order (excluding the home location if present in the plan), and covers the following fields (in order):
    ///         frame, command, autocontinue, param1, param2, param3, param4, param5, param6, param7.
    ///       
    ///  MISSION_CHECKSUM
    /// </summary>
    public class MissionChecksumPacket: PacketV2<MissionChecksumPayload>
    {
	    public const int PacketMessageId = 53;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 3;

        public override MissionChecksumPayload Payload { get; } = new MissionChecksumPayload();

        public override string Name => "MISSION_CHECKSUM";
    }

    /// <summary>
    ///  MISSION_CHECKSUM
    /// </summary>
    public class MissionChecksumPayload : IPayload
    {
        public byte GetMaxByteSize() => 5; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            Checksum = BinSerialize.ReadUInt(ref buffer);index+=4;
            MissionType = (MavMissionType)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,Checksum);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)MissionType);index+=1;
            return index; // /*PayloadByteSize*/5;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            Checksum = BitConverter.ToUInt32(buffer,index);index+=4;
            MissionType = (MavMissionType)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Checksum).CopyTo(buffer, index);index+=4;
            buffer[index] = (byte)MissionType;index+=1;
            return index - start; // /*PayloadByteSize*/5;
        }

        /// <summary>
        /// CRC32 checksum of current plan for specified type.
        /// OriginName: checksum, Units: , IsExtended: false
        /// </summary>
        public uint Checksum { get; set; }
        /// <summary>
        /// Mission type.
        /// OriginName: mission_type, Units: , IsExtended: false
        /// </summary>
        public MavMissionType MissionType { get; set; }
    }


#endregion


}

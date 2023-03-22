// MIT License
//
// Copyright (c) 2018 Alexey (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 1.2.2

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvGbs
{

    public static class AsvGbsHelper
    {
        public static void RegisterAsvGbsDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvGbsOutStatusPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify RTK ground base station in HEARTBEAT packet.
        /// MAV_TYPE_ASV_GBS
        /// </summary>
        MavTypeAsvGbs = 250,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Run in observation mode to determine the current position of GBS and start sending RTK corrections.
        /// Param 1 - Minimum observation time (seconds).
        /// Param 2 - Minimum position accuracy (m).
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_GBS_RUN_AUTO_MODE
        /// </summary>
        MavCmdAsvGbsRunAutoMode = 13001,
        /// <summary>
        /// Run in fixed mode with well known GBS position and start sending RTK corrections.
        /// Param 1 - Latitude (int32_t,degE7).
        /// Param 2 - Longitude (int32_t,degE7).
        /// Param 3 - Altitude (int32_t, mm).
        /// Param 4 - Position accuracy (m).
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_GBS_RUN_FIXED_MODE
        /// </summary>
        MavCmdAsvGbsRunFixedMode = 13002,
        /// <summary>
        /// Cancel all modes and switch to default state. Used to cancel all other modes and disable RTK sending.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_GBS_RUN_IDLE_MODE
        /// </summary>
        MavCmdAsvGbsRunIdleMode = 13003,
    }

    /// <summary>
    /// Current GBS mode
    ///  ASV_GBS_STATE
    /// </summary>
    public enum AsvGbsState:uint
    {
        /// <summary>
        /// ASV_GBS_STATE_LOADING
        /// </summary>
        AsvGbsStateLoading = 0,
        /// <summary>
        /// ASV_GBS_STATE_IDLE_MODE
        /// </summary>
        AsvGbsStateIdleMode = 1,
        /// <summary>
        /// ASV_GBS_STATE_ERROR
        /// </summary>
        AsvGbsStateError = 2,
        /// <summary>
        /// ASV_GBS_STATE_AUTO_MODE_IN_PROGRESS
        /// </summary>
        AsvGbsStateAutoModeInProgress = 3,
        /// <summary>
        /// ASV_GBS_STATE_AUTO_MODE
        /// </summary>
        AsvGbsStateAutoMode = 4,
        /// <summary>
        /// ASV_GBS_STATE_FIXED_MODE_IN_PROGRESS
        /// </summary>
        AsvGbsStateFixedModeInProgress = 5,
        /// <summary>
        /// ASV_GBS_STATE_FIXED_MODE
        /// </summary>
        AsvGbsStateFixedMode = 6,
    }


#endregion

#region Messages

    /// <summary>
    /// Ground base station status message. Send with 1 Hz frequency.
    ///  ASV_GBS_OUT_STATUS
    /// </summary>
    public class AsvGbsOutStatusPacket: PacketV2<AsvGbsOutStatusPayload>
    {
	    public const int PacketMessageId = 13001;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 180;

        public override AsvGbsOutStatusPayload Payload { get; } = new AsvGbsOutStatusPayload();

        public override string Name => "ASV_GBS_OUT_STATUS";
    }

    /// <summary>
    ///  ASV_GBS_OUT_STATUS
    /// </summary>
    public class AsvGbsOutStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 13; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 13; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            State = (AsvGbsState)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteByte(ref buffer,(byte)State);
            /* PayloadByteSize = 13 */;
        }



        /// <summary>
        /// Latitude of GBS (value / 10000000D).
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude of GBS (value / 10000000D).
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// Altitude of GBS.
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Current state of GBS. See ASV_GBS_STATE enum.
        /// OriginName: state, Units: , IsExtended: false
        /// </summary>
        public AsvGbsState State { get; set; }
    }


#endregion


}

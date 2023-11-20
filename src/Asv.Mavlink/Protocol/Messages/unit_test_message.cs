// MIT License
//
// Copyright (c) 2023 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 3.2.5-alpha-11

using System;
using Asv.IO;

namespace Asv.Mavlink.V2.UnitTestMessage
{

    public static class UnitTestMessageHelper
    {
        public static void RegisterUnitTestMessageDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new ChemicalDetectorDataPacket());
        }
    }

#region Enums


#endregion

#region Messages

    /// <summary>
    /// Chemical Detector Data
    ///  CHEMICAL_DETECTOR_DATA
    /// </summary>
    public class ChemicalDetectorDataPacket: PacketV2<ChemicalDetectorDataPayload>
    {
	    public const int PacketMessageId = 14202;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 157;
        public override bool WrapToV2Extension => false;

        public override ChemicalDetectorDataPayload Payload { get; } = new ChemicalDetectorDataPayload();

        public override string Name => "CHEMICAL_DETECTOR_DATA";
    }

    /// <summary>
    ///  CHEMICAL_DETECTOR_DATA
    /// </summary>
    public class ChemicalDetectorDataPayload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //AgentId
            sum+=4; //Concentration
            sum+=4; //Bars
            sum+=4; //BarsPeak
            sum+=4; //Dose
            sum+=4; //HazardLevel
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            AgentId = BinSerialize.ReadUInt(ref buffer);
            Concentration = BinSerialize.ReadFloat(ref buffer);
            Bars = BinSerialize.ReadUInt(ref buffer);
            BarsPeak = BinSerialize.ReadUInt(ref buffer);
            Dose = BinSerialize.ReadFloat(ref buffer);
            HazardLevel = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,AgentId);
            BinSerialize.WriteFloat(ref buffer,Concentration);
            BinSerialize.WriteUInt(ref buffer,Bars);
            BinSerialize.WriteUInt(ref buffer,BarsPeak);
            BinSerialize.WriteFloat(ref buffer,Dose);
            BinSerialize.WriteFloat(ref buffer,HazardLevel);
            /* PayloadByteSize = 24 */;
        }
        
        



        /// <summary>
        /// Agent ID, 0=No agent, 1=GA, 2=GB, 3=GD/GF, 4=VX, 5=VXR, 6=DPM, 7=AC/CK, 8=CK, 9=AC, 11=HD
        /// OriginName: agent_id, Units: , IsExtended: false
        /// </summary>
        public uint AgentId { get; set; }
        /// <summary>
        /// Concentration (mg/m3), IEEE floating point format
        /// OriginName: concentration, Units: mg/m3, IsExtended: false
        /// </summary>
        public float Concentration { get; set; }
        /// <summary>
        /// Bars (0 - 8)
        /// OriginName: bars, Units: , IsExtended: false
        /// </summary>
        public uint Bars { get; set; }
        /// <summary>
        /// Peak Bars (0 - 8)
        /// OriginName: bars_peak, Units: , IsExtended: false
        /// </summary>
        public uint BarsPeak { get; set; }
        /// <summary>
        /// Dose (mg-min/m3), IEEE floating point format
        /// OriginName: dose, Units: mg-min/m3, IsExtended: false
        /// </summary>
        public float Dose { get; set; }
        /// <summary>
        /// Hazard Level - none, low, medium, high ( 0-3 )
        /// OriginName: hazard_level, Units: , IsExtended: false
        /// </summary>
        public float HazardLevel { get; set; }
    }


#endregion


}

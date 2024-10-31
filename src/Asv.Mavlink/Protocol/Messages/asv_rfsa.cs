// MIT License
//
// Copyright (c) 2024 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 3.10.4+c1002429a625f2cf26c5bd2680700906e0b44d76

namespace Asv.Mavlink.V2.AsvRfsa
{

    public static class AsvRfsaHelper
    {
        public static void RegisterAsvRfsaDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
        }
    }

#region Enums

    /// <summary>
    /// A mapping of RFSA modes for custom_mode field of heartbeat.
    ///  ASV_RFSA_CUSTOM_MODE
    /// </summary>
    public enum AsvRfsaCustomMode:uint
    {
        /// <summary>
        /// ASV_RFSA_CUSTOM_MODE_IDLE
        /// </summary>
        AsvRfsaCustomModeIdle = 0,
        /// <summary>
        /// ASV_RFSA_CUSTOM_MODE_MEASURE
        /// </summary>
        AsvRfsaCustomModeMeasure = 1,
    }

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify RF spectrum analyzer payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_RFSA
        /// </summary>
        MavTypeAsvRfsa = 253,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Enable RF analyzer. Change mode to ASV_RFSA_CUSTOM_MODE_MEASURE
        /// Param 1 - Frequency in Hz, 0-3 bytes of uint_64(uint32).
        /// Param 2 - Frequency in Hz, 4-7 bytes of uint_64(uint32).
        /// Param 3 - Span frequency in Hz (unit32_t).
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RFSA_ON
        /// </summary>
        MavCmdAsvRfsaOn = 13300,
        /// <summary>
        /// Disable analyzer. Change mode to ASV_RFSA_CUSTOM_MODE_IDLE
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RFSA_OFF
        /// </summary>
        MavCmdAsvRfsaOff = 13301,
    }


#endregion

#region Messages



#endregion


}

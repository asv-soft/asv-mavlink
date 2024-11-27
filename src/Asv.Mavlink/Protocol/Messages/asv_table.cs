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

// This code was generate by tool Asv.Mavlink.Shell version 3.10.4+1a2d7cd3ae509bbfa5f932af5791dfe12de59ff1

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.AsvTable
{

    public static class AsvTableHelper
    {
        public static void RegisterAsvTableDialect(this ImmutableDictionary<ushort,Func<MavlinkMessage>>.Builder src)
        {
        }
    }

#region Enums

    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_TABLE_*_REQUEST commands.
    ///  ASV_TABLE_REQUEST_ACK
    /// </summary>
    public enum AsvTableRequestAck:uint
    {
        /// <summary>
        /// Request is ok.
        /// ASV_TABLE_REQUEST_ACK_OK
        /// </summary>
        AsvTableRequestAckOk = 0,
        /// <summary>
        /// Already in progress.
        /// ASV_TABLE_REQUEST_ACK_IN_PROGRESS
        /// </summary>
        AsvTableRequestAckInProgress = 1,
        /// <summary>
        /// Internal error.
        /// ASV_TABLE_REQUEST_ACK_FAIL
        /// </summary>
        AsvTableRequestAckFail = 2,
        /// <summary>
        /// Not supported.
        /// ASV_TABLE_REQUEST_ACK_NOT_SUPPORTED
        /// </summary>
        AsvTableRequestAckNotSupported = 3,
        /// <summary>
        /// Element not found.
        /// ASV_TABLE_REQUEST_ACK_NOT_FOUND
        /// </summary>
        AsvTableRequestAckNotFound = 4,
    }

    /// <summary>
    /// Chart data transmission data type
    ///  ASV_TABLE_DATA_FORMAT
    /// </summary>
    public enum AsvTableDataFormat:uint
    {
        /// <summary>
        /// Write a value as a fraction between a given minimum and maximum. Uses 8 bits so we have '256' steps between min and max.
        /// ASV_TABLE_DATA_FORMAT_RANGE_FLOAT_8BIT
        /// </summary>
        AsvTableDataFormatRangeFloat8bit = 0,
        /// <summary>
        /// Write a value as a fraction between a given minimum and maximum. Uses 16 bits so we have '65535' steps between min and max.
        /// ASV_TABLE_DATA_FORMAT_RANGE_FLOAT_16BIT
        /// </summary>
        AsvTableDataFormatRangeFloat16bit = 1,
        /// <summary>
        /// Write a value as a float. Uses 32 bits.
        /// ASV_TABLE_DATA_FORMAT_FLOAT
        /// </summary>
        AsvTableDataFormatFloat = 2,
    }

    /// <summary>
    /// Chart type
    ///  ASV_TABLE_UNIT_TYPE
    /// </summary>
    public enum AsvTableUnitType:uint
    {
        /// <summary>
        /// Custom unit.
        /// ASV_TABLE_UNIT_TYPE_CUSTOM
        /// </summary>
        AsvTableUnitTypeCustom = 0,
        /// <summary>
        /// dBm.
        /// ASV_TABLE_UNIT_TYPE_DBM
        /// </summary>
        AsvTableUnitTypeDbm = 1,
    }


#endregion

#region Messages



#endregion


}

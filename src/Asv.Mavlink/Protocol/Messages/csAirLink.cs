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
using System.Text;
using Asv.IO;

namespace Asv.Mavlink.V2.Csairlink
{

    public static class CsairlinkHelper
    {
        public static void RegisterCsairlinkDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AirlinkAuthPacket());
            src.Register(()=>new AirlinkAuthResponsePacket());
        }
    }

#region Enums

    /// <summary>
    ///  AIRLINK_AUTH_RESPONSE_TYPE
    /// </summary>
    public enum AirlinkAuthResponseType:uint
    {
        /// <summary>
        /// Login or password error
        /// AIRLINK_ERROR_LOGIN_OR_PASS
        /// </summary>
        AirlinkErrorLoginOrPass = 0,
        /// <summary>
        /// Auth successful
        /// AIRLINK_AUTH_OK
        /// </summary>
        AirlinkAuthOk = 1,
    }


#endregion

#region Messages

    /// <summary>
    /// Authorization package
    ///  AIRLINK_AUTH
    /// </summary>
    public class AirlinkAuthPacket: PacketV2<AirlinkAuthPayload>
    {
	    public const int PacketMessageId = 52000;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 13;
        public override bool WrapToV2Extension => false;

        public override AirlinkAuthPayload Payload { get; } = new AirlinkAuthPayload();

        public override string Name => "AIRLINK_AUTH";
    }

    /// <summary>
    ///  AIRLINK_AUTH
    /// </summary>
    public class AirlinkAuthPayload : IPayload
    {
        public byte GetMaxByteSize() => 100; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 100; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=Login.Length; //Login
            sum+=Password.Length; //Password
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/50 - Math.Max(0,((/*PayloadByteSize*/100 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Login = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Login)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Login.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            arraySize = 50;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Password)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Password.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Login)
                {
                    Encoding.ASCII.GetBytes(charPointer, Login.Length, bytePointer, Login.Length);
                }
            }
            buffer = buffer.Slice(Login.Length);
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Password)
                {
                    Encoding.ASCII.GetBytes(charPointer, Password.Length, bytePointer, Password.Length);
                }
            }
            buffer = buffer.Slice(Password.Length);
            
            /* PayloadByteSize = 100 */;
        }
        
        



        /// <summary>
        /// Login
        /// OriginName: login, Units: , IsExtended: false
        /// </summary>
        public char[] Login { get; set; } = new char[50];
        public byte GetLoginMaxItemsCount() => 50;
        /// <summary>
        /// Password
        /// OriginName: password, Units: , IsExtended: false
        /// </summary>
        public char[] Password { get; } = new char[50];
    }
    /// <summary>
    /// Response to the authorization request
    ///  AIRLINK_AUTH_RESPONSE
    /// </summary>
    public class AirlinkAuthResponsePacket: PacketV2<AirlinkAuthResponsePayload>
    {
	    public const int PacketMessageId = 52001;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 239;
        public override bool WrapToV2Extension => false;

        public override AirlinkAuthResponsePayload Payload { get; } = new AirlinkAuthResponsePayload();

        public override string Name => "AIRLINK_AUTH_RESPONSE";
    }

    /// <summary>
    ///  AIRLINK_AUTH_RESPONSE
    /// </summary>
    public class AirlinkAuthResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 1; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 1; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // RespType
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RespType = (AirlinkAuthResponseType)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)RespType);
            /* PayloadByteSize = 1 */;
        }
        
        



        /// <summary>
        /// Response type
        /// OriginName: resp_type, Units: , IsExtended: false
        /// </summary>
        public AirlinkAuthResponseType RespType { get; set; }
    }


#endregion


}

// MIT License
//
// Copyright (c) 2025 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.15+3a942e4794bafbc9b7e025a76c610b9704955531 25-05-11.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.Csairlink
{

    public static class CsairlinkHelper
    {
        public static void RegisterCsairlinkDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AirlinkAuthPacket.MessageId, ()=>new AirlinkAuthPacket());
            src.Add(AirlinkAuthResponsePacket.MessageId, ()=>new AirlinkAuthResponsePacket());
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
    public class AirlinkAuthPacket : MavlinkV2Message<AirlinkAuthPayload>
    {
        public const int MessageId = 52000;
        
        public const byte CrcExtra = 13;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AirlinkAuthPayload Payload { get; } = new();

        public override string Name => "AIRLINK_AUTH";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "login",
            "Login",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            50, 
            false),
            new(1,
            "password",
            "Password",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            50, 
            false),
        ];
        public const string FormatMessage = "AIRLINK_AUTH:"
        + "char[50] login;"
        + "char[50] password;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Login);
            writer.Write(StaticFields[1], Payload.Password);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            reader.ReadCharArray(StaticFields[0], Payload.Login);
            reader.ReadCharArray(StaticFields[1], Payload.Password);
        
            
        }
    }

    /// <summary>
    ///  AIRLINK_AUTH
    /// </summary>
    public class AirlinkAuthPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 100; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 100; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +Login.Length // char[50] login
            +Password.Length // char[50] password
            );
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
        public const int LoginMaxItemsCount = 50;
        public char[] Login { get; set; } = new char[50];
        [Obsolete("This method is deprecated. Use GetLoginMaxItemsCount instead.")]
        public byte GetLoginMaxItemsCount() => 50;
        /// <summary>
        /// Password
        /// OriginName: password, Units: , IsExtended: false
        /// </summary>
        public const int PasswordMaxItemsCount = 50;
        public char[] Password { get; } = new char[50];
    }
    /// <summary>
    /// Response to the authorization request
    ///  AIRLINK_AUTH_RESPONSE
    /// </summary>
    public class AirlinkAuthResponsePacket : MavlinkV2Message<AirlinkAuthResponsePayload>
    {
        public const int MessageId = 52001;
        
        public const byte CrcExtra = 239;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AirlinkAuthResponsePayload Payload { get; } = new();

        public override string Name => "AIRLINK_AUTH_RESPONSE";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "resp_type",
            "Response type",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "AIRLINK_AUTH_RESPONSE:"
        + "uint8_t resp_type;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RespType);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RespType = (AirlinkAuthResponseType)reader.ReadEnum(StaticFields[0]);
        
            
        }
    }

    /// <summary>
    ///  AIRLINK_AUTH_RESPONSE
    /// </summary>
    public class AirlinkAuthResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 1; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 1; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t resp_type
            );
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

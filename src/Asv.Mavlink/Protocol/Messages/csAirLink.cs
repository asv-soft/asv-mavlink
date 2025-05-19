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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.16+a43ef88c0eb6d4725d650c062779442ee3bd78f6 25-05-19.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
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
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Login)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Login.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            arraySize = 50;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Password)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Password.Length);
                }
            }
            buffer = buffer[arraySize..];
           

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

        public void Visit(IVisitor visitor)
        {
            ArrayType.Accept(visitor,LoginField, 50, (index,v) =>
            {
                var tmp = (byte)Login[index];
                UInt8Type.Accept(v,LoginField, ref tmp);
                Login[index] = (char)tmp;
            });
            ArrayType.Accept(visitor,PasswordField, 50, (index,v) =>
            {
                var tmp = (byte)Password[index];
                UInt8Type.Accept(v,PasswordField, ref tmp);
                Password[index] = (char)tmp;
            });

        }

        /// <summary>
        /// Login
        /// OriginName: login, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LoginField = new Field.Builder()
            .Name(nameof(Login))
            .Title("login")
            .Description("Login")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,50))

            .Build();
        public const int LoginMaxItemsCount = 50;
        public char[] Login { get; } = new char[50];
        [Obsolete("This method is deprecated. Use GetLoginMaxItemsCount instead.")]
        public byte GetLoginMaxItemsCount() => 50;
        /// <summary>
        /// Password
        /// OriginName: password, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PasswordField = new Field.Builder()
            .Name(nameof(Password))
            .Title("password")
            .Description("Password")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,50))

            .Build();
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

        public void Visit(IVisitor visitor)
        {
            var tmpRespType = (byte)RespType;
            UInt8Type.Accept(visitor,RespTypeField, ref tmpRespType);
            RespType = (AirlinkAuthResponseType)tmpRespType;

        }

        /// <summary>
        /// Response type
        /// OriginName: resp_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RespTypeField = new Field.Builder()
            .Name(nameof(RespType))
            .Title("resp_type")
            .Description("Response type")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AirlinkAuthResponseType _RespType;
        public AirlinkAuthResponseType RespType { get => _RespType; set => _RespType = value; } 
    }




        


#endregion


}

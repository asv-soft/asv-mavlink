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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.2+82bde669fa8b85517700c6d12362e9f17d819d33 25-06-27.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
using System.Linq;
using System.Collections.Generic;
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
    public enum AirlinkAuthResponseType : ulong
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
    public static class AirlinkAuthResponseTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"AIRLINK_ERROR_LOGIN_OR_PASS");
            yield return new EnumValue<T>(converter(1),"AIRLINK_AUTH_OK");
        }
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

        public void Accept(IVisitor visitor)
        {
            ArrayType.Accept(visitor,LoginField, LoginField.DataType, 50, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref Login[index]));
            ArrayType.Accept(visitor,PasswordField, PasswordField.DataType, 50, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref Password[index]));

        }

        /// <summary>
        /// Login
        /// OriginName: login, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LoginField = new Field.Builder()
            .Name(nameof(Login))
            .Title("login")
            .Description("Login")

            .DataType(new ArrayType(CharType.Ascii,50))
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

            .DataType(new ArrayType(CharType.Ascii,50))
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

        public void Accept(IVisitor visitor)
        {
            var tmpRespType = (byte)RespType;
            UInt8Type.Accept(visitor,RespTypeField, RespTypeField.DataType, ref tmpRespType);
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
            .DataType(new UInt8Type(AirlinkAuthResponseTypeHelper.GetValues(x=>(byte)x).Min(),AirlinkAuthResponseTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AirlinkAuthResponseTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AirlinkAuthResponseType _respType;
        public AirlinkAuthResponseType RespType { get => _respType; set => _respType = value; } 
    }




        


#endregion


}

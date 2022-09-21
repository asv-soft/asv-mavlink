using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public static class Crc32Q
    {
        //private static uint _crc32Polynomial = 0xD5828281;

        private static readonly uint[] Crc32 =
        {
            0x00000000, 0x999A0002, 0x98310507, 0x01AB0505, 0x9B670F0D, 0x02FD0F0F, 0x03560A0A, 0x9ACC0A08,
            0x9DCB1B19, 0x04511B1B, 0x05FA1E1E, 0x9C601E1C, 0x06AC1414, 0x9F361416, 0x9E9D1113, 0x07071111,
            0x90933331, 0x09093333, 0x08A23636, 0x91383634, 0x0BF43C3C, 0x926E3C3E, 0x93C5393B, 0x0A5F3939,
            0x0D582828, 0x94C2282A, 0x95692D2F, 0x0CF32D2D, 0x963F2725, 0x0FA52727, 0x0E0E2222, 0x97942220,
            0x8A236361, 0x13B96363, 0x12126666, 0x8B886664, 0x11446C6C, 0x88DE6C6E, 0x8975696B, 0x10EF6969,
            0x17E87878, 0x8E72787A, 0x8FD97D7F, 0x16437D7D, 0x8C8F7775, 0x15157777, 0x14BE7272, 0x8D247270,
            0x1AB05050, 0x832A5052, 0x82815557, 0x1B1B5555, 0x81D75F5D, 0x184D5F5F, 0x19E65A5A, 0x807C5A58,
            0x877B4B49, 0x1EE14B4B, 0x1F4A4E4E, 0x86D04E4C, 0x1C1C4444, 0x85864446, 0x842D4143, 0x1DB74141,
            0xBF43C3C1, 0x26D9C3C3, 0x2772C6C6, 0xBEE8C6C4, 0x2424CCCC, 0xBDBECCCE, 0xBC15C9CB, 0x258FC9C9,
            0x2288D8D8, 0xBB12D8DA, 0xBAB9DDDF, 0x2323DDDD, 0xB9EFD7D5, 0x2075D7D7, 0x21DED2D2, 0xB844D2D0,
            0x2FD0F0F0, 0xB64AF0F2, 0xB7E1F5F7, 0x2E7BF5F5, 0xB4B7FFFD, 0x2D2DFFFF, 0x2C86FAFA, 0xB51CFAF8,
            0xB21BEBE9, 0x2B81EBEB, 0x2A2AEEEE, 0xB3B0EEEC, 0x297CE4E4, 0xB0E6E4E6, 0xB14DE1E3, 0x28D7E1E1,
            0x3560A0A0, 0xACFAA0A2, 0xAD51A5A7, 0x34CBA5A5, 0xAE07AFAD, 0x379DAFAF, 0x3636AAAA, 0xAFACAAA8,
            0xA8ABBBB9, 0x3131BBBB, 0x309ABEBE, 0xA900BEBC, 0x33CCB4B4, 0xAA56B4B6, 0xABFDB1B3, 0x3267B1B1,
            0xA5F39391, 0x3C699393, 0x3DC29696, 0xA4589694, 0x3E949C9C, 0xA70E9C9E, 0xA6A5999B, 0x3F3F9999,
            0x38388888, 0xA1A2888A, 0xA0098D8F, 0x39938D8D, 0xA35F8785, 0x3AC58787, 0x3B6E8282, 0xA2F48280,
            0xD5828281, 0x4C188283, 0x4DB38786, 0xD4298784, 0x4EE58D8C, 0xD77F8D8E, 0xD6D4888B, 0x4F4E8889,
            0x48499998, 0xD1D3999A, 0xD0789C9F, 0x49E29C9D, 0xD32E9695, 0x4AB49697, 0x4B1F9392, 0xD2859390,
            0x4511B1B0, 0xDC8BB1B2, 0xDD20B4B7, 0x44BAB4B5, 0xDE76BEBD, 0x47ECBEBF, 0x4647BBBA, 0xDFDDBBB8,
            0xD8DAAAA9, 0x4140AAAB, 0x40EBAFAE, 0xD971AFAC, 0x43BDA5A4, 0xDA27A5A6, 0xDB8CA0A3, 0x4216A0A1,
            0x5FA1E1E0, 0xC63BE1E2, 0xC790E4E7, 0x5E0AE4E5, 0xC4C6EEED, 0x5D5CEEEF, 0x5CF7EBEA, 0xC56DEBE8,
            0xC26AFAF9, 0x5BF0FAFB, 0x5A5BFFFE, 0xC3C1FFFC, 0x590DF5F4, 0xC097F5F6, 0xC13CF0F3, 0x58A6F0F1,
            0xCF32D2D1, 0x56A8D2D3, 0x5703D7D6, 0xCE99D7D4, 0x5455DDDC, 0xCDCFDDDE, 0xCC64D8DB, 0x55FED8D9,
            0x52F9C9C8, 0xCB63C9CA, 0xCAC8CCCF, 0x5352CCCD, 0xC99EC6C5, 0x5004C6C7, 0x51AFC3C2, 0xC835C3C0,
            0x6AC14140, 0xF35B4142, 0xF2F04447, 0x6B6A4445, 0xF1A64E4D, 0x683C4E4F, 0x69974B4A, 0xF00D4B48,
            0xF70A5A59, 0x6E905A5B, 0x6F3B5F5E, 0xF6A15F5C, 0x6C6D5554, 0xF5F75556, 0xF45C5053, 0x6DC65051,
            0xFA527271, 0x63C87273, 0x62637776, 0xFBF97774, 0x61357D7C, 0xF8AF7D7E, 0xF904787B, 0x609E7879,
            0x67996968, 0xFE03696A, 0xFFA86C6F, 0x66326C6D, 0xFCFE6665, 0x65646667, 0x64CF6362, 0xFD556360,
            0xE0E22221, 0x79782223, 0x78D32726, 0xE1492724, 0x7B852D2C, 0xE21F2D2E, 0xE3B4282B, 0x7A2E2829,
            0x7D293938, 0xE4B3393A, 0xE5183C3F, 0x7C823C3D, 0xE64E3635, 0x7FD43637, 0x7E7F3332, 0xE7E53330,
            0x70711110, 0xE9EB1112, 0xE8401417, 0x71DA1415, 0xEB161E1D, 0x728C1E1F, 0x73271B1A, 0xEABD1B18,
            0xEDBA0A09, 0x74200A0B, 0x758B0F0E, 0xEC110F0C, 0x76DD0504, 0xEF470506, 0xEEEC0003, 0x77760001
        };




        public static uint Calc(byte[] buf)
        {
            uint crc = 0;
            for (var i = 0; i < buf.Length; i++)
                crc = ((crc << 8) & 0xFFFFFF) ^ Crc32[((crc >> 8) ^ buf[i]) & 0xff];
            return crc;
        }

        public static uint Calc(byte[] buf, int buffLen, uint initValue)
        {
            uint crc = initValue;
            for (var i = 0; i < buffLen; i++)
                crc = ((crc << 8) & 0xFFFFFF) ^ Crc32[((crc >> 8) ^ buf[i]) & 0xff];
            return crc;
        }

        public static uint Calc(ReadOnlySpan<byte> buf, int buffLen, uint initValue)
        {
            uint crc = initValue;
            for (var i = 0; i < buffLen; i++)
                crc = ((crc << 8) & 0xFFFFFF) ^ Crc32[((crc >> 8) ^ buf[i]) & 0xff];
            return crc;
        }

        public static uint CalculateCrc32QHash(this IEnumerable<ISizedSpanSerializable> types, uint initValue = 0U)
        {
            return types.Aggregate(initValue, (current, desc) => CalculateCrc32QHash(desc, current));
        }

        public static uint CalculateCrc32QHash(this ISizedSpanSerializable type, uint initValue = 0U)
        {
            var data = ArrayPool<byte>.Shared.Rent(type.GetByteSize());
            try
            {
                var span = new Span<byte>(data, 0, type.GetByteSize());
                var size = span.Length;
                type.Serialize(ref span);
                return Crc32Q.Calc(data, size, initValue);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
            }
        }


    }
}

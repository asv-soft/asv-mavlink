using System.Linq;
using System.Text;

namespace Asv.Mavlink.Shell
{
    public static class X25Crc
    {
        public const ushort CrcSeed = 0xffff;

        public static ushort Accumulate(string s, ushort crc)
        {
            // 28591 = ISO-8859-1 = latin1
            var bytes = Encoding.GetEncoding(28591).GetBytes(s);

            return bytes.Aggregate(crc, (current, b) => Accumulate(b, current));
        }

        public static ushort Accumulate(byte[] bytes, ushort crc, int start, int cnt)
        {
            var stop = start + cnt;
            for (var i = start; i < stop; i++)
            {
                crc = Accumulate(bytes[i], crc);
            }
            return crc;
        }

        public static ushort Accumulate(byte b, ushort crc)
        {
            unchecked
            {
                var ch = (byte)(b ^ (byte)(crc & 0x00ff));
                ch = (byte)(ch ^ (ch << 4));
                return (ushort)((crc >> 8) ^ (ch << 8) ^ (ch << 3) ^ (ch >> 4));
            }
        }
    }
}

using System;

namespace Asv.Mavlink;

public static class Crc32Mavlink
{
    private const uint Polynomial = 0x04C11DB7;
    
    private static readonly uint[] Crc32Table = new uint[256];

    static Crc32Mavlink()
    {
        for (uint i = 0; i < 256; i++)
        {
            var crc = i << 24;
            for (uint j = 0; j < 8; j++)
            {
                crc = (crc & 0x80000000) != 0 ? (crc << 1) ^ Polynomial : crc << 1;
            }
            Crc32Table[i] = crc;
        }
    }
    
    public static uint Accumulate(ReadOnlySpan<byte> buff)
    {
        uint crc = 0;

        foreach (var data in buff)
        {
            var tableIndex = ((crc >> 24) ^ data) & 0xFF;
            crc = (crc << 8) ^ Crc32Table[tableIndex];
        }
        
        return crc;
    }
}
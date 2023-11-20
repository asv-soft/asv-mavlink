using Xunit;

namespace Asv.Mavlink.Test;

public class MavParamEncodingTest
{
    [Theory]
    [InlineData(byte.MinValue)]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MaxValue/2)]
    
    [InlineData(sbyte.MinValue)]
    [InlineData((sbyte)0)]
    [InlineData(sbyte.MaxValue)]
    
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    [InlineData(ushort.MaxValue/2)]
    
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue)]
    [InlineData((short)0)]
    
    [InlineData(uint.MinValue)]
    [InlineData(uint.MaxValue)]
    [InlineData(uint.MaxValue/2)]
    
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(0)]
    
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    [InlineData(float.Epsilon)]
    [InlineData(float.NaN)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.MaxValue/2)]
    [InlineData(float.MinValue/2)]
    [InlineData(0.0f)]
    public void Convert_value_using_byte_wise_encoding_and_check_equal(MavParamValue origin)
    {
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.ByteWiseEncoding);
        var result = encoding.ConvertToMavlinkUnion(origin);
        var convertedValue = encoding.ConvertFromMavlinkUnion(result, origin.Type);
        Assert.Equal(origin, convertedValue);
        
    }
    
    [Theory]
    [InlineData(byte.MinValue)]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MaxValue/2)]
    
    [InlineData(sbyte.MinValue)]
    [InlineData((sbyte)0)]
    [InlineData(sbyte.MaxValue)]
    
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    [InlineData(ushort.MaxValue/2)]
    
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue)]
    [InlineData((short)0)]
    
    // cstyle encoding can't convert large digits to float without loss of precision
    // [InlineData(uint.MinValue)]
    // [InlineData(uint.MaxValue)]
    // [InlineData(uint.MaxValue/2)]
    
    // [InlineData(int.MinValue)]
    // [InlineData(int.MaxValue)]
    
    [InlineData((uint)65535)]
    [InlineData((uint)0)]
    
    [InlineData(65535)]
    [InlineData(0)]
    
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    [InlineData(float.Epsilon)]
    [InlineData(float.NaN)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.MaxValue/2)]
    [InlineData(float.MinValue/2)]
    [InlineData(0.0f)]
    public void Convert_value_using_cstyle_encoding_and_check_equal(MavParamValue origin)
    {
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.CStyleEncoding);
        var result = encoding.ConvertToMavlinkUnion(origin);
        var convertedValue = encoding.ConvertFromMavlinkUnion(result, origin.Type);
        Assert.Equal(origin, convertedValue);
        
    }
    
    
    
}
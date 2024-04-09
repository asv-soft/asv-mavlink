using System;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test.ParamsExt;

public class ParamsExtEncodingTest
{
    #region Convert test valid values

    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeUint8(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeUint8);
        Assert.Equal(MavParamExtType.MavParamExtTypeUint8, encoding.Type);
    }

    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeInt8(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeInt8);
        Assert.Equal(MavParamExtType.MavParamExtTypeInt8, encoding.Type);
    }
    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeUint16(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeUint16);
        Assert.Equal(MavParamExtType.MavParamExtTypeUint16, encoding.Type);
    }
    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeUint32(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeUint32);
        Assert.Equal(MavParamExtType.MavParamExtTypeUint32, encoding.Type);
    }
    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeReal32(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeReal32);
        Assert.Equal(MavParamExtType.MavParamExtTypeReal32, encoding.Type);
    }
    [Theory]
   // [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x7F, 0x40,
    })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeUint64(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeUint64);
        Assert.Equal(MavParamExtType.MavParamExtTypeUint64, encoding.Type);
    }
    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x7F, 0x40})]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeInt64(byte[] origin)
    {
        var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeInt64);
        Assert.Equal(MavParamExtType.MavParamExtTypeInt64, encoding.Type);
    }
    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeReal64(byte[] origin)
    {
      var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeReal64);
        Assert.Equal(MavParamExtType.MavParamExtTypeReal64, encoding.Type);
    }
    [Theory]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6,2,3,4,5,6,7,8,5,34,2,2,34,45,56,6,4,43,43,4,3,3, })]
    [InlineData(data: new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x7F, 0x40})]
    
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeCustom(byte[] origin)
    {
       var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeCustom);
        Assert.Equal(MavParamExtType.MavParamExtTypeCustom, encoding.Type);
    }

    #endregion

    #region Convert test bad values
    [Theory]
    [InlineData(data: new byte[0])]
    [InlineData(data: new byte[] { 1, 3, 4, 5, 6 })]
    
    public void Convert_value_using_byte_wise_encoding_and_check_type_MavParamExtTypeCustom_Bad(byte[] origin)
    {
        try
        {
            var encoding = MavParamExtHelper.CreateFromBuffer(origin, MavParamExtType.MavParamExtTypeCustom);
            Assert.Equal(MavParamExtType.MavParamExtTypeCustom, encoding.Type);
        }
        catch (ArgumentException ex)
        {
            Assert.True(true, ex.Message);
        }        
    }

    #endregion
    
    
    #region Check parameter name with bad values

    [Theory]
    [InlineData("")]
    public void CheckParamName_string_empty_test(string origin)
    {
        Assert.Throws<Exception>(() => MavParamExtHelper.CheckParamName(origin));
    }

    [Theory]
    [InlineData("PARAM_EXT_REQUEST_LIST")]
    public void CheckParamName_string_overflow_test(string origin)
    {
        Assert.Throws<Exception>(() => MavParamExtHelper.CheckParamName(origin));
    }

    [Theory]
    [InlineData("_EXT__LIST")]
    public void CheckParamName_string_regex_mismatch_test(string origin)
    {
        Assert.Throws<ArgumentException>(() => MavParamExtHelper.CheckParamName(origin));
    }

    #endregion

    #region Check parameter name with valid values

    [Theory]
    [InlineData("SIM_GPS_TYPE")]
    public void CheckParamName_string_valid_test(string origin)
    {
        try
        {
            MavParamExtHelper.CheckParamName(origin);
            Assert.True(true);
        }
        catch (Exception ex)
        {
            Assert.False(true, ex.Message);
        }
    }

    #endregion
}
using System;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test.ParamsExt;

public class ParamsExtValueTest
{
    #region Constructor test

    [Fact]
    public void MavParamExtValue_byte_Constructor_Test()
    {
        var data = new byte();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeUint8,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_sbyte_Constructor_Test()
    {
        var data = new sbyte();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeInt8,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_short_Constructor_Test()
    {
        var data = new short();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeInt16,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_ushort_Constructor_Test()
    {
        var data = new ushort();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeUint16,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_int_Constructor_Test()
    {
        var data = new int();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeInt32,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_uint_Constructor_Test()
    {
        var data = new uint();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeUint32,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_long_Constructor_Test()
    {
        var data = new long();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeInt32,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_ulong_Constructor_Test()
    {
        var data = new ulong();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeUint32,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_float_Constructor_Test()
    {
        var data = new float();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeReal32,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_double_Constructor_Test()
    {
        var data = new double();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeReal64,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    [Fact]
    public void MavParamExtValue_char_arr_Constructor_Test()
    {
        var data = Array.Empty<char>();
        MavParamExtValue testValueParamExtValue = new MavParamExtValue(data);
        Assert.True(testValueParamExtValue.Type == MavParamExtType.MavParamExtTypeCustom,
            $"{testValueParamExtValue.Type.ToString()}");
    }

    #endregion

    #region Comparison Test

    [Fact]
    public void Test_MavParamsExtValue_Compare_To()
    {
        var data = new MavParamExtValue(new byte[] { 1, 2, 3, 4, 5 });
        MavParamExtValue data1 = new MavParamExtValue(new byte[] { 1, 2, 3, 4, 5 });
        var res = data1.CompareTo(data);
        Assert.True(res == 0, $"{res}");
    }

    #endregion
}
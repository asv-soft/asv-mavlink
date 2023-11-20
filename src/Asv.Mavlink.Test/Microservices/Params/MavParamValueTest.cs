using System;
using Asv.Mavlink.V2.Common;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class MavParamValueTest
{
    private readonly ITestOutputHelper _output;

    public MavParamValueTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void Byte_Min_Value()
    {
        var mavValue = new MavParamValue(byte.MinValue);
        var value = byte.MinValue;

        _output.WriteLine($"mav: {(byte)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeUint8);
    }
    
    [Fact]
    public void Byte_Max_Value()
    {
        var mavValue = new MavParamValue(byte.MaxValue);
        var value = byte.MaxValue;
        
        _output.WriteLine($"mav: {(byte)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeUint8);
    }
    
    [Fact]
    public void SByte_Min_Value()
    {
        var mavValue = new MavParamValue(sbyte.MinValue);
        var value = sbyte.MinValue;
        
        _output.WriteLine($"mav: {(sbyte)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeInt8);
    }
    
    [Fact]
    public void SByte_Max_Value()
    {
        var mavValue = new MavParamValue(sbyte.MaxValue);
        var value = sbyte.MaxValue;
        
        _output.WriteLine($"mav: {(sbyte)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeInt8);
    }

    [Fact]
    public void Short_Min_Value()
    {
        var mavValue = new MavParamValue(short.MinValue);
        var value = short.MinValue;
        
        _output.WriteLine($"mav: {(short)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeInt16);
    }
    
    [Fact]
    public void Short_Max_Value()
    {
        var mavValue = new MavParamValue(short.MaxValue);
        var value = short.MaxValue;
        
        _output.WriteLine($"mav: {(short)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeInt16);
    }
    
    [Fact]
    public void UShort_Min_Value()
    {
        var mavValue = new MavParamValue(ushort.MinValue);
        var value = ushort.MinValue;
        
        _output.WriteLine($"mav: {(ushort)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeUint16);
    }
    
    [Fact]
    public void UShort_Max_Value()
    {
        var mavValue = new MavParamValue(ushort.MaxValue);
        var value = ushort.MaxValue;
        
        _output.WriteLine($"mav: {(ushort)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeUint16);
    }
    
    [Fact]
    public void Int_Min_Value()
    {
        var mavValue = new MavParamValue(int.MinValue);
        var value = int.MinValue;
        
        _output.WriteLine($"mav: {(int)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeInt32);
    }
    
    [Fact]
    public void Int_Max_Value()
    {
        var mavValue = new MavParamValue(int.MaxValue);
        var value = int.MaxValue;
        
        _output.WriteLine($"mav: {(int)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeInt32);
    }
    
    [Fact]
    public void UInt_Min_Value()
    {
        var mavValue = new MavParamValue(uint.MinValue);
        var value = uint.MinValue;
        
        _output.WriteLine($"mav: {(uint)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeUint32);
    }
    
    [Fact]
    public void UInt_Max_Value()
    {
        var mavValue = new MavParamValue(uint.MaxValue);
        var value = uint.MaxValue;
        
        _output.WriteLine($"mav: {(uint)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.True(mavValue.Type == MavParamType.MavParamTypeUint32);
    }
    
    [Fact]
    public void Float_Min_Value()
    {
        var mavValue = new MavParamValue(float.MinValue);
        var value = float.MinValue;
        
        _output.WriteLine($"mav: {(float)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.Equal(MavParamType.MavParamTypeReal32, mavValue.Type);
    }
    
    [Fact]
    public void Float_Max_Value()
    {
        var mavValue = new MavParamValue(float.MaxValue);
        var value = float.MaxValue;
        
        _output.WriteLine($"mav: {(float)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.Equal(MavParamType.MavParamTypeReal32, mavValue.Type);
    }
    
    [Fact]
    public void Float_NaN_Value()
    {
        var mavValue = new MavParamValue(float.NaN);
        var value = float.NaN;
        
        _output.WriteLine($"mav: {(float)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.Equal(MavParamType.MavParamTypeReal32, mavValue.Type);
    }
    
    [Fact]
    public void Float_Positive_Infinity_Value()
    {
        var mavValue = new MavParamValue(float.PositiveInfinity);
        var value = float.PositiveInfinity;
        
        _output.WriteLine($"mav: {(float)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.Equal(MavParamType.MavParamTypeReal32, mavValue.Type);
    }
    
    [Fact]
    public void Float_Negative_Infinity_Value()
    {
        var mavValue = new MavParamValue(float.NegativeInfinity);
        var value = float.NegativeInfinity;
        
        _output.WriteLine($"mav: {(float)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.Equal(MavParamType.MavParamTypeReal32, mavValue.Type);
    }
    
    [Fact]
    public void Float_Epsilon_Value()
    {
        var mavValue = new MavParamValue(float.Epsilon);
        var value = float.Epsilon;
        
        _output.WriteLine($"mav: {(float)mavValue}");
        _output.WriteLine($"real: {value}");

        Assert.True(value == mavValue);
        Assert.True(mavValue.Equals(value));
        Assert.Equal(MavParamType.MavParamTypeReal32, mavValue.Type);
    }

    [Fact]
    public void Float_Cast_To_Int_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var value = (int)mavValue;
        });
    }
    
    [Fact]
    public void Int_Cast_To_Float_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(1234);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var value = (float)mavValue;
        });
    }
    
    [Fact]
    public void Int_Rather_Than()
    {
        var mavValue = new MavParamValue(1234);
        Assert.True(mavValue > 220);
    }
    
    [Fact]
    public void Int_Rather_Than_Or_Equals()
    {
        var mavValue = new MavParamValue(1234);
        Assert.True(mavValue >= 0);
    }
    
    [Fact]
    public void Int_Less_Than()
    {
        var mavValue = new MavParamValue(1234);
        Assert.False(mavValue < 220);
    }
    
    [Fact]
    public void Int_Less_Than_Or_Equals()
    {
        var mavValue = new MavParamValue(1234);
        Assert.False(mavValue <= 0);
    }
    
    [Fact]
    public void Float_Rather_Than()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.True(mavValue > 2.20f);
    }
    
    [Fact]
    public void Float_Rather_Than_Or_Equals()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.True(mavValue >= 0f);
    }
    
    [Fact]
    public void Float_Less_Than()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.False(mavValue < 0f);
    }
    
    [Fact]
    public void Float_Less_Than_Or_Equals()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.False(mavValue <= 2.2f);
    }
    
    [Fact]
    public void Float_Rather_Than_Int_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue > 2;
        });
    }
    
    [Fact]
    public void Float_Rather_Than_Or_Equals_Int_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue >= 2;
        });
    }
    
    [Fact]
    public void Float_Less_Than_Int_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue < 0;
        });
    }
    
    [Fact]
    public void Float_Less_Than_Or_Equals_Int_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(12.34f);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue <= 2;
        });
    }
    
    [Fact]
    public void Int_Rather_Than_Float_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(1234);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue > 2.2f;
        });
    }
    
    [Fact]
    public void Int_Rather_Than_Or_Equals_Float_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(1234);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue >= 2.2f;
        });
    }
    
    [Fact]
    public void Int_Less_Than_Float_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(1234);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue < 12.34f;
        });
    }
    
    [Fact]
    public void Int_Less_Than_Or_Equals_Float_Invalid_Operation_Exception()
    {
        var mavValue = new MavParamValue(1234);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = mavValue <= 12.34f;
        });
    }
}
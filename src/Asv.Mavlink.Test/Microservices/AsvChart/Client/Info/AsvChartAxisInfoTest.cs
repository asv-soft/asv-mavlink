using System;
using Asv.Mavlink.AsvChart;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartAxisInfo))]
public class AsvChartAxisInfoTest
{
    [Theory]
    [InlineData("name", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 1f, 10)]
    [InlineData("name123443211235", AsvChartUnitType.AsvChartUnitTypeCustom, -10f, 200f, 1)]
    public void Ctor_PerfectArguments_Success(
        string name, 
        AsvChartUnitType unitType, 
        float min, 
        float max, 
        int size
    )
    {
        // Act
        var info = new AsvChartAxisInfo(name, unitType, min, max, size);
        
        // Assert
        Assert.NotNull(info);
        Assert.Equal(name, info.Name);
        Assert.Equal(unitType, info.Unit);
        Assert.Equal(min, info.Min);
        Assert.Equal(max, info.Max);
        Assert.Equal(size, info.Size);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("12345678910111211")]
    public void Ctor_WrongNameArgument_Throws(string name)
    {
        // Act + Assert
        Assert.Throws<MavlinkException>(() =>
        {
            _ = new AsvChartAxisInfo(name, AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 1f, 10);
        });
    }
    
    [Theory]
    [InlineData(100, 0)]
    [InlineData(0, 0)]
    public void Ctor_WrongMinAndMax_Throws(float min, float max)
    {
        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new AsvChartAxisInfo("name", AsvChartUnitType.AsvChartUnitTypeDbm, min, max, 10);
        });
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Ctor_WrongSize_Throws(int size)
    {
        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new AsvChartAxisInfo("name", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 1f, size);
        });
    }
}
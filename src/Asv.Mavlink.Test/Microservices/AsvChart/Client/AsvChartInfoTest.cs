using System;

using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(AsvChartAxisInfo))]
public class AsvChartInfoTest
{
    [Fact]
    public void CtorAsvChartInfo_ThrowsNullReferenceException_ArgIsNull()
    {
        AsvChartInfoPayload? payload = null;
        AsvChartAxisInfo? axis= null;
        Assert.Throws<NullReferenceException>(() =>
        {
            var chart = new AsvChartInfo(payload);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var chart = new AsvChartInfo(1, "testChart", axis, axis, AsvChartDataFormat.AsvChartDataFormatFloat);
        });
    }
    
    [Fact]
    public void CtorAsvChartAxisInfo_NullReferenceException_ArgIsNull()
    {
        Assert.Throws<Exception>(() =>
        {
            var axis = new AsvChartAxisInfo("", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 0f, 10);
        });
    }
}
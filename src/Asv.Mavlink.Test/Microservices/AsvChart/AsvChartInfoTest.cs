using Asv.Mavlink.V2.AsvChart;
using Xunit;
using Asv.Mavlink.V2.AsvRfsa;

namespace Asv.Mavlink.Test
{
    public class AsvChartInfoTest
    {
        [Fact]
        public void TestSignalInfoConstructorAndFill()
        {
            // Arrange
            ushort id = 1;
            float maxX = 10, minX = 0, maxY = 20, minY = 0;
            int axisXSize = 5, axisYSize = 5;
            AsvChartDataFormat format = AsvChartDataFormat.AsvChartDataFormatFloat;
            string signalName = "test", axesXName = "x", axesYName = "y";

            // Act
            var chartInfo = new AsvChartInfo(id, signalName, new AsvChartAxisInfo(axesXName, AsvChartUnitType.AsvChartUnitTypeCustom,  minX,maxX,axisXSize), new AsvChartAxisInfo(axesYName, AsvChartUnitType.AsvChartUnitTypeDbm,  minY,maxY, axisYSize), format, null);

            // Assert
            Assert.Equal(id, chartInfo.Id);
            Assert.Equal(maxX, chartInfo.AxisX.Max);
            Assert.Equal(minX, chartInfo.AxisX.Min);
            Assert.Equal(maxY, chartInfo.AxisY.Max);
            Assert.Equal(minY, chartInfo.AxisY.Min);
            Assert.Equal(axisXSize, chartInfo.AxisX.Size);
            Assert.Equal(axisYSize, chartInfo.AxisY.Size);
            Assert.Equal(format, chartInfo.Format);
            Assert.Equal(signalName, chartInfo.SignalName);
            Assert.Equal(axesXName, chartInfo.AxisX.Name);
            Assert.Equal(axesYName, chartInfo.AxisY.Name);

            // Arrange
            var payload = new AsvChartInfoPayload();

            // Act
            chartInfo.Fill(payload);

            // Assert
            Assert.Equal(id, payload.ChartId);
            Assert.Equal(maxX, payload.AxesXMax);
            Assert.Equal(minX, payload.AxesXMin);
            Assert.Equal(maxY, payload.AxesYMax);
            Assert.Equal(minY, payload.AxesYMin);
            Assert.Equal((ushort)axisXSize, payload.AxesXCount);
            Assert.Equal((ushort)axisYSize, payload.AxesYCount);
            Assert.Equal(format, payload.Format);
            Assert.Equal(signalName, MavlinkTypesHelper.GetString(payload.ChartName));
            Assert.Equal(axesXName, MavlinkTypesHelper.GetString(payload.AxesXName));
            Assert.Equal(axesYName, MavlinkTypesHelper.GetString(payload.AxesYName));
        }
    }
}
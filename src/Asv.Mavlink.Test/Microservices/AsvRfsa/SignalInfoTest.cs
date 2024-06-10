using Xunit;
using Asv.Mavlink.V2.AsvRfsa;

namespace Asv.Mavlink.Test
{
    public class SignalInfoTest
    {
        [Fact]
        public void TestSignalInfoConstructorAndFill()
        {
            // Arrange
            ushort id = 1;
            float maxX = 10, minX = 0, maxY = 20, minY = 0;
            int axisXSize = 5, axisYSize = 5;
            AsvRfsaSignalFormat format = AsvRfsaSignalFormat.AsvSdrSignalFormatFloat;
            string signalName = "test", axesXName = "x", axesYName = "y";

            // Act
            var signalInfo = new SignalInfo(id, maxX, minX, maxY, minY, axisXSize, axisYSize, format, signalName, axesXName, axesYName);

            // Assert
            Assert.Equal(id, signalInfo.Id);
            Assert.Equal(maxX, signalInfo.MaxX);
            Assert.Equal(minX, signalInfo.MinX);
            Assert.Equal(maxY, signalInfo.MaxY);
            Assert.Equal(minY, signalInfo.MinY);
            Assert.Equal(axisXSize, signalInfo.AxisXSize);
            Assert.Equal(axisYSize, signalInfo.AxisYSize);
            Assert.Equal(format, signalInfo.Format);
            Assert.Equal(signalName, signalInfo.SignalName);
            Assert.Equal(axesXName, signalInfo.AxesXName);
            Assert.Equal(axesYName, signalInfo.AxesYName);

            // Arrange
            var payload = new AsvRfsaSignalInfoPayload();

            // Act
            signalInfo.Fill(payload);

            // Assert
            Assert.Equal(id, payload.SignalId);
            Assert.Equal(maxX, payload.AxesXMax);
            Assert.Equal(minX, payload.AxesXMin);
            Assert.Equal(maxY, payload.AxesYMax);
            Assert.Equal(minY, payload.AxesYMin);
            Assert.Equal((ushort)axisXSize, payload.AxesXCount);
            Assert.Equal((ushort)axisYSize, payload.AxesYCount);
            Assert.Equal(format, payload.Format);
            Assert.Equal(signalName, MavlinkTypesHelper.GetString(payload.SignalName));
            Assert.Equal(axesXName, MavlinkTypesHelper.GetString(payload.AxesXName));
            Assert.Equal(axesYName, MavlinkTypesHelper.GetString(payload.AxesYName));
        }
    }
}
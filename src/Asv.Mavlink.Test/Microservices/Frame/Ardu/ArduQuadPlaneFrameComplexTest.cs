using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ArduQuadPlaneFrameClient))]
public class ArduQuadPlaneFrameComplexTest(ITestOutputHelper log) : ArduFrameComplexTestBase(log)
{
    protected override ReadOnlyDictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>>
        AvailableFramesMap => ArduQuadPlaneFrameCompability.Map;

    protected override FrameClient CreateFrameClient(ParamsClientEx paramsClientEx, HeartbeatClient heartbeatClient)
    {
        return new ArduQuadPlaneFrameClient(heartbeatClient, paramsClientEx);
    }

    protected override string FrameClassParamName => ArduQuadPlaneFrameClient.FrameClassParam;
    protected override string FrameTypeParamName => ArduQuadPlaneFrameClient.FrameTypeParam;
}
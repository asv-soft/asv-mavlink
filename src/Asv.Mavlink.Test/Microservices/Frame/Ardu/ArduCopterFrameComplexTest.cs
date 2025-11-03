using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ArduCopterClientDevice))]
public class ArduCopterFrameComplexTest(ITestOutputHelper log) : ArduFrameComplexTestBase(log)
{
    protected override ReadOnlyDictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>>
        AvailableFramesMap => ArduCopterFrameCompability.Map;

    protected override FrameClient CreateFrameClient(ParamsClientEx paramsClientEx, HeartbeatClient heartbeatClient)
    {
        return new ArduCopterFrameClient(heartbeatClient, paramsClientEx);
    }
    
    protected override string FrameClassParamName => ArduCopterFrameClient.FrameClassParam;
    protected override string FrameTypeParamName => ArduCopterFrameClient.FrameTypeParam;
}
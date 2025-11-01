using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public abstract class ArduFrameComplexTestBase(ITestOutputHelper log) : ComplexTestBase<FrameClient, ParamsServerEx>(log)
{
    private const int ParamsDefaultValue = 0;
    private const int ParamsClientChunkUpdateBufferMs = 100;
    
    private readonly HeartbeatClientConfig _heartbeatConfig = new();
    private readonly ParamsClientExConfig _paramsClientConfig = new()
    {
        ChunkUpdateBufferMs = ParamsClientChunkUpdateBufferMs
    };
    private readonly ParamsServerExConfig _paramsServerConfig = new();
    private readonly MavParamCStyleEncoding _encoding = new();

    protected override FrameClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var paramsDesc = GetFrameParameterNames()
            .Select(name => new ParamDescription
            {
                Name = name,
                ParamType = MavParamType.MavParamTypeInt32
            })
            .ToList();

        var heartbeatClient = new HeartbeatClient(identity, _heartbeatConfig, core);
        var paramsClient = new ParamsClient(identity, _paramsClientConfig, core);
        var paramsClientEx = new ParamsClientEx(paramsClient, _paramsClientConfig, _encoding, paramsDesc);
        
        
        return CreateFrameClient(paramsClientEx, heartbeatClient);
    }

    protected override ParamsServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var paramsMeta = GetFrameParameterNames()
            .Select(name => new MavParamTypeMetadata(name, MavParamType.MavParamTypeInt32)
            {
                DefaultValue = new MavParamValue(ParamsDefaultValue),
                MinValue = new MavParamValue(0),
                MaxValue = new MavParamValue(100),
            })
            .ToList();

        var statusTextServer = new StatusTextServer(identity, new StatusTextLoggerConfig(), core);
        var configuration = new InMemoryConfiguration();
        var paramsServer = new ParamsServer(identity, core);
        return new ParamsServerEx(paramsServer,
            statusTextServer,
            paramsMeta,
            _encoding,
            configuration,
            _paramsServerConfig);
    }

    private List<string> GetFrameParameterNames() => [FrameClassParamName, FrameTypeParamName];
    
    protected abstract string FrameClassParamName { get; }
    protected abstract string FrameTypeParamName { get; }
    
    protected abstract FrameClient CreateFrameClient(ParamsClientEx paramsClientEx, HeartbeatClient heartbeatClient);

    protected abstract ReadOnlyDictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>> AvailableFramesMap
    {
        get;
    }

    [Fact]
    public async Task LoadAvailableFrames_NotEmpty_Success()
    {
        // Arrange
        _ = Server;
        var correctAvailableDroneFrames = GetAvailableFrames();

        // Act
        await Client.Init();

        // Assert
        Assert.True(correctAvailableDroneFrames.IsDeepEqual(Client.Frames));
    }

    [Fact]
    public async Task SetFrame_ValidValue_Success()
    {
        // Arrange
        _ = Server;
        await Client.Init();
        var availableFrameToSet = Client.Frames.First();
        var tsc = new TaskCompletionSource<bool>();
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(1000));
        cts.Token.Register(() => tsc.TrySetResult(false));
        using var sub = Client.CurrentFrame.WhereNotNull().Subscribe(v =>
        {
            if (v.Id == availableFrameToSet.Key)
            {
                tsc.TrySetResult(true);
            }
        });
        
        // Act
        await Client.SetFrame(availableFrameToSet.Value, cts.Token);
        ClientTime.Advance(TimeSpan.FromMilliseconds(ParamsClientChunkUpdateBufferMs + 50));
        var res = await tsc.Task;

        // Assert
        Assert.True(res);
        Assert.Equal(Client.CurrentFrame.CurrentValue, availableFrameToSet.Value);
    }

    [Fact]
    public async Task SetFrame_FromOtherFrame_Throws()
    {
        // Arrange
        _ = Server;
        await Client.Init();
        var frameToSet = new TestDroneFrame();

        // Act
        var task = Client.SetFrame(frameToSet);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await task;
        });
    }

    [Fact]
    public async Task SetFrame_InvalidFrameParams_Throws()
    {
        // Arrange
        _ = Server;
        var frameToSet = new ArduDroneFrame(ArduFrameClass.DynamicScriptingMatrix, null);

        // Act
        var task = Client.SetFrame(frameToSet);

        // Assert
        await Assert.ThrowsAsync<DroneFrameIsNotAvailableException>(async () =>
        {
            await task;
        });
    }
    
    private IReadOnlyDictionary<string, IDroneFrame> GetAvailableFrames()
    {
        var droneFrames = new Dictionary<string, IDroneFrame>();
        
        foreach (var (frameClass, types) in AvailableFramesMap)
        {
            if (types.Count == 0)
            {
                var meta = new Dictionary<string, string>
                {
                    [FrameClassParamName] = frameClass.ToString()
                };
                var droneFrame = new ArduDroneFrame(frameClass, null, meta);
                droneFrames.Add(droneFrame.Id, droneFrame);
            }
            else
            {
                foreach (var type in types)
                {
                    var meta = new Dictionary<string, string>
                    {
                        [FrameClassParamName] = frameClass.ToString(),
                        [FrameTypeParamName] = type.ToString()
                    };
                    var droneFrame = new ArduDroneFrame(frameClass, type, meta);
                    droneFrames.Add(droneFrame.Id, droneFrame);
                }
            }
        }

        return droneFrames;
    }
}

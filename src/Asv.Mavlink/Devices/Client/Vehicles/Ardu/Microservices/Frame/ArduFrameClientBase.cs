using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public abstract class ArduFrameClientBase : FrameClient
{
    private readonly IParamsClientEx _paramsClient;
    private readonly ReadOnlyDictionary<ArduCopterFrameClass, IReadOnlyList<ArduCopterFrameType>> _availableFramesMap;
    private readonly ObservableDictionary<string, IMotorFrame> _motorFrames;

    protected ArduFrameClientBase(
        IHeartbeatClient heartbeat,
        IParamsClientEx paramsClient,
        ReadOnlyDictionary<ArduCopterFrameClass, IReadOnlyList<ArduCopterFrameType>> availableFramesMap)
        : base(heartbeat.Identity, heartbeat.Core)
    {
        _paramsClient = paramsClient;
        _availableFramesMap = availableFramesMap;
        _motorFrames = [];
    }

    public abstract string FrameClassParamName { get; }
    public abstract string FrameTypeParamName { get; }
    
    public override IReadOnlyObservableDictionary<string, IMotorFrame> MotorFrames => _motorFrames;
    
    public override ValueTask LoadAvailableFrames(CancellationToken cancel = default)
    {
        _motorFrames.Clear();

        foreach (var (frameClass, types) in _availableFramesMap)
        {
            if (types.Count == 0)
            {
                var meta = new Dictionary<string, string>
                {
                    [FrameClassParamName] = frameClass.ToString()
                };  
                var motorFrame = new ArduMotorFrame(frameClass, null, meta);
                _motorFrames.Add(motorFrame.Id, motorFrame);
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
                    var motorFrame = new ArduMotorFrame(frameClass, type, meta);
                    _motorFrames.Add(motorFrame.Id, motorFrame);
                }
            }
        }
        
        return ValueTask.CompletedTask;
    }
    
    public override async Task SetFrame(IMotorFrame motorFrameToSet, CancellationToken cancel = default)
    {
        if (motorFrameToSet is not ArduMotorFrame arduMotorFrame)
        {
            // ReSharper disable once LocalizableElement
            throw new ArgumentException($"Instance of {nameof(ArduMotorFrame)} expected", nameof(motorFrameToSet));
        }
        
        if (!_motorFrames.ContainsKey(motorFrameToSet.Id))
        {
            throw new MotorFrameIsNotAvailableException();
        }
        
        await _paramsClient.WriteOnce(
            FrameClassParamName,
            (int) arduMotorFrame.MotorFrameClass,
            cancel).ConfigureAwait(false);

        if (arduMotorFrame.MotorFrameType is not null)
        {
            await _paramsClient.WriteOnce(
                FrameTypeParamName,
                (int) arduMotorFrame.MotorFrameType.Value,
                cancel).ConfigureAwait(false);
        }
    }

    public override async Task<IMotorFrame?> GetCurrentFrame(CancellationToken cancel = default)
    {
        cancel.ThrowIfCancellationRequested();
        var currentFrameClassRaw = await _paramsClient.ReadOnce(FrameClassParamName, cancel).ConfigureAwait(false);
        var currentFrameTypeRaw = await _paramsClient.ReadOnce(FrameTypeParamName, cancel).ConfigureAwait(false);
        
        var currentFrameClass = (ArduCopterFrameClass)(int)currentFrameClassRaw;
        var currentFrameType = (ArduCopterFrameType?)(int?)currentFrameTypeRaw;

        if (!Enum.IsDefined(currentFrameClass))
        {
            throw new ArduCopterFrameClassUnknownException();
        }

        if (currentFrameType is not null && !Enum.IsDefined(currentFrameType.Value))
        {
            throw new ArduCopterFrameTypeUnknownException();
        }
        
        EnsureValidFrameType(currentFrameClass, ref currentFrameType);
        var motorFrameId = ArduMotorFrame.GenerateId(currentFrameClass, currentFrameType);
        return _motorFrames.GetValueOrDefault(motorFrameId);
    }

    /// <summary>
    /// Ensure that the current frame type is null if it is not presented in the <ref name="_availableFramesMap"/>
    /// </summary>
    /// <param name="currentFrameClass">Frame class</param>
    /// <param name="currentFrameType">Frame type to validate</param>
    /// <remarks>
    /// If a valid frame class exists in the map but has no specific frame types (empty list),
    /// We need to set the type to null to match how those frames were created
    /// </remarks>
    private void EnsureValidFrameType(ArduCopterFrameClass currentFrameClass, ref ArduCopterFrameType? currentFrameType)
    {
        if (_availableFramesMap.TryGetValue(currentFrameClass, out var types) && types.Count == 0)
        {
            currentFrameType = null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public abstract class ArduFrameClientBase : FrameClient
{
    private readonly IParamsClientEx _paramsClient;
    private readonly ReadOnlyDictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>> _availableFramesMap;
    private readonly ObservableDictionary<string, IMotorFrame> _motorFrames;
    private readonly ReactiveProperty<IMotorFrame?> _currentMotorFrame;
    private IDisposable? _currentFrameUpdateSub;

    protected ArduFrameClientBase(
        IHeartbeatClient heartbeat,
        IParamsClientEx paramsClient,
        ReadOnlyDictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>> availableFramesMap)
        : base(heartbeat.Identity, heartbeat.Core)
    {
        _paramsClient = paramsClient;
        _availableFramesMap = availableFramesMap;
        _motorFrames = [];

        _currentMotorFrame = new ReactiveProperty<IMotorFrame?>(null);
        CurrentMotorFrame = _currentMotorFrame.ToReadOnlyReactiveProperty();
    }

    public abstract string FrameClassParamName { get; }
    public abstract string FrameTypeParamName { get; }
    
    public override IReadOnlyObservableDictionary<string, IMotorFrame> MotorFrames => _motorFrames;
    
    public override ReadOnlyReactiveProperty<IMotorFrame?> CurrentMotorFrame { get; }
    
    public override ValueTask RefreshAvailableFrames(CancellationToken cancel = default)
    {
        cancel.ThrowIfCancellationRequested();
        
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
        cancel.ThrowIfCancellationRequested();
        
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

    public override async Task RefreshCurrentFrame(CancellationToken cancel = default)
    {
        cancel.ThrowIfCancellationRequested();
        
        var currentFrameClassRaw = await _paramsClient.ReadOnce(FrameClassParamName, cancel).ConfigureAwait(false);
        var currentFrameTypeRaw = await _paramsClient.ReadOnce(FrameTypeParamName, cancel).ConfigureAwait(false);
        
        _currentMotorFrame.Value = GetMotorFrameFromParams(currentFrameClassRaw, currentFrameTypeRaw);

        _currentFrameUpdateSub?.Dispose();
        _currentFrameUpdateSub = _paramsClient.OnValueChanged
            .Where(kvp => kvp.Item1 == FrameClassParamName || kvp.Item1 == FrameTypeParamName)
            .Subscribe(v =>{
                var frameClassPresent = _paramsClient.Items.ContainsKey(FrameClassParamName);
                var frameTypePresent = _paramsClient.Items.ContainsKey(FrameTypeParamName);
            
                if (!frameClassPresent || !frameTypePresent)
                {
                    return;
                }
            
                if (v.Item1 == FrameClassParamName)
                {
                    currentFrameClassRaw = v.Item2;
                    currentFrameTypeRaw = _paramsClient.Items[FrameTypeParamName].Value.Value;
                }
                else if (v.Item1 == FrameTypeParamName)
                {
                    currentFrameClassRaw = _paramsClient.Items[FrameClassParamName].Value.Value;
                    currentFrameTypeRaw = v.Item2;
                }
                else
                {
                    return;
                }
            
                _currentMotorFrame.Value = GetMotorFrameFromParams(currentFrameClassRaw, currentFrameTypeRaw);
            });
    }

    private IMotorFrame? GetMotorFrameFromParams(
        MavParamValue currentFrameClassRaw, 
        MavParamValue currentFrameTypeRaw)
    {
        var currentFrameClass = (ArduFrameClass)int.Parse(currentFrameClassRaw.PrintValue());
        var currentFrameType = (ArduFrameType?)int.Parse(currentFrameTypeRaw.PrintValue());

        if (!Enum.IsDefined(currentFrameClass))
        {
            throw new ArduFrameClassUnknownException();
        }

        if (!Enum.IsDefined(currentFrameType.Value))
        {
            throw new ArduFrameTypeUnknownException();
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
    private void EnsureValidFrameType(ArduFrameClass currentFrameClass, ref ArduFrameType? currentFrameType)
    {
        if (_availableFramesMap.TryGetValue(currentFrameClass, out var types) && types.Count == 0)
        {
            currentFrameType = null;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _currentFrameUpdateSub?.Dispose();
            _currentMotorFrame.Dispose();
            CurrentMotorFrame.Dispose();
        }
        
        base.Dispose(disposing);
    }
}
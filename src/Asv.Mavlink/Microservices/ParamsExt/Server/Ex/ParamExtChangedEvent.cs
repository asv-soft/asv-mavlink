namespace Asv.Mavlink;

public readonly struct ParamExtChangedEvent((ushort, IMavParamExtTypeMetadata) param, MavParamExtValue oldValue, 
    MavParamExtValue newValue, bool isRemoteChange)
{
    public bool IsRemoteChange { get; } = isRemoteChange;
    public ushort ParamIndex { get; } = param.Item1;
    public IMavParamExtTypeMetadata Metadata { get; } = param.Item2;
    public MavParamExtValue OldValue { get; } = oldValue;
    public MavParamExtValue NewValue { get; } = newValue;
}
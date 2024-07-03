using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;
using DynamicData;
using NLog;

namespace Asv.Mavlink;

public class AsvRsgaClientEx : DisposableOnceWithCancel, IAsvRsgaClientEx
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly ICommandClient _commandClient;
    private readonly SourceList<AsvRsgaCustomMode> _supportedModes;
    public AsvRsgaClientEx(IAsvRsgaClient client, ICommandClient commandClient)
    {
        _commandClient = commandClient;
        Base = client;
        _supportedModes = new SourceList<AsvRsgaCustomMode>().DisposeItWith(Disposable);
        client.OnCompatibilityResponse.Subscribe(OnCapabilityResponse).DisposeItWith(Disposable);
    }

    private void OnCapabilityResponse(AsvRsgaCompatibilityResponsePayload asv)
    {
        if (asv.Result != AsvRsgaRequestAck.AsvRsgaRequestAckOk)
        {
            Logger.Warn($"Error to get compatibility:{asv.Result:G}");
            return;
        }
        
        _supportedModes.Edit(inner =>
        {
            inner.Clear();
            inner.AddRange(RsgaHelper.GetSupportedModes(asv));
        });
    }

    public IAsvRsgaClient Base { get; }

    public IObservable<IChangeSet<AsvRsgaCustomMode>> AvailableModes => _supportedModes.Connect();

    public Task RefreshInfo(CancellationToken cancel = default)
    {
        return Base.GetCompatibilities(cancel);
    }

    public async Task<MavResult> SetMode(AsvRsgaCustomMode mode, ulong frequencyHz, CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => RsgaHelper.SetArgsForSetMode(item, mode,frequencyHz),cs.Token).ConfigureAwait(false);
        return result.Result;
    }
}
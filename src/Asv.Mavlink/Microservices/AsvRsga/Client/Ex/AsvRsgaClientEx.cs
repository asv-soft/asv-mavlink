using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class AsvRsgaClientEx : DisposableOnceWithCancel, IAsvRsgaClientEx
{
    private readonly ILogger _logger;
    private readonly ICommandClient _commandClient;
    private readonly SourceList<AsvRsgaCustomMode> _supportedModes;
    public AsvRsgaClientEx(
        IAsvRsgaClient client, 
        ICommandClient commandClient,
        ILogger? logger = null)
    {
        _logger = logger ?? NullLogger.Instance;
        _commandClient = commandClient;
        Base = client;
        _supportedModes = new SourceList<AsvRsgaCustomMode>().DisposeItWith(Disposable);
        client.OnCompatibilityResponse.Subscribe(OnCapabilityResponse).DisposeItWith(Disposable);
    }

    private void OnCapabilityResponse(AsvRsgaCompatibilityResponsePayload asv)
    {
        if (asv.Result != AsvRsgaRequestAck.AsvRsgaRequestAckOk)
        {
            _logger.ZLogWarning($"Error to get compatibility:{asv.Result:G}");
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

    public async Task<MavResult> SetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => RsgaHelper.SetArgsForSetMode(item, mode),cs.Token).ConfigureAwait(false);
        return result.Result;
    }
}
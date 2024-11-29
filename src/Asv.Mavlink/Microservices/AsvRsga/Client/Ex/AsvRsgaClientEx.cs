using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class AsvRsgaClientEx : DisposableOnceWithCancel, IAsvRsgaClientEx
{
    private readonly ILogger _logger;
    private readonly ICommandClient _commandClient;
    private readonly ObservableList<AsvRsgaCustomMode> _supportedModes;
    public AsvRsgaClientEx(IAsvRsgaClient client, ICommandClient commandClient)
    {
        _logger = client.Core.Log.CreateLogger<AsvRsgaClientEx>();
        _commandClient = commandClient;
        Base = client;
        _supportedModes = new ObservableList<AsvRsgaCustomMode>();
        client.OnCompatibilityResponse.Subscribe(OnCapabilityResponse).DisposeItWith(Disposable);
    }

    private void OnCapabilityResponse(AsvRsgaCompatibilityResponsePayload? asv)
    {
        if (asv == null) return;
        if (asv.Result != AsvRsgaRequestAck.AsvRsgaRequestAckOk)
        {
            _logger.ZLogWarning($"Error to get compatibility:{asv.Result:G}");
            return;
        }
        
        _supportedModes.Clear();
        _supportedModes.AddRange(RsgaHelper.GetSupportedModes(asv));
    }
    public string Name => $"{Base.Name}Ex";
    public IAsvRsgaClient Base { get; }

    public IReadOnlyObservableList<AsvRsgaCustomMode> AvailableModes => _supportedModes;

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

    public MavlinkClientIdentity Identity =>Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}
using System;
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

public class AsvRsgaClientEx : MavlinkMicroserviceClient, IAsvRsgaClientEx
{
    private readonly ILogger _logger;
    private readonly ICommandClient _commandClient;
    private readonly ObservableList<AsvRsgaCustomMode> _supportedModes;
    private readonly IDisposable _sub2;

    public AsvRsgaClientEx(IAsvRsgaClient client, ICommandClient commandClient)
        :base(RsgaHelper.MicroserviceExName, client.Identity, client.Core)
    {
        _logger = client.Core.LoggerFactory.CreateLogger<AsvRsgaClientEx>();
        _commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
        Base = client;
        _supportedModes = new ObservableList<AsvRsgaCustomMode>();
        _sub2 = client.OnCompatibilityResponse.Subscribe(OnCapabilityResponse);
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
}
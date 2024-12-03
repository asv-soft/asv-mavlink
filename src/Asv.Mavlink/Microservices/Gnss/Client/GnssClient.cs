using System;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

public class GnssClient : MavlinkMicroserviceClient, IGnssClient
{
    public GnssClient(MavlinkClientIdentity identity,ICoreServices core) : base(GnssHelper.MicroserviceName, identity, core)
    {
        Main = InternalFilter<GpsRawIntPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        Additional = InternalFilter<Gps2RawPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
    }

    public ReadOnlyReactiveProperty<GpsRawIntPayload?> Main { get; }
    public ReadOnlyReactiveProperty<Gps2RawPayload?> Additional { get; }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Main.Dispose();
            Additional.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(Main).ConfigureAwait(false);
        await CastAndDispose(Additional).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}
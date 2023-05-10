using System;
using System.Collections.ObjectModel;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public class AdsbVehicleClientEx : DisposableOnceWithCancel, IAdsbVehicleClientEx
{
    private readonly SourceCache<AdsbVehiclePayload, uint> _targetSource;
    private readonly ReadOnlyObservableCollection<AdsbVehiclePayload> _targets;

    public AdsbVehicleClientEx(IAdsbVehicleClient client)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));

        _targetSource = new SourceCache<AdsbVehiclePayload, uint>(_ => _.IcaoAddress).DisposeItWith(Disposable);
        _targetSource.Connect().Bind(out _targets).Subscribe().DisposeItWith(Disposable);

        client.Target.Subscribe(_targetSource.AddOrUpdate).DisposeItWith(Disposable);
    }

    public ReadOnlyObservableCollection<AdsbVehiclePayload> Targets => _targets;
}
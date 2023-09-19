using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using DynamicData.Binding;
using NLog;

namespace Asv.Mavlink;

public class MissionServerEx : DisposableOnceWithCancel, IMissionServerEx
{
    private readonly IMissionServer _server;
    private readonly IConfiguration _config;
    private readonly SourceCache<MissionItem, ushort> _missionSource;
    private ReadOnlyObservableCollection<MissionItem> _missionItems;
    private ushort _currentMission;
    
    public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public MissionServerEx(IMissionServer server, IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _server = server ?? throw new ArgumentNullException(nameof(server));
        
        _missionSource = new SourceCache<MissionItem, ushort>(_ => _.Index).DisposeItWith(Disposable);

        _missionSource.Connect()
            .SortBy(_ => _.Index)
            .Bind(out _missionItems)
            .DisposeMany()
            .Subscribe()
            .DisposeItWith(Disposable);
        
        _server.OnMissionCount.Subscribe(UploadMission).DisposeItWith(Disposable);
        _server.OnMissionRequestList.Subscribe(DownloadMission).DisposeItWith(Disposable);
        _server.OnMissionClearAll.Subscribe(ClearAllMission).DisposeItWith(Disposable);
        _server.OnMissionSetCurrent.Subscribe(SetCurrentMission).DisposeItWith(Disposable);
    }

    private async void UploadMission(MissionCountPacket missionCount)
    {
        _missionSource.Clear();
        
        for (ushort i = 0; i < missionCount.Payload.Count; i++)
        {
            await _server.SendMissionRequestInt(_ => _.Payload.Seq = i).ConfigureAwait(false);
            
            var missionItem = await _server.OnMissionItemInt;
            
            _missionSource.AddOrUpdate(new MissionItem(missionItem.Payload));
        }

        _server.SendMissionAck(_ =>
        {
            _.Payload.TargetComponent = missionCount.ComponentId;
            _.Payload.TargetComponent = missionCount.SystemId;
            _.Payload.MissionType = MavMissionType.MavMissionTypeMission;
            _.Payload.Type = MavMissionResult.MavMissionAccepted;
        });
    }
    
    private async void DownloadMission(MissionRequestListPacket packet)
    {
        ushort missionCount = (ushort)_missionItems.Count;
        
        await _server.SendMissionCount(_ =>
        {
            _.Payload.MissionType = MavMissionType.MavMissionTypeMission;
            _.Payload.Count = missionCount;
        });
        
        for (ushort i = 0; i < missionCount; i++)
        {
            var missionItem = await _server.OnMissionRequestInt;

            await _server.SendMissionItemInt(_ =>
            {
                _.Payload.TargetSystem = _missionItems[i].Payload.TargetSystem;
                _.Payload.TargetComponent = _missionItems[i].Payload.TargetComponent;
                _.Payload.MissionType = _missionItems[i].Payload.MissionType;
                _.Payload.Seq = _missionItems[i].Payload.Seq;
                _.Payload.Command = _missionItems[i].Payload.Command;
                _.Payload.Current = _missionItems[i].Payload.Current;
                _.Payload.Autocontinue = _missionItems[i].Payload.Autocontinue;
                _.Payload.Frame = _missionItems[i].Payload.Frame;
                _.Payload.Param1 = _missionItems[i].Payload.Param1;
                _.Payload.Param2 = _missionItems[i].Payload.Param2;
                _.Payload.Param3 = _missionItems[i].Payload.Param3;
                _.Payload.Param4 = _missionItems[i].Payload.Param4;
                _.Payload.X = _missionItems[i].Payload.X;
                _.Payload.Y = _missionItems[i].Payload.Y;
                _.Payload.Z = _missionItems[i].Payload.Z;
            });
            
            await _server.SendMissionRequestInt(_ => _.Payload.Seq = i);
        }

        await _server.OnMissionAck;
    }
    
    private async void ClearAllMission(MissionClearAllPacket packet)
    {
        _missionSource.Clear();
    }
    
    private async void SetCurrentMission(MissionSetCurrentPacket packet)
    {
        _currentMission = packet.Payload.Seq;
    }
}
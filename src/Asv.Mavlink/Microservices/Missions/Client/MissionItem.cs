using System;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using R3;


namespace Asv.Mavlink
{
    public sealed class MissionItem : IDisposable,IAsyncDisposable
    {
        internal readonly MissionItemIntPayload Payload;
        private readonly Subject<Unit> _onChanged;

        public MissionItem(MissionItemIntPayload item)
        {
            Payload = item ?? throw new ArgumentNullException(nameof(item));
            _onChanged = new Subject<Unit>();
            Index = Payload.Seq;
            
            Location = new ReactiveProperty<GeoPoint>(
                new GeoPoint(
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(item.X), 
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(item.Y), 
                item.Z
                )
            );
            _sub1 = Location.Subscribe(Edit,(x,cb) =>
            {
                cb(p=>
                {
                    p.X = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Latitude);
                    p.Y = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Longitude);
                    p.Z = (float)x.Altitude;
                });
            });

            AutoContinue = new ReactiveProperty<bool>(item.Autocontinue != 0);
            _sub2 = AutoContinue.Subscribe(Edit,(b,cb) =>cb(p=>p.Autocontinue = (byte)(b ? 1 : 0)));

            Command = new ReactiveProperty<MavCmd>(item.Command);
            _sub3 = Command.Subscribe(Edit,(c,cb) => cb(p=>p.Command = c));

            Current = new ReactiveProperty<bool>(item.Current != 0);
            _sub3 = Current.Subscribe(Edit,(b,cb) => cb(p=>p.Current = (byte)(b ? 1 : 0)));

            Frame = new ReactiveProperty<MavFrame>(item.Frame);
            _sub4 = Frame.Subscribe(Edit,(f,cb) => cb(p=>p.Frame = f));

            MissionType = new ReactiveProperty<MavMissionType>(item.MissionType);
            _sub5 = MissionType.Subscribe(Edit,(t,cb) => cb(p=>p.MissionType = t));

            Param1 = new ReactiveProperty<float>(item.Param1);
            _sub6 = Param1.Subscribe(Edit,(f,cb) => cb(p=>p.Param1 = f));

            Param2 = new ReactiveProperty<float>(item.Param2);
            _sub7 = Param2.Subscribe(Edit,(f,cb) => cb(p=>p.Param2 = f));

            Param3 = new ReactiveProperty<float>(item.Param3);
            _sub8 = Param3.Subscribe(Edit,(f,cb) => cb(p=>p.Param3 = f));

            Param4 = new ReactiveProperty<float>(item.Param4);
            _sub9 = Param4.Subscribe(Edit,(f,cb) => cb(p=>p.Param4 = f));

            _sub10 = _onChanged.Subscribe(_ =>
            {
                SyncPropertiesWith(Payload);
            });
        }
        public ushort Index { get; private set; }
        public ReactiveProperty<GeoPoint> Location { get; }
        public ReactiveProperty<bool> AutoContinue { get; }
        public ReactiveProperty<MavCmd> Command { get; }
        public ReactiveProperty<bool> Current { get; }
        public ReactiveProperty<MavFrame> Frame { get; }
        public ReactiveProperty<MavMissionType> MissionType { get; }
        public ReactiveProperty<float> Param1 { get; }
        public ReactiveProperty<float> Param2 { get; }
        public ReactiveProperty<float> Param3 { get; }
        public ReactiveProperty<float> Param4 { get; }
        public Observable<Unit> OnChanged => _onChanged;

        public void Edit(Action<MissionItemIntPayload> editCallback)
        {
            var oldState = (Payload.X, Payload.Y, Payload.Z, Payload.Autocontinue, Payload.Command, 
                Payload.Current, Payload.Frame, Payload.MissionType, Payload.Param1, 
                Payload.Param2, Payload.Param3, Payload.Param4, Payload.Seq);
            
            editCallback(Payload);
            
            var newState = (Payload.X, Payload.Y, Payload.Z, Payload.Autocontinue, Payload.Command, 
                Payload.Current, Payload.Frame, Payload.MissionType, Payload.Param1, 
                Payload.Param2, Payload.Param3, Payload.Param4, Payload.Seq);
            
            if (oldState.Equals(newState))
            {
                return;
            }
            
            _onChanged.OnNext(Unit.Default);
        }
        
        public object? Tag { get; set; }

        public override string ToString()
        {
            return $"Mission Item: {Command} target: {Location})";
        }

        private void SyncPropertiesWith(MissionItemIntPayload payload)
        {
            Index = payload.Seq;
            Location.Value = new GeoPoint(
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(payload.X),
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(payload.Y),
                payload.Z
            );
            AutoContinue.Value = payload.Autocontinue != 0;
            Command.Value = payload.Command;
            Current.Value = payload.Current != 0;
            Frame.Value = payload.Frame;
            MissionType.Value = payload.MissionType;
            Param1.Value = payload.Param1;
            Param2.Value = payload.Param2;
            Param3.Value = payload.Param3;
            Param4.Value = payload.Param4;
        }

        #region Dispose
        
        private readonly IDisposable _sub1;
        private readonly IDisposable _sub2;
        private readonly IDisposable _sub3;
        private readonly IDisposable _sub4;
        private readonly IDisposable _sub5;
        private readonly IDisposable _sub6;
        private readonly IDisposable _sub7;
        private readonly IDisposable _sub8;
        private readonly IDisposable _sub9;
        private readonly IDisposable _sub10;

        public void Dispose()
        {
            _onChanged.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
            _sub4.Dispose();
            _sub5.Dispose();
            _sub6.Dispose();
            _sub7.Dispose();
            _sub8.Dispose();
            _sub9.Dispose();
            _sub10.Dispose();
            Location.Dispose();
            AutoContinue.Dispose();
            Command.Dispose();
            Current.Dispose();
            Frame.Dispose();
            MissionType.Dispose();
            Param1.Dispose();
            Param2.Dispose();
            Param3.Dispose();
            Param4.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await CastAndDispose(_onChanged).ConfigureAwait(false);
            await CastAndDispose(_sub1).ConfigureAwait(false);
            await CastAndDispose(_sub2).ConfigureAwait(false);
            await CastAndDispose(_sub3).ConfigureAwait(false);
            await CastAndDispose(_sub4).ConfigureAwait(false);
            await CastAndDispose(_sub5).ConfigureAwait(false);
            await CastAndDispose(_sub6).ConfigureAwait(false);
            await CastAndDispose(_sub7).ConfigureAwait(false);
            await CastAndDispose(_sub8).ConfigureAwait(false);
            await CastAndDispose(_sub9).ConfigureAwait(false);
            await CastAndDispose(_sub10).ConfigureAwait(false);
            await CastAndDispose(Location).ConfigureAwait(false);
            await CastAndDispose(AutoContinue).ConfigureAwait(false);
            await CastAndDispose(Command).ConfigureAwait(false);
            await CastAndDispose(Current).ConfigureAwait(false);
            await CastAndDispose(Frame).ConfigureAwait(false);
            await CastAndDispose(MissionType).ConfigureAwait(false);
            await CastAndDispose(Param1).ConfigureAwait(false);
            await CastAndDispose(Param2).ConfigureAwait(false);
            await CastAndDispose(Param3).ConfigureAwait(false);
            await CastAndDispose(Param4).ConfigureAwait(false);

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
}
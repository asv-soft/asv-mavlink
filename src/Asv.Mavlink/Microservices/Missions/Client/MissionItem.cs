using System;
using System.Reactive;
using System.Reactive.Subjects;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class MissionItem : DisposableOnceWithCancel
    {
        internal readonly MissionItemIntPayload Payload;
        private readonly Subject<Unit> _onChanged;

        public MissionItem(MissionItemIntPayload item)
        {
            Payload = item ?? throw new ArgumentNullException(nameof(item));
            _onChanged = new Subject<Unit>().DisposeItWith(Disposable);
            Location = new RxValue<GeoPoint>(new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(item.X), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(item.Y), item.Z)).DisposeItWith(Disposable);
            Location.Subscribe(x =>
            {
                Edit(p=>
                {
                    p.X = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Latitude);
                    p.Y = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Longitude);
                    p.Z = (float)x.Altitude;
                });
            }).DisposeItWith(Disposable);

            AutoContinue = new RxValue<bool>(item.Autocontinue != 0).DisposeItWith(Disposable);
            AutoContinue.Subscribe(b =>Edit(p=>p.Autocontinue = (byte)(b ? 1 : 0))).DisposeItWith(Disposable);

            Command = new RxValue<MavCmd>(item.Command).DisposeItWith(Disposable);
            Command.Subscribe(c => Edit(p=>p.Command = c)).DisposeItWith(Disposable);

            Current = new RxValue<bool>(item.Current != 0).DisposeItWith(Disposable);
            Current.Subscribe(b => Edit(p=>p.Current = (byte)(b ? 1 : 0))).DisposeItWith(Disposable);

            Frame = new RxValue<MavFrame>(item.Frame).DisposeItWith(Disposable);
            Frame.Subscribe(f => Edit(p=>p.Frame = f)).DisposeItWith(Disposable);

            MissionType = new RxValue<MavMissionType>(item.MissionType).DisposeItWith(Disposable);
            MissionType.Subscribe(t => Edit(p=>p.MissionType = t)).DisposeItWith(Disposable);

            Param1 = new RxValue<float>(item.Param1).DisposeItWith(Disposable);
            Param1.Subscribe(f => Edit(p=>p.Param1 = f)).DisposeItWith(Disposable);

            Param2 = new RxValue<float>(item.Param2).DisposeItWith(Disposable);
            Param2.Subscribe(f => Edit(p=>p.Param2 = f)).DisposeItWith(Disposable);

            Param3 = new RxValue<float>(item.Param3).DisposeItWith(Disposable);
            Param3.Subscribe(f => Edit(p=>p.Param3 = f)).DisposeItWith(Disposable);

            Param4 = new RxValue<float>(item.Param4).DisposeItWith(Disposable);
            Param4.Subscribe(f => Edit(p=>p.Param4 = f)).DisposeItWith(Disposable);

            
        }

        public ushort Index => Payload.Seq;

        public IRxEditableValue<GeoPoint> Location { get; }
        public IRxEditableValue<bool> AutoContinue { get; }
        public IRxEditableValue<MavCmd> Command { get; }
        public IRxEditableValue<bool> Current { get; }
        public IRxEditableValue<MavFrame> Frame { get; }
        public IRxEditableValue<MavMissionType> MissionType { get; }
        public IRxEditableValue<float> Param1 { get; }
        public IRxEditableValue<float> Param2 { get; }
        public IRxEditableValue<float> Param3 { get; }
        public IRxEditableValue<float> Param4 { get; }
        public IObservable<Unit> OnChanged => _onChanged;

        public void Edit(Action<MissionItemIntPayload> editCallback)
        {
            editCallback(Payload);
            _onChanged.OnNext(Unit.Default);
        }
        
        public object Tag { get; set; }

        public override string ToString()
        {
            return $"Mission Item: {Command} target: {Location})";
        }
    }
}
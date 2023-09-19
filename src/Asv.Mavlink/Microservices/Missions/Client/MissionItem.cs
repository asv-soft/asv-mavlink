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
        private readonly Subject<Unit> _onChanged = new();

        public MissionItem(MissionItemIntPayload item)
        {
            Payload = item;
            Location = new RxValue<GeoPoint>(new GeoPoint(item.X / 10_000_000.0, item.Y / 10_000_000.0, item.Z)).DisposeItWith(Disposable);
            Location.Subscribe(_ =>
            {
                Payload.X = (int)(_.Latitude * 10_000_000.0);
                Payload.Y = (int)(_.Longitude * 10_000_000.0);
                Payload.Z = (float)_.Altitude;
                _onChanged.OnNext(Unit.Default);
            }).DisposeItWith(Disposable);

            Autocontinue = new RxValue<bool>(item.Autocontinue != 0).DisposeItWith(Disposable);
            Autocontinue.Subscribe(_ => item.Autocontinue = (byte)(_ ? 1 : 0)).DisposeItWith(Disposable);

            Command = new RxValue<MavCmd>(item.Command).DisposeItWith(Disposable);
            Command.Subscribe(_ => item.Command = _).DisposeItWith(Disposable);

            Current = new RxValue<bool>(item.Current != 0).DisposeItWith(Disposable);
            Current.Subscribe(_ => item.Current = (byte)(_ ? 1 : 0)).DisposeItWith(Disposable);

            Frame = new RxValue<MavFrame>(item.Frame).DisposeItWith(Disposable);
            Frame.Subscribe(_ => item.Frame = _).DisposeItWith(Disposable);

            MissionType = new RxValue<MavMissionType>(item.MissionType).DisposeItWith(Disposable);
            MissionType.Subscribe(_ => item.MissionType = _).DisposeItWith(Disposable);

            Param1 = new RxValue<float>(item.Param1).DisposeItWith(Disposable);
            Param1.Subscribe(_ => item.Param1 = _).DisposeItWith(Disposable);

            Param2 = new RxValue<float>(item.Param2).DisposeItWith(Disposable);
            Param2.Subscribe(_ => item.Param2 = _).DisposeItWith(Disposable);

            Param3 = new RxValue<float>(item.Param3).DisposeItWith(Disposable);
            Param3.Subscribe(_ => item.Param3 = _).DisposeItWith(Disposable);

            Param4 = new RxValue<float>(item.Param4).DisposeItWith(Disposable);
            Param4.Subscribe(_ => item.Param4 = _).DisposeItWith(Disposable);

            _onChanged.DisposeItWith(Disposable);
        }

        public ushort Index => Payload.Seq;

        public IRxEditableValue<GeoPoint> Location { get; }
        public IRxEditableValue<bool> Autocontinue { get; }
        public IRxEditableValue<MavCmd> Command { get; }
        public IRxEditableValue<bool> Current { get; }
        public IRxEditableValue<MavFrame> Frame { get; }
        public IRxEditableValue<MavMissionType> MissionType { get; }
        public IRxEditableValue<float> Param1 { get; }
        public IRxEditableValue<float> Param2 { get; }
        public IRxEditableValue<float> Param3 { get; }
        public IRxEditableValue<float> Param4 { get; }
        public IObservable<Unit> OnChanged => _onChanged;
        public object Tag { get; set; }

        public override string ToString()
        {
            return $"Mission Item: {Command} target: {Location})";
        }
    }
}
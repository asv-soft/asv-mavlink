using System;
using System.Threading;
using Asv.Common;
using R3;

namespace Asv.Mavlink;

public static class HeartbeatClientHelper
{
        
    public static Observable<LinkState> WhenRepairConnection(this ReadOnlyReactiveProperty<LinkState> src, TimeSpan lostTime,
        CancellationToken cancel = default)
    {
        return new LostConnectionSubject(src, cancel, lostTime);
    }
    
    public class LostConnectionSubject : Observable<LinkState>
    {
        private readonly TimeSpan _lostTime;
        private readonly Subject<LinkState> _rx = new();
        private LinkState _prevState = LinkState.Disconnected;
        private DateTime _lastTimeDisconnected = DateTime.MinValue;
        private readonly IDisposable _sub1;

        public LostConnectionSubject(ReadOnlyReactiveProperty<LinkState> src, CancellationToken cancel, TimeSpan lostTime)
        {
            _lostTime = lostTime;
            _sub1 = src.DistinctUntilChanged().Subscribe(OnChange);
        }

        private void OnChange(LinkState linkState)
        {
            if (_prevState == LinkState.Disconnected) return;
            switch (linkState)
            {
                case LinkState.Disconnected:
                    _lastTimeDisconnected = DateTime.Now;
                    break;
                case LinkState.Downgrade:
                    break;
                case LinkState.Connected:
                    if (_prevState == LinkState.Disconnected & (DateTime.Now - _lastTimeDisconnected > _lostTime))
                    {
                        _rx.OnNext(LinkState.Connected);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(linkState), linkState, null);
            }

            _prevState = linkState;
        }

        protected override IDisposable SubscribeCore(Observer<LinkState> observer)
        {
            return _rx.Subscribe(observer);
        }
    }
}
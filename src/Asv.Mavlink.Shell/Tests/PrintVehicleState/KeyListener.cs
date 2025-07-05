using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Asv.Mavlink.Shell;

public class KeyListener : IDisposable
{
    private readonly Subject<ConsoleKey> _keySubject = new();
    private readonly Subject<VehicleDirection> _directionSubject = new();
    private bool _isRunning;
    
    public IObservable<ConsoleKey> KeyStream => _keySubject;
    
    public IObservable<VehicleDirection> DirectionStream => _directionSubject;
    
    public async Task ListenAsync()
    {
        _isRunning = true;
        while (_isRunning)
        {
            var key = Console.ReadKey(true).Key;

            _keySubject.OnNext(key);

            if (TryMapKeyToDirection(key, out var direction))
            {
                _directionSubject.OnNext(direction);
            }
        }
    }

    public void Stop() => _isRunning = false;
    
    public Task<ConsoleKey> WaitForKeyAsync(IEnumerable<ConsoleKey> validKeys)
    {
        var tcs = new TaskCompletionSource<ConsoleKey>();

        Task.Run(() =>
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (!validKeys.Any() || validKeys.Contains(key))
                {
                    tcs.TrySetResult(key);
                    break;
                }
            }
        });

        return tcs.Task;
    }

    private static bool TryMapKeyToDirection(ConsoleKey key, out VehicleDirection direction)
    {
        switch (key)
        {
            case ConsoleKey.RightArrow:
                direction = VehicleDirection.Right;
                return true;
            case ConsoleKey.LeftArrow:
                direction = VehicleDirection.Left;
                return true;
            case ConsoleKey.UpArrow:
                direction = VehicleDirection.Up;
                return true;
            case ConsoleKey.DownArrow:
                direction = VehicleDirection.Down;
                return true;
            case ConsoleKey.U:
                direction = VehicleDirection.U;
                return true;
            case ConsoleKey.D:
                direction = VehicleDirection.D;
                return true;
            case ConsoleKey.PageUp:
                direction = VehicleDirection.PageUp;
                return true;
            case ConsoleKey.PageDown:
                direction = VehicleDirection.PageDown;
                return true;
            case ConsoleKey.Q:
                direction = VehicleDirection.Q;
                return true;
            case ConsoleKey.T:
                direction = VehicleDirection.T;
                return true;
            default:
                direction = default;
                return false;
        }
    }

    public void Dispose()
    {
        _keySubject?.Dispose();
        _directionSubject?.Dispose();
    }
}

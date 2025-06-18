using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink.Shell;

public sealed class KeyListener: IDisposable
{
    private bool _isDisposed;

    private readonly ReactiveProperty<ConsoleKey> _keyPressed;
    public ReadOnlyReactiveProperty<VehicleAction> KeyPressed { get; }

    public KeyListener()
    {
        _keyPressed = new ReactiveProperty<ConsoleKey>();
        KeyPressed = _keyPressed.Select(ConvertKeyToAction).ToReadOnlyReactiveProperty();
    }
    
    public ValueTask ListenAsync(CancellationToken cancel)
    {
        if (cancel.IsCancellationRequested)
        {
            return ValueTask.CompletedTask;
        }
        
        while (!cancel.IsCancellationRequested)
        {
            var key = Console.ReadKey(true).Key;
            _keyPressed.OnNext(ConsoleKey.None);
            _keyPressed.OnNext(key);
        }
        
        return ValueTask.CompletedTask;
    }
    
    private static VehicleAction ConvertKeyToAction(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.RightArrow => VehicleAction.GoRight,
            ConsoleKey.LeftArrow => VehicleAction.GoLeft,
            ConsoleKey.UpArrow => VehicleAction.GoForward,
            ConsoleKey.DownArrow => VehicleAction.GoBackwards,
            ConsoleKey.U => VehicleAction.Upward,
            ConsoleKey.D => VehicleAction.Downward,
            ConsoleKey.PageUp => VehicleAction.PageUp,
            ConsoleKey.PageDown => VehicleAction.PageDown,
            ConsoleKey.Q => VehicleAction.Quit,
            ConsoleKey.T => VehicleAction.TakeOff,
            _ => VehicleAction.Unknown
        };
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        
        KeyPressed.Dispose();
        _keyPressed.Dispose();
        
        _isDisposed = true;
    }
}

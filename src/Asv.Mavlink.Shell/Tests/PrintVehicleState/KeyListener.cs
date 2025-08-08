using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink.Shell;

public class KeyListener: IDisposable
{
    private bool _isDisposed;

    private readonly ReactiveProperty<ConsoleKey> _keyPressed;
    public ReadOnlyReactiveProperty<VehicleAction> KeyPressed { get; }

    public KeyListener()
    {
        _keyPressed = new ReactiveProperty<ConsoleKey>();
        KeyPressed = _keyPressed.Select(ConvertKeyToAction).ToReadOnlyReactiveProperty();
    }
    
    public Task ListenAsync(CancellationToken cancel)
    {
        if (cancel.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }
        
        while (!cancel.IsCancellationRequested)
        {
            var key = Console.ReadKey(true).Key;
            _keyPressed.OnNext(ConsoleKey.None);
            _keyPressed.OnNext(key);
        }
        
        return Task.CompletedTask;
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
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    
    private void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            KeyPressed.Dispose();
            _keyPressed.Dispose();
        }

        _isDisposed = true;
    }
    
    ~KeyListener()
    {
        Dispose(disposing: false);
    }
}

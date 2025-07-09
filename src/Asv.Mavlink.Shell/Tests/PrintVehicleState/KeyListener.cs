using System;
using System.Threading.Tasks;

namespace Asv.Mavlink.Shell;

public class KeyListener
{
    private bool _isRunning;

    public event Func<VehicleAction, Task>? OnDirectionPressed;
    
    public async Task ListenAsync()
    {
        _isRunning = true;
        while (_isRunning)
        {
            var key = Console.ReadKey(true).Key;

            if (!TryMapKeyToDirection(key, out var direction))
                continue;

            if (OnDirectionPressed is not null)
            {
                await OnDirectionPressed.Invoke(direction);
            }
        }
    }


    public void Stop() => _isRunning = false;

    private static bool TryMapKeyToDirection(ConsoleKey key, out VehicleAction direction)
    {
        switch (key)
        {
            case ConsoleKey.RightArrow:
                direction = VehicleAction.GoRight; return true;
            case ConsoleKey.LeftArrow:
                direction = VehicleAction.GoLeft; return true;
            case ConsoleKey.UpArrow:
                direction = VehicleAction.GoUp; return true;
            case ConsoleKey.DownArrow:
                direction = VehicleAction.GoDown; return true;
            case ConsoleKey.U:
                direction = VehicleAction.Upward; return true;
            case ConsoleKey.D:
                direction = VehicleAction.Downward; return true;
            case ConsoleKey.PageUp:
                direction = VehicleAction.PageUp; return true;
            case ConsoleKey.PageDown:
                direction = VehicleAction.PageDown; return true;
            case ConsoleKey.Q:
                direction = VehicleAction.Quit; return true;
            case ConsoleKey.T:
                direction = VehicleAction.TakeOff; return true;
            default:
                direction = default; return false;
        }
    }
}

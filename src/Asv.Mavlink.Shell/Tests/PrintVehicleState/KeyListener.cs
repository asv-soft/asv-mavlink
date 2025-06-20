using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asv.Mavlink.Shell;

public class KeyListener
{
    private VehicleDirection _lastCommand;
    public event Func<VehicleDirection, Task>? OnKeyPressedAsync;
    
    /// <summary>
    /// Continuously listens for keyboard input and triggers the corresponding vehicle direction event.
    /// Supports specific keys mapped to directional and control commands.
    /// </summary>
    /// <returns>A task representing the asynchronous key listening operation.</returns>
    /// <remarks>
    /// When a supported key is pressed, the <see cref="OnKeyPressedAsync"/> event is invoked with the corresponding <see cref="VehicleDirection"/>.
    /// </remarks>
    public async Task ListenAsync()    
    {
        while (true)
        {
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    _lastCommand = VehicleDirection.Right;
                    break;
                case ConsoleKey.LeftArrow:
                    _lastCommand = VehicleDirection.Left;
                    break;
                case ConsoleKey.UpArrow:
                    _lastCommand = VehicleDirection.Up;
                    break;
                case ConsoleKey.DownArrow:
                    _lastCommand = VehicleDirection.Down;
                    break;
                case ConsoleKey.U:
                    _lastCommand = VehicleDirection.U;
                    break;
                case ConsoleKey.D:
                    _lastCommand = VehicleDirection.D;
                    break;
                case ConsoleKey.PageUp:
                    _lastCommand = VehicleDirection.PageUp;
                    break;
                case ConsoleKey.PageDown:
                    _lastCommand = VehicleDirection.PageDown;
                    break;
                case ConsoleKey.Q:
                    _lastCommand = VehicleDirection.Q;
                    break;
                case ConsoleKey.T:
                    _lastCommand = VehicleDirection.T;
                    break;
            }

            if (OnKeyPressedAsync is not null)
            {
                await OnKeyPressedAsync.Invoke(_lastCommand);
            }
        }
    }
    
    /// <summary>
    /// Waits asynchronously until the user presses one of the specified valid keys.
    /// </summary>
    /// <param name="validKeys">A collection of allowed <see cref="ConsoleKey"/> values. If empty, any key is accepted.</param>
    /// <returns>
    /// A task that completes when a valid key is pressed, returning the pressed <see cref="ConsoleKey"/>.
    /// </returns>
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

}
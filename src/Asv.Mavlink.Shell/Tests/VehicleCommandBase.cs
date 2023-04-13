using System.Threading;
using System.Threading.Tasks;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    public abstract class VehicleCommandBase : ConsoleCommand
    {
        private string _connectionString = "udp://0.0.0.0:14560";
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        protected VehicleCommandBase()
        {
            HasOption("cs=", $"Connection string. Default '{_connectionString}'", _ => _connectionString = _);
        }

        public override int Run(string[] remainingArguments)
        {
            Vehicle = CreateVehicle(_connectionString);
            return RunAsync(Vehicle).Result;
        }

        public IVehicleClient Vehicle { get; private set; }

        protected abstract IVehicleClient CreateVehicle(string cs);

        protected abstract Task<int> RunAsync(IVehicleClient vehicle);
        
    }



}

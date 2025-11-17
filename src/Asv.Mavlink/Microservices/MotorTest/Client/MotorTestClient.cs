using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public abstract class MotorTestClient(MavlinkClientIdentity identity, IMavlinkContext core)
	: MavlinkMicroserviceClient("MOTOR_TEST", identity, core), IMotorTestClient
{
	public abstract IReadOnlyObservableList<ITestMotor> TestMotors { get; }
	public abstract Task Refresh(CancellationToken cancellationToken = default);

	protected override async Task InternalInit(CancellationToken cancel)
	{
		await base.InternalInit(cancel).ConfigureAwait(false);
		await Refresh(cancel).ConfigureAwait(false);
	}
}
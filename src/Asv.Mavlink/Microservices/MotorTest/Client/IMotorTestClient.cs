using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public interface IMotorTestClient
{
	IReadOnlyObservableList<ITestMotor> TestMotors { get; }
	Task Refresh(CancellationToken cancellationToken = default);
}
using ObservableCollections;

namespace Asv.Mavlink;

public interface IMotorTestClient
{
	IReadOnlyObservableList<ITestMotor> TestMotors { get; }
}
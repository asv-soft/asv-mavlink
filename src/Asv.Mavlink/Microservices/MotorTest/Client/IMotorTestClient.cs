using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

public interface IMotorTestClient
{
	Task<MavResult> StartMotor(int motor, int throttleValue, int timeout, CancellationToken cancel = default);
	Task<MavResult> StopMotor(int motor, CancellationToken cancel = default);
	ReadOnlyReactiveProperty<MotorsTelemetry> MotorsTelemetry { get; }
	Observable<MotorPwm> GetMotorPwm(int motor);
}
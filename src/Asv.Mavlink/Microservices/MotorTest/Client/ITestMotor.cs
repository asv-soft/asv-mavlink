using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

public interface ITestMotor
{
	int Id { get; }
	int ServoChannel { get; }
	ReadOnlyReactiveProperty<bool> IsTestRun { get; }
	ReadOnlyReactiveProperty<ushort> Pwm { get; }
	Task<MavResult> StartTest(int throttle, int timeout, CancellationToken cancel = default);
	Task<MavResult> StopTest(CancellationToken cancel = default);
}
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

internal interface IMotorTestRunner
{
	public ReadOnlyReactiveProperty<bool> IsTestRun { get; }
	Task<MavResult> StartTest(int motorId, int throttleValue, int timeout, CancellationToken cancel = default);
	Task<MavResult> StopTest(int motorId, CancellationToken cancel = default);
}
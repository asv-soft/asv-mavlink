using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public class WorkModeIdleSuccess : WorkModeStatus
    {
        public override WorkModeStatusClassEnum Class => WorkModeStatusClassEnum.Success;
        public override byte Id => 0;
        public override string Name => "Ok";
        public override string Description => "Ok";
    }

    public class WorkModeIdleLoading : WorkModeStatus
    {
        public override WorkModeStatusClassEnum Class => WorkModeStatusClassEnum.Success;
        public override byte Id => 1;
        public override string Name => "Loading";
        public override string Description => "Loading state";
    }

    public class WorkModeIdleError : WorkModeStatus
    {
        public override WorkModeStatusClassEnum Class => WorkModeStatusClassEnum.Error;
        public override byte Id => 2;
        public override string Name => "Error";
        public override string Description => "Error status";
    }

    public class WorkModeIdle : IWorkModeFactory, IWorkMode
    {
        public static readonly WorkModeStatus SuccessStatus = new WorkModeIdleSuccess();
        public static readonly WorkModeStatus LoadingStatus = new WorkModeIdleLoading();
        public static readonly WorkModeStatus ErrorStatus = new WorkModeIdleError();
        private static readonly WorkModeStatus[] StatusList = { SuccessStatus, LoadingStatus, ErrorStatus };
        private readonly RxValue<WorkModeStatus> _status = new();

        public WorkModeIdle()
        {
            _status.Value = LoadingStatus;
        }

        public void Dispose()
        {
            // ignore
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public IRxValue<WorkModeStatus> Status => _status;
        public string Name => "IDLE";
        public string Description => "Default mode with do nothing";
        public WorkModeStatus[] AvailableStatus => StatusList;
        public IconTypeEnum Icon => IconTypeEnum.Sleep;

        public IWorkMode Create()
        {
            return this;
        }

        public void SetErrorStatus()
        {
            _status.Value = ErrorStatus;
        }

        public void SetLoadingStatus()
        {
            _status.Value = LoadingStatus;
        }

        public void SetOkStatus()
        {
            _status.Value = SuccessStatus;
        }
    }
}

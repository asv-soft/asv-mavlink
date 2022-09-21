using System;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public abstract class WorkModeStatus
    {
        public abstract WorkModeStatusClassEnum Class { get; }
        public abstract byte Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
    }

    public interface IWorkMode : IDisposable
    {
        string Name { get; }
        IRxValue<WorkModeStatus> Status { get; }
        Task Init();
    }

    public abstract class WorkModeBase : DisposableOnceWithCancel, IWorkMode
    {
        private readonly RxValue<WorkModeStatus> _status = new();

        protected WorkModeBase(WorkModeStatus defaultStatus)
        {
            _status.Value = defaultStatus;
            Disposable.Add(_status);
        }

        protected IRxEditableValue<WorkModeStatus> EditableStatus => _status;

        public abstract string Name { get; }

        public virtual Task Init()
        {
            return Task.CompletedTask;
        }

        public IRxValue<WorkModeStatus> Status => _status;
    }

    public interface IWorkModeFactory
    {
        public string Name { get; }
        public string Description { get; }
        public WorkModeStatus[] AvailableStatus { get; }
        IconTypeEnum Icon { get; }

        IWorkMode Create();
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Client
{

    public class ParamsClientConfig
    {
        public int TimeoutToReadAllParamsMs { get; set; } = (int) TimeSpan.FromSeconds(60).TotalMilliseconds;
        public int ReadWriteTimeoutMs { get; set; } = 10000;
    }

    public class ParamsClient : MavlinkMicroserviceClient, IParamsClient
    {
        public readonly ParamsClientConfig _config;
        private readonly ConcurrentDictionary<string, MavParam> _params = new();
        private readonly Subject<MavParam> _paramUpdated = new();
        private readonly RxValue<int?> _paramsCount = new();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ParamsClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, ParamsClientConfig config, IScheduler scheduler):
            base(connection,identity,seq,"PARAMS", scheduler)
            
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            Converter = new MavParamArdupilotValueConverter();
            InternalFilter<ParamValuePacket>().Subscribe(UpdateParam).DisposeItWith(Disposable);
        }

        public IReadOnlyDictionary<string, MavParam> Params => _params;
        public IRxValue<int?> ParamsCount => _paramsCount;
        public IObservable<MavParam> OnParamUpdated => _paramUpdated;

        public Task RequestAllParams(CancellationToken cancel)
        {
            _logger.Info($"{LogSend} Request all params from vehicle");
            return InternalSend<ParamRequestListPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;

            }, cancel);
        }

        public async Task ReadAllParams(CancellationToken cancel, IProgress<double> progress = null)
        {
            progress = progress ?? new Progress<double>();

            _logger.Info($"{LogSend} Request all params from vehicle");
            await InternalSend<ParamRequestListPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
            }, cancel).ConfigureAwait(false);

            var samplesBySecond = InternalFilter<ParamValuePacket>().Buffer(TimeSpan.FromSeconds(1)).Next();

            


            var timeout = DateTime.Now + TimeSpan.FromMilliseconds(_config.TimeoutToReadAllParamsMs);

            int? totalCnt = null;
            progress.Report(0);
            var paramsNames = new HashSet<string>();
            foreach (var paramsPart in samplesBySecond)
            {
                if (DateTime.Now >= timeout)
                {
                    throw new TimeoutException(string.Format(RS.Vehicle_ReadAllParams_Timeout_to_read_all_params_from_Vehicle, _config.TimeoutToReadAllParamsMs));
                }
                foreach (var p in paramsPart)
                {
                    totalCnt = totalCnt ?? p.Payload.ParamCount;
                    var name = GetParamName(p.Payload);
                    paramsNames.Add(name);
                }
                if (totalCnt.HasValue && totalCnt.Value <= paramsNames.Count) break;
                var val = totalCnt == null ? 0 : Math.Min(1d, paramsNames.Count / (double) totalCnt);
                progress.Report(val);
                _logger.Trace($"{LogRecv} Request all params from vehicle: {val:P0}");
            }
            _logger.Trace($"{LogRecv} Request all params from vehicle: SUCCESS");
            progress.Report(1);
        }

        public async Task<MavParam> ReadParam(string name, int attemptCount, CancellationToken cancel)
        {
            _logger.Info($"Read param by name '{name}': BEGIN");
            var packet = InternalGeneratePacket<ParamRequestReadPacket>();
            packet.Payload.TargetComponent = Identity.TargetComponentId;
            packet.Payload.TargetSystem = Identity.TargetSystemId;
            packet.Payload.ParamId = SetParamName(name);
            packet.Payload.ParamIndex = -1;
        
            byte currentAttempt = 0;
            MavParam result = null;
            while (currentAttempt < attemptCount)
            {
                ++currentAttempt;
                
                using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel);
                linkedCancel.CancelAfter(_config.ReadWriteTimeoutMs);
                var tcs = new TaskCompletionSource<MavParam>();
                using var c1 = cancel.Register(()=>tcs.TrySetCanceled());
                try
                {
                    using var subscribe = OnParamUpdated.FirstAsync(_ => _.Name == name)
                        .Subscribe(_=>tcs.TrySetResult(_));
                    await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
                    result = await tcs.Task.ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    _logger.Warn($"Read param by name '{name}': TIMEOUT {currentAttempt} of {attemptCount}");
                    if (cancel.IsCancellationRequested)
                    {
                        throw;
                    }
                }
                _logger.Debug($"Read param by name '{name}': SUCCEES ({result})");
                return result;
            }

            if (result == null)
            {
                var ex = new TimeoutException(string.Format("Timeout to read param '{0}' with '{1}' attempts (timeout {1} times by {2:g} )", name, currentAttempt, TimeSpan.FromMilliseconds(_config.ReadWriteTimeoutMs)));
                _logger.Error($"Read param by name '{name}': {ex.Message}");
                throw ex;
            }
            return result;
        }

        public async Task<MavParam> ReadParam(short index, int attemptCount, CancellationToken cancel)
        {
            _logger.Info($"Begin read param by index '{index}': BEGIN");
            var packet = InternalGeneratePacket<ParamRequestReadPacket>();
            packet.Payload.TargetComponent = Identity.TargetComponentId;
            packet.Payload.TargetSystem = Identity.TargetSystemId;
            packet.Payload.ParamId = SetParamName(string.Empty);
            packet.Payload.ParamIndex = index;
            
            byte currentAttempt = 0;
            MavParam result = null;
            while (currentAttempt < attemptCount)
            {
                ++currentAttempt;

                using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel);
                linkedCancel.CancelAfter(_config.ReadWriteTimeoutMs);
                var tcs = new TaskCompletionSource<MavParam>();
                using var c1 = linkedCancel.Token.Register(()=>tcs.TrySetCanceled());
                try
                {
                    using var subscribe = OnParamUpdated.FirstAsync(_ => _.Index == index)
                        .Subscribe(_=>tcs.TrySetResult(_));
                    await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
                    result = await tcs.Task.ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    _logger.Warn($"Read param by index '{index}': TIMEOUT {currentAttempt} of {attemptCount}");
                    if (cancel.IsCancellationRequested)
                    {
                        throw;
                    }
                }
                return result;
            }

            if (result == null)
            {
                var ex = new TimeoutException(string.Format("Timeout to read param with index '{0}' with '{1}' attempts (timeout {1} times by {2:g} )", index, currentAttempt, TimeSpan.FromMilliseconds(_config.ReadWriteTimeoutMs)));
                _logger.Error($"Read param by index '{index}': {ex.Message}");
                throw ex;
            }
            return result;
        }

        public IMavParamValueConverter Converter { get; set; }

        public async Task<MavParam> WriteParam(MavParam param, int attemptCount, CancellationToken cancel)
        {
            _logger.Info($"Write param '{param}': BEGIN");
            var packet = InternalGeneratePacket<ParamSetPacket>();
            packet.Payload.TargetComponent = Identity.TargetComponentId;
            packet.Payload.TargetSystem = Identity.TargetSystemId;
            packet.Payload.ParamId = SetParamName(string.Empty);
            packet.Payload.ParamType = param.Type;
            packet.Payload.ParamValue = Converter.ConvertToMavlinkUnionToParamValue(param);

           
            var currentAttempt = 0;
            MavParam result = null;
            while (currentAttempt < attemptCount)
            {
                ++currentAttempt;
                using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel);
                linkedCancel.CancelAfter(_config.ReadWriteTimeoutMs);
                var tcs = new TaskCompletionSource<MavParam>();
                using var c1 = linkedCancel.Token.Register(()=>tcs.TrySetCanceled());
                try
                {
                    using var subscribe = OnParamUpdated.FirstAsync(_ => _.Name == param.Name).Subscribe(_=>tcs.TrySetResult(_));
                    await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
                    result = await tcs.Task.ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    _logger.Warn($"Write param '{param}': TIMEOUT {currentAttempt} of {attemptCount}");
                    if (cancel.IsCancellationRequested)
                    {
                        throw;
                    }
                }
                return result;
            }

            if (result == null)
            {
                var ex = new TimeoutException(string.Format("Timeout to write param '{0}' with '{1}' attempts (timeout {1} times by {2:g} )", param.Name, currentAttempt, TimeSpan.FromMilliseconds(_config.ReadWriteTimeoutMs)));
                _logger.Error($"Write param '{param}': {ex.Message}");
                throw ex;
            }
            return result;

        }

        

       

        private char[] SetParamName(string name)
        {
            return name.PadRight(16, '\0').ToCharArray();
        }

        private void UpdateParam(ParamValuePacket p)
        {
            try
            {
                var name = GetParamName(p.Payload);

                float? floatVal;
                long? longVal;
                Converter.ConvertFromMavlinkUnionToParamValue(p.Payload.ParamValue, p.Payload.ParamType, out floatVal, out longVal);
                var mavParam = new MavParam(p.Payload.ParamIndex, name, p.Payload.ParamType, floatVal, longVal);
                _params.AddOrUpdate(name, mavParam, (s, param) => mavParam);
                _paramUpdated.OnNext(mavParam);
                _paramsCount.OnNext(p.Payload.ParamCount);
                _logger.Trace($"Recieve new param: {mavParam}");
            }
            catch (Exception e)
            {
                _logger.Warn($"Recieve MavLink param '{p.Name}' with error:{e.Message}");
            }
            
        }

        

        

        private string GetParamName(ParamValuePayload payload)
        {
            return new string(payload.ParamId.Where(_ => _ != '\0').ToArray());
        }

        protected override void InternalDisposeOnce()
        {
            base.InternalDisposeOnce();
            _paramUpdated?.OnCompleted();
            _paramUpdated?.Dispose();

            _paramsCount?.OnCompleted();
            _paramsCount?.Dispose();

            _params.Clear();
        }
    }
}

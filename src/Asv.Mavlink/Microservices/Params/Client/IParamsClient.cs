using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Client
{
    public interface IParamsClient
    {
        IMavParamValueConverter Converter { get; set; }

        IReadOnlyDictionary<string, MavParam> Params { get; }
        IRxValue<int?> ParamsCount { get; }
        IObservable<MavParam> OnParamUpdated { get; }
        /// <summary>
        /// Send request params
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task RequestAllParams(CancellationToken cancel);
        /// <summary>
        /// Not work on Ardupilot (params_count field is wrong)
        /// </summary>
        /// <param name="cancel"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        Task ReadAllParams(CancellationToken cancel, IProgress<double> progress = null);
        Task<MavParam> ReadParam(string name, int attemptCount, CancellationToken cancel);
        Task<MavParam> ReadParam(short index, int attemptCount, CancellationToken cancel);
        Task<MavParam> WriteParam(MavParam param, int attemptCount, CancellationToken cancel);
    }
}

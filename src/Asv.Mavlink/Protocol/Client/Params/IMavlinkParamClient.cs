using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Client
{
    public interface IMavlinkParameterClient
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

    public static class VehicleParamHelper
    {
        public const int DefaultAttemptCount = 3;

        public static Task<MavParam> ReadParam(this IMavlinkParameterClient src, string name, CancellationToken cancel)
        {
            return src.ReadParam(name, DefaultAttemptCount, cancel);
        }

        public static Task<MavParam> ReadParam(this IMavlinkParameterClient src, short index, CancellationToken cancel)
        {
            return src.ReadParam(index, DefaultAttemptCount, cancel);
        }

        public static Task<MavParam> WriteParam(this IMavlinkParameterClient src, MavParam param, CancellationToken cancel)
        {
            return src.WriteParam(param, DefaultAttemptCount, cancel);
        }

        public static async Task<MavParam> WriteParam(this IMavlinkParameterClient src, string name, float value, CancellationToken cancel)
        {
            MavParam param;
            if (!src.Params.TryGetValue(name, out param))
            {
                param = await src.ReadParam(name, cancel).ConfigureAwait(false);
            }

            return await src.WriteParam(new MavParam(param, value), cancel).ConfigureAwait(false);
        }

        public static async Task<MavParam> WriteParam(this IMavlinkParameterClient src, string name, long value, CancellationToken cancel)
        {
            MavParam param;
            if (!src.Params.TryGetValue(name, out param))
            {
                param = await src.ReadParam(name, cancel).ConfigureAwait(false);
            }

            return await src.WriteParam(new MavParam(param, value), cancel).ConfigureAwait(false);
        }


        public static async Task<MavParam> GetOrReadFromVehicleParam(this IMavlinkParameterClient src, string name, CancellationToken cancel)
        {
            MavParam value;
            if (src.Params.TryGetValue(name, out value)) return value;
            var param = await src.ReadParam(name, DefaultAttemptCount, cancel).ConfigureAwait(false);
            return param;
        }


    }

}

using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink.Client;

public static class ParamsHelper
{
    public const int DefaultAttemptCount = 3;

    public static Task<MavParam> ReadParam(this IParamsClient src, string name, CancellationToken cancel)
    {
        return src.ReadParam(name, DefaultAttemptCount, cancel);
    }

    public static Task<MavParam> ReadParam(this IParamsClient src, short index, CancellationToken cancel)
    {
        return src.ReadParam(index, DefaultAttemptCount, cancel);
    }

    public static Task<MavParam> WriteParam(this IParamsClient src, MavParam param, CancellationToken cancel)
    {
        return src.WriteParam(param, DefaultAttemptCount, cancel);
    }

    public static async Task<MavParam> WriteParam(this IParamsClient src, string name, float value, CancellationToken cancel)
    {
        MavParam param;
        if (!src.Params.TryGetValue(name, out param))
        {
            param = await src.ReadParam(name, cancel).ConfigureAwait(false);
        }

        return await src.WriteParam(new MavParam(param, value), cancel).ConfigureAwait(false);
    }

    public static async Task<MavParam> WriteParam(this IParamsClient src, string name, long value, CancellationToken cancel)
    {
        MavParam param;
        if (!src.Params.TryGetValue(name, out param))
        {
            param = await src.ReadParam(name, cancel).ConfigureAwait(false);
        }

        return await src.WriteParam(new MavParam(param, value), cancel).ConfigureAwait(false);
    }


    public static async Task<MavParam> GetOrReadFromVehicleParam(this IParamsClient src, string name, CancellationToken cancel)
    {
        MavParam value;
        if (src.Params.TryGetValue(name, out value)) return value;
        var param = await src.ReadParam(name, DefaultAttemptCount, cancel).ConfigureAwait(false);
        return param;
    }


}
using Asv.Common;

namespace Asv.Mavlink
{
    public interface IVehiclePlane : IVehicle
    {
        IRxValue<double?> PlaneCircleRadius { get; }
    }
}

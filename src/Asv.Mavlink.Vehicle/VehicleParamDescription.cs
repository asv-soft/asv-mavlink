using System.Collections.Generic;

namespace Asv.Mavlink
{

    public class VehicleParamValue
    {
        public decimal Code { get; set; }
        public string Description { get; set; }
    }

    public class VehicleParamDescription
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string  Description { get; set; }
        public List<VehicleParamValue> AvailableValues { get; } = new List<VehicleParamValue>();
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public string Units { get; set; }
        public string UnitsDisplayName { get; set; }
        public string GroupName { get; set; }
        public string User { get; set; }
        public decimal? Increment { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRebootRequired { get; set; }
        public string Values { get; set; }
        public int Calibration { get; set; }
        public string BoardType { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
namespace Asv.Mavlink.Shell;

public class AdsbCommandConfig
{
    public static readonly AdsbCommandConfig Default = new()
    {
        SystemId = 1,
        ComponentId = 240,
        Ports =
        [
            "tcp://127.0.0.1:5760",
            "tcps://127.0.0.1:7341"
        ],
        Vehicles =
        [
            new AdsbCommandVehicleConfig
            {
                CallSign = "PLANE1",
                UpdateRateMs = 500,
                IcaoAddress = 1234,
                Squawk = 777,
                Route =
                [
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.305641",
                        Lon = "61.500886",
                        Alt = 250,
                        Velocity = 10,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.362666",
                        Lon = "61.210466",
                        Alt = 1000,
                        Velocity = 300,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.276028",
                        Lon = "61.135319",
                        Alt = 500,
                        Velocity = 250,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.166806",
                        Lon = "61.691734",
                        Alt = 500,
                        Velocity = 600,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.268774",
                        Lon = "61.760482",
                        Alt = 400,
                        Velocity = 600,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.303278",
                        Lon = "61.518754",
                        Alt = 250,
                        Velocity = 10,
                    }
                ]
            },
            new AdsbCommandVehicleConfig
            {
                CallSign = "PLANE2",
                UpdateRateMs = 500,
                IcaoAddress = 4321,
                Squawk = 666,
                Route =
                [
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.303278",
                        Lon = "61.518754",
                        Alt = 250,
                        Velocity = 10,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.268774",
                        Lon = "61.760482",
                        Alt = 400,
                        Velocity = 600,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.166806",
                        Lon = "61.691734",
                        Alt = 500,
                        Velocity = 600,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.276028",
                        Lon = "61.135319",
                        Alt = 500,
                        Velocity = 250,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.362666",
                        Lon = "61.210466",
                        Alt = 1000,
                        Velocity = 300,
                    },
                    new AdsbCommandVehicleRouteItemConfig
                    {
                        Lat = "55.305641",
                        Lon = "61.500886",
                        Alt = 250,
                        Velocity = 10,
                    }
                ]
            }
        ]
    };

    public string[] Ports { get; set; } = [];
    public AdsbCommandVehicleConfig[] Vehicles { get; set; } = [];
    public byte SystemId { get; set; }
    public byte ComponentId { get; set; }
}

public class AdsbCommandVehicleConfig
{
    public string CallSign { get; set; } = string.Empty;
    public ushort Squawk { get; set; }
    public AdsbCommandVehicleRouteItemConfig[] Route { get; set; } = [];
    public int UpdateRateMs { get; set; }
    public uint IcaoAddress { get; set; }
}

public class AdsbCommandVehicleRouteItemConfig
{
    public string? Lat { get; set; }
    public string? Lon { get; set; }
    public double Alt { get; set; }
    public double Velocity { get; set; }
}
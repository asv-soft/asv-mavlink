using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Cfg.ImMemory;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Client;
using Asv.Mavlink.Payload.Digits;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Icarous;
using Asv.Mavlink.V2.Minimal;
using Asv.Mavlink.V2.Uavionix;

namespace Asv.Mavlink.Payload.Test
{
    public static class PayloadV2TestHelper
    {
        private static readonly IPEndPoint DefaultLoopbackEndpoint = new IPEndPoint(IPAddress.Loopback, port: 0);

        public static int GetAvailablePort()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(DefaultLoopbackEndpoint);
                return ((IPEndPoint)socket.LocalEndPoint).Port;
            }
        }

        public static IPayloadV2Server CreateServer(out int port, byte compId = 1,byte sysId = 1)
        {
            port = GetAvailablePort();
            var serverPort = PortFactory.Create($"tcp://127.0.0.1:{port}?srv=true");
            serverPort.Enable();

            var serverConnection = new MavlinkV2Connection(serverPort, _ =>
            {
                _.RegisterMinimalDialect();
                _.RegisterCommonDialect();
                _.RegisterArdupilotmegaDialect();
                _.RegisterIcarousDialect();
                _.RegisterUavionixDialect();
            });
            var mavlinkServer = new MavlinkServerBase(serverConnection,
                new MavlinkServerIdentity { ComponentId = compId, SystemId = sysId });
            mavlinkServer.Heartbeat.Set(_ => _.Autopilot = MavAutopilot.MavAutopilotGeneric);
            mavlinkServer.Heartbeat.Start();
            return new PayloadV2Server(mavlinkServer);
        }

        public static PayloadV2Client CreateClient(int port, byte comId = 255,byte sysId = 255,byte targetCompId = 1,byte targetSysId =1 )
        {
            var clientPort = PortFactory.Create($"tcp://127.0.0.1:{port}");
            clientPort.Enable();
            var clientConnection = new MavlinkV2Connection(clientPort, _ =>
            {
                _.RegisterMinimalDialect();
                _.RegisterCommonDialect();
                _.RegisterArdupilotmegaDialect();
                _.RegisterIcarousDialect();
                _.RegisterUavionixDialect();
            });
            var mavlinkClient = new MavlinkClient(clientConnection, new MavlinkClientIdentity
            {
                ComponentId = 255,
                SystemId = 255,
                TargetComponentId = 1,
                TargetSystemId = 1,
            }, new MavlinkClientConfig());
            return new PayloadV2Client(mavlinkClient);
        }


        public static void CreateParams(out IPv2ClientParamsInterface clientParams,
            out IPv2ServerParamsInterface serverParams, IEnumerable<Pv2ParamType> paramsList)
        {
            var cfg = new InMemoryConfiguration();
            var server = CreateServer(out var port);
            var client = CreateClient(port);
            clientParams = new Pv2ClientParamsInterface(client, Pv2CfgDescriptionEmptyStore.Default, cfg);
            serverParams = new Pv2ServerParamsInterface(server, cfg, paramsList);
            WaitUntilConnect(client);
        }

        public static void WaitUntilConnect(PayloadV2Client client)
        {
            client.Client.Heartbeat.Link.Where(_ => _ == LinkState.Connected).FirstAsync().Wait();
        }



        public static IPv2ServerDevice CreateServerDevice(out int port, IEnumerable<Pv2RttRecordDesc> records,
            IEnumerable<Pv2RttFieldDesc> fields, out string rttFolder, uint writeRttTimeMs, byte comId = 1, byte sysId = 1)
        {
            port = GetAvailablePort();
            var serverPort = PortFactory.Create($"tcp://127.0.0.1:{port}?srv=true");
            serverPort.Enable();

            var serverConnection = new MavlinkV2Connection(serverPort, _ =>
            {
                _.RegisterMinimalDialect();
                _.RegisterCommonDialect();
                _.RegisterArdupilotmegaDialect();
                _.RegisterIcarousDialect();
                _.RegisterUavionixDialect();
            });
            var config = new InMemoryConfiguration();
            var suffix = "PV3";
            Pv2DeviceParams.SystemId.WriteToConfigValue(config, suffix, sysId);
            Pv2DeviceParams.ComponentId.WriteToConfigValue(config, suffix, comId);
            Pv2RttInterface.RecordTickTime.WriteToConfigValue(config,suffix, writeRttTimeMs);
            rttFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return new Pv2ServerDeviceBase(serverConnection, config,
                Array.Empty<Pv2ParamType>(), Pv2DeviceClass.PayloadSdr, Array.Empty<IWorkModeFactory>(), new ChunkFileStore(rttFolder), records, fields, suffix);
        }

        public static IPv2ClientDevice CreateClientDevice(int port, byte comId = 255, byte sysId = 255, byte targetCompId = 1, byte targetSysId = 1)
        {
            var serverPort = PortFactory.Create($"tcp://127.0.0.1:{port}");
            serverPort.Enable();

            var serverConnection = new MavlinkV2Connection(serverPort, _ =>
            {
                _.RegisterMinimalDialect();
                _.RegisterCommonDialect();
                _.RegisterArdupilotmegaDialect();
                _.RegisterIcarousDialect();
                _.RegisterUavionixDialect();
            });
            return new Pv2ClientDeviceBase(serverConnection, new MavlinkClientIdentity
            {
                ComponentId = comId,
                SystemId = sysId,
                TargetComponentId = targetCompId,
                TargetSystemId = targetSysId,
            }, new PacketSequenceCalculator(), new Pv2CfgDescriptionEmptyStore(), new Pv2RttDescriptionEmptyStore(),new Pv2BaseDescriptionEmptyStore(), new Pv2DeviceBaseConfig(),true);

        }

        public static async Task<(IPv2ClientDevice,IPv2ServerDevice)> CreateServerAndClientDevices(IEnumerable<Pv2RttRecordDesc> records,
            IEnumerable<Pv2RttFieldDesc> fields, uint writeRttTimeMs, Action<string> logger = null)
        {

            var server = CreateServerDevice(out var port, records, fields, out var rttFolder, writeRttTimeMs);
            logger?.Invoke("Server RTT store folder:"+rttFolder);
            var client = CreateClientDevice(port);
            await client.InitState.Where(_ => _ == VehicleInitState.Complete).FirstAsync();
            return (client, server);
        }
        
    }
}

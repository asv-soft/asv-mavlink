using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Client;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Test
{
    public class MavlinkTestHelper
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

        public static MavlinkServerBase CreateServer(out int port, byte compId = 1,byte sysId = 1)
        {
            port = GetAvailablePort();
            var serverPort = PortFactory.Create($"tcp://127.0.0.1:{port}?srv=true");
            serverPort.Enable();

            var serverConnection = MavlinkV2ConnectionFactory.Create(serverPort);
            var mavlinkServer = new MavlinkServerBase(serverConnection,
                new MavlinkServerIdentity { ComponentId = compId, SystemId = sysId });
            mavlinkServer.Heartbeat.Set(_ => _.Autopilot = MavAutopilot.MavAutopilotGeneric);
            mavlinkServer.Heartbeat.Start();
            return mavlinkServer;
        }

        public static MavlinkClient CreateClient(int port, byte comId = 255,byte sysId = 255,byte targetCompId = 1,byte targetSysId =1 )
        {
            var clientPort = PortFactory.Create($"tcp://127.0.0.1:{port}");
            clientPort.Enable();
            var clientConnection = MavlinkV2ConnectionFactory.Create(clientPort);
            var mavlinkClient = new MavlinkClient(clientConnection, new MavlinkClientIdentity
            {
                ComponentId = 255,
                SystemId = 255,
                TargetComponentId = 1,
                TargetSystemId = 1,
            }, new MavlinkClientConfig());
            return mavlinkClient;
        }
        
        public static void WaitUntilConnect(MavlinkClient client)
        {
            client.Heartbeat.Link.Where(_ => _ == LinkState.Connected).FirstAsync().Wait();
        }
       
        public static async Task<(MavlinkClient,MavlinkServerBase)> CreateServerAndClientDevices()
        {
            var server = CreateServer(out var port);
            var client = CreateClient(port);
            return (client, server);
        }
    }
}

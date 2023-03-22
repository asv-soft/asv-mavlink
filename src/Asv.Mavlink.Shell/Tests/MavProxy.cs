using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using System.Threading;
using Asv.Mavlink.V2.Common;
using ManyConsole;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Asv.Mavlink.Shell
{
    public class MavProxy:ConsoleCommand
    {
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private StreamWriter _out;
        private readonly Subject<ConsoleKeyInfo> _userInput = new Subject<ConsoleKeyInfo>();
        private readonly List<MavlinkV2Connection> _connections = new List<MavlinkV2Connection>();
        private readonly List<int> _sysId = new List<int>();
        private readonly List<int> _msgId = new List<int>();
        private readonly List<int> _dirIndex = new List<int>();
        private Regex _nameFilter;
        private bool _silentMode;
        private Regex _textFilter;


        public MavProxy()
        {
            IsCommand("proxy", "Used for connecting vehicle and several ground station");
            HasLongDescription("Used for connecting vehicle and several ground station\n" +
                               "Example: proxy -l=udp://192.168.0.140:14560 -l=udp://192.168.0.140:14550 -o=out.txt");
            HasOption("l|link=", $"Add connection to hub. Can used multiple times. Example 'udp://192.168.0.140:45560' or 'serial://COM5?br=57600'", AddLink);
            HasOption("o|out=", "Write filtered message to file", _ => _out = new StreamWriter(_));
            HasOption("silent", "Disable print filtered message to screen", _ => _silentMode = true);

            HasOption("sys=", "Filter for logging: system id field (Example: --sys=1 --sys=255)", (int _) => _sysId.Add(_));
            HasOption("mid=", "Filter for logging: message id field (Example: --mid=1 --mid=255)", (int _) => _msgId.Add(_));
            HasOption("name=", "Filter for logging: regex message name filter (Example: --name=MAV_CMD_D)", _ => _nameFilter = new Regex(_, RegexOptions.Compiled));
            HasOption("txt=", "Filter for logging: regex json text filter (Example: --txt=MAV_CMD_D)", _ => _textFilter = new Regex(_, RegexOptions.Compiled));
            HasOption("from=", "Filter for packet direction: select only input packets from the specified direction (Example:--dir=1)", (int _) => _dirIndex.Add(_));
            _cancel.Token.Register(() => _out?.Dispose());
            _cancel.Token.Register(() => _userInput.Dispose());
        }

        private void AddLink(string connectionString)
        {
            var dirIndex = _connections.Count;
            var conn = new MavlinkV2Connection(connectionString, _ =>
            {
                _.RegisterCommonDialect();
            });

            conn.Subscribe(_ => OnPacket(conn, _, dirIndex), _cancel.Cancel);
            _connections.Add(conn);
        }

        public override int Run(string[] remainingArguments)
        {
            Console.ReadLine();
            return 0;
        }

        private void OnPacket(MavlinkV2Connection receiveFrom, IPacketV2<IPayload> packetV2, int dirIndex)
        {
            string txt;
            if (Filter(packetV2, dirIndex, out txt))
            {
                if (!_silentMode) Console.WriteLine(txt);
                _out?.WriteLine(txt);
            }
            
            foreach (var mavlinkV2Connection in _connections)
            {
                // skip source link
                if (mavlinkV2Connection == receiveFrom) continue;
                mavlinkV2Connection.Send(packetV2, _cancel.Token);
            }
        }

        private bool Filter(IPacketV2<IPayload> packetV2, int dirIndex, out string txt)
        {
            txt = JsonConvert.SerializeObject(packetV2, Formatting.None, new StringEnumConverter());

            if (_sysId.Count != 0 && !_sysId.Contains(packetV2.SystemId)) return false;
            if (_dirIndex.Count != 0 && !_dirIndex.Contains(dirIndex)) return false;
            if (_msgId.Count != 0 && !_msgId.Contains(packetV2.MessageId)) return false;
            if (_nameFilter!=null && !_nameFilter.IsMatch(packetV2.Name)) return false;
            if (_textFilter != null && !_textFilter.IsMatch(txt)) return false;
            return true;
        }
    }




    
}
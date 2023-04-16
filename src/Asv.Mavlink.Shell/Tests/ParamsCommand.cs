using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink.Shell
{
    public class ParamsCommand : VehicleCommandBase
    {
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly Subject<ConsoleKeyInfo> _userInput = new Subject<ConsoleKeyInfo>();
        private int _pageSize = 30;
        private string _search;
        private int _skip;
        private ReadOnlyObservableCollection<IParamItem> _list;

        public ParamsCommand()
        {
            IsCommand("params", "Read all params from Vehicle");
            HasOption("p|pageSize=", $"max size of rows in table. Default={_pageSize}", (int p) => _pageSize = p);
            _userInput.Where(_=>_.Key == ConsoleKey.Backspace && !string.IsNullOrEmpty(_search)).Subscribe(_=>
            {
                _skip = 0;
                _search = _search.Substring(0, _search.Length - 1);
                Redraw();
            });
            _userInput.Where(_ => char.IsLetterOrDigit(_.KeyChar) || _.KeyChar == '_').Subscribe(_ =>
            {
                _skip = 0;
                _search += _.KeyChar;
                Redraw();
            });
            _userInput.Where(_ => _.Key == ConsoleKey.RightArrow).Subscribe(_ =>
            {
                _skip += _pageSize;
                Redraw();
            });
            _userInput.Where(_ => _.Key == ConsoleKey.LeftArrow).Subscribe(_ =>
            {
                _skip -= _pageSize;
                if (_skip < 0) _skip = 0; 
                Redraw();
            });
        }

        private void KeyListen()
        {
            while (!_cancel.IsCancellationRequested)
            {
                var key = Console.ReadKey(true);
                _userInput.OnNext(key);
            }
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.Cancel) _cancel.Cancel(false);
        }

        protected override IVehicleClient CreateVehicle(string cs)
        {
            Task.Factory.StartNew(_ => KeyListen(), _cancel.Token);
            Console.CancelKeyPress += Console_CancelKeyPress;
            return new ArduCopterClient(MavlinkV2Connection.Create(cs), new MavlinkClientIdentity{ComponentId = 15,SystemId = 15,TargetComponentId = 1,TargetSystemId = 1},new VehicleClientConfig(), new PacketSequenceCalculator(),Scheduler.Default);
        }

        protected override async Task<int> RunAsync(IVehicleClient vehicle)
        {
            Vehicle.Params.Items.Filter(_=> _search.IsNullOrWhiteSpace() || _.Name.Contains(_search, StringComparison.InvariantCultureIgnoreCase))
                .Bind(out _list).Subscribe();
            await vehicle.Params.ReadAll(new Progress<double>(_ => Console.WriteLine("Read params progress:" + TextRender.Progress(_, 20))));
            
            while (!_cancel.IsCancellationRequested)
            {
                Redraw();
                await Task.Delay(1000).ConfigureAwait(false);
            }
            return 0;
        }

        private void Redraw()
        {
            Console.Clear();
            Console.WriteLine("Use Left/Right arrows for page navigation (<-|->) and type for search");
            Console.Write("Search:"+_search);
            var top = Console.CursorTop;
            var left = Console.CursorLeft;
            Console.WriteLine();

            
            
            var items =_list.ToArray();
            Console.WriteLine($"Show [{_skip} - {_skip + _pageSize}] of {items.Length}. All {Vehicle.Params.RemoteCount.Value} items: ");
            TextTable.PrintTableFromObject(Console.WriteLine, new DoubleTextTableBorder(), 1, int.MaxValue, items.Skip(_skip).Take(_pageSize) );
            Console.SetCursorPosition(left,top);
        }

    }
}

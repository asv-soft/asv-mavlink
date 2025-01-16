using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class ParamsCommand
    {
        private readonly CancellationTokenSource _cancel = new();
        private readonly Subject<ConsoleKeyInfo> _userInput = new();
        private const int PageSize = 20;
        private string _search = string.Empty;
        private int _skip;
        private Table _paramsTable;
        private Table _paramsPageTable = new Table().AddColumn("");
        private readonly List<ParamItem> _viewedParamItems = [];
        private ParamsClientEx _paramsClientEx;
        private IDeviceExplorer _deviceExplorer;

        public ParamsCommand()
        {
            _userInput.Where(_ => _.Key == ConsoleKey.F6).Subscribe(_ =>
            {
                _paramsClientEx.Dispose();
                _cancel.Cancel();
            });
            _userInput.Where(_ => _.Key == ConsoleKey.F7).Subscribe(async _ =>
            {
                await _paramsClientEx.ReadAll(cancel:_cancel.Token);
                _viewedParamItems.Clear();
                _viewedParamItems.AddRange(_paramsClientEx.Items.Values);
                UpdateOutPut();
            });
            _userInput.Where(_ => _.Key == ConsoleKey.Backspace && !string.IsNullOrEmpty(_search)).Subscribe(_ =>
            {
                _skip = 0;
                _search = _search.Substring(0, _search.Length - 1);
                UpdateOutPut();
            });
            _userInput.Where(_ => char.IsLetterOrDigit(_.KeyChar) || _.KeyChar == '_').Subscribe(_ =>
            {
                _skip = 0;
                _search += _.KeyChar;
                UpdateOutPut();
            });
            _userInput.Where(_ => _.Key == ConsoleKey.RightArrow).Subscribe(_ =>
            {
                _skip += PageSize;
                UpdateOutPut();
            });
            _userInput.Where(_ => _.Key == ConsoleKey.LeftArrow).Subscribe(_ =>
            {
                _skip -= PageSize;
                if (_skip < 0) _skip = 0;
                UpdateOutPut();
            });
        }


        /// <summary>
        /// Vehicle params real time monitoring
        /// </summary>
        /// <param name="connection">-connection, Connection string. Default "tcp://127.0.0.1:7341"</param>
        [Command("params")]
        public int Run(string connection = "tcp://127.0.0.1:7341")
        {
            AnsiConsole.Status().Start("CreateRouter", ctx =>
            {
                ctx.SpinnerStyle(Style.Plain);
                ShellCommandsHelper.CreateDeviceExplorer(connection, out _deviceExplorer);
            });
            var selectedDevice = ShellCommandsHelper.DeviceAwaiter(_deviceExplorer);
            const string status = @"Waiting for init device";
            AnsiConsole.Status().StartAsync(status, statusContext =>
            {
                statusContext.Spinner(Spinner.Known.Bounce);
                while (selectedDevice is { State.CurrentValue: ClientDeviceState.Uninitialized })
                {
                    Task.Delay(TimeSpan.FromMilliseconds(100));
                }

                return Task.CompletedTask;
            });
            if (selectedDevice.Microservices.FirstOrDefault(_ => _ is IParamsClientEx) is not ParamsClientEx
                paramsClient)
                return 0;
            var controlTable = new Table()
                .AddColumns("Left Arrow", "Right Arrow", "F6", "F7")
                .AddRow("Next Page", "Prev. Page", "Quit", "Update Params").Border(TableBorder.Rounded)
                .BorderColor(Color.Green);
            _paramsTable = new Table().AddColumn($"Params of {selectedDevice}");
            _paramsTable.AddRow(_paramsPageTable);
            _paramsTable.AddRow($"Search:{_search}");
            _paramsTable.AddRow(controlTable);
            _paramsClientEx = paramsClient;
            ReadAllParamsStatus(paramsClient);

            var actionThread = new Thread(KeyListen);
            actionThread.Start();
            AnsiConsole.Live(_paramsTable).StartAsync(context =>
            {
                while (_cancel.IsCancellationRequested == false)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(35));
                    context.Refresh();
                }

                return Task.CompletedTask;
            });

            return 0;
        }

        private void ReadAllParamsStatus(IParamsClientEx paramsClientEx)
        {
            var cancel = new CancellationTokenSource();
            AnsiConsole.Progress().Start(progressContext =>
            {
                paramsClientEx.ReadAll(cancel: cancel.Token);
                var task = progressContext.AddTask("Downloading params", new ProgressTaskSettings()
                {
                    AutoStart = true,
                    MaxValue = paramsClientEx.RemoteCount.CurrentValue,
                });
                while (paramsClientEx.LocalCount.CurrentValue != paramsClientEx.RemoteCount.CurrentValue )
                {
                    task.Value = paramsClientEx.LocalCount.CurrentValue;
                }
            });
            if(paramsClientEx.LocalCount.CurrentValue == 0) ReadAllParamsStatus(paramsClientEx);
            _viewedParamItems.AddRange(paramsClientEx.Items.Values);
            UpdateOutPut();
        }


        private void UpdateOutPut()
        {
            _paramsPageTable = new Table().AddColumn($"Page #{_skip / PageSize+1}");
            IEnumerable<ParamItem> result;
            result = _search == string.Empty
                ? _viewedParamItems.Skip(_skip).Take(PageSize)
                : _viewedParamItems.Where(paramItem => paramItem.Name.Contains(_search));
            foreach (var item in result)
            {
                _paramsPageTable.AddRow($@"{item.Name} = {Markup.Escape(item.Value.CurrentValue.ToString())}");
            }

            _paramsTable.UpdateCell(0, 0, _paramsPageTable);
            _paramsTable.UpdateCell(1, 0, $"Search:{_search}");
        }

        private void KeyListen()
        {
            while (!_cancel.IsCancellationRequested)
            {
                var key = Console.ReadKey(true);
                _userInput.OnNext(key);
                UpdateOutPut();
            }
        }
    }
}
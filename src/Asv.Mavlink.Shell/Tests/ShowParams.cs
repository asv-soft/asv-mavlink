using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class ShowParams
    {
        private const string ConnectionString = "tcp://127.0.0.1:5762";
        /// <summary>
        /// Show params for device
        /// </summary>
        /// <param name="cs">-cs, Connection string. Default "tcp://127.0.0.1:5760"</param>
        [Command("params")]
        public async Task Run(string cs = ConnectionString)
        {
            var factory = new ClientDeviceFactory(new MavlinkIdentity(255, 255), [
                new AdsbClientDeviceProvider(new AdsbClientDeviceConfig(), []),
                new GbsClientDeviceProvider(new GbsClientDeviceConfig()),
                new GenericDeviceProvider(new GenericDeviceConfig()),
                new RadioClientDeviceProvider(new RadioClientDeviceConfig()),
                new RfsaClientDeviceProvider(new RfsaClientDeviceConfig()),
                new RsgaClientDeviceProvider(new RsgaClientDeviceConfig()),
                new SdrClientDeviceProvider(new SdrClientDeviceConfig()),
                new ArduCopterClientDeviceProvider(new VehicleClientDeviceConfig()),
                new ArduPlaneClientDeviceProvider(new VehicleClientDeviceConfig()),
                new Px4CopterClientDeviceProvider(new VehicleClientDeviceConfig()),
                new Px4PlaneClientDeviceProvider(new VehicleClientDeviceConfig())
            ]);
            var router = Protocol.Create(builder =>
            {
                builder.RegisterMavlinkV2Protocol();
            }).CreateRouter("ROUTER");
            var core = new CoreServices(router);
            var browser = new ClientDeviceBrowser(factory, new DeviceBrowserConfig(), core);
            
            IClientDevice choice;
            do
            {
                await AnsiConsole.Status()
                    .StartAsync("Refresh devices...", async ctx =>
                    {
                        ImmutableArray<IClientDevice> items;
                        do
                        {
                            items = [..browser.Devices.Select(x => x.Value)];
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        } while (items.Length == 0);
                    }); 
                
                choice = AnsiConsole.Prompt(
                    new SelectionPrompt<IClientDevice>()
                        .Title("Select [green]mavlink devices[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more devices)[/]")
                        .UseConverter(x=>
                        {
                            if (x == null) return "[red]|=> Refresh <=|[/]";
                            return $"[green]{x.Name.CurrentValue.EscapeMarkup()}[/]";
                        })
                        .AddChoices(browser.Devices.Select(x=>x.Value).Concat(new IClientDevice[] { null })));
                
            }
            while(choice == null);

            

        }
    }
}

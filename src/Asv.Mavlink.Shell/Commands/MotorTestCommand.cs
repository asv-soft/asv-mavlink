using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Asv.Mavlink.Shell;

public class MotorTestCommand
{
	[Command("motor-test")]
	public async Task<int> RunMotorTestAsync(string connection, CancellationToken cancellationToken)
	{
		ShellCommandsHelper.CreateDeviceExplorer(connection, out var deviceExplorer);

		var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer, 3000);
		if (device is null)
		{
			AnsiConsole.MarkupLine("[red]error:[/] cannot connect to the device.");
			return 1;
		}

		var motorTestClient = device.GetMicroservice<IMotorTestClient>();
		if (motorTestClient is null)
		{
			AnsiConsole.MarkupLine("[red]error:[/] this device is unsupported yet.");
			return 1;
		}

		var appExit = false;
		cancellationToken.Register(() => appExit = true);

		while (!appExit)
		{
			using var liveCts = new CancellationTokenSource();

			var liveTask = RunLiveAsync(motorTestClient, liveCts.Token);

			var keyTask = Task.Run(async () =>
			{
				while (!liveCts.IsCancellationRequested && !appExit)
				{
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey(intercept: true).Key;
						if (key == ConsoleKey.Q)
						{
							liveCts.Cancel();
							appExit = true;
							break;
						}

						if (key == ConsoleKey.E)
						{
							liveCts.Cancel();
							break;
						}
					}

					await Task.Delay(80);
				}
			});

			await Task.WhenAny(liveTask, keyTask);

			try
			{
				await liveTask;
			}
			catch { }

			if (appExit)
				break;

			Console.Clear();
			AnsiConsole.MarkupLine("[bold cyan]Motor Configuration[/]\n");
			var motorsCount = motorTestClient.TestMotors.Count;

			var motor = AnsiConsole.Ask<int>($"Enter motor number (1-{motorsCount})");
			var throttle = AnsiConsole.Ask<int>("Throttle level, % (0-100)");
			var timeout = AnsiConsole.Ask<int>("Test duration, s");

			var startTask = AnsiConsole.Confirm("Start test?");
			if (!startTask)
				continue;

			var testMotor = motorTestClient.TestMotors.First(m => m.Id == motor);
			AnsiConsole.MarkupLine($"[green]Starting motor test # {motor} ... ({timeout}s)[/]");
			await Task.Run(async () => { await testMotor.StartTest(throttle, timeout, CancellationToken.None); },
				CancellationToken.None);
			AnsiConsole.MarkupLine("[green]Test started.[/]");
		}

		AnsiConsole.MarkupLine("[grey]Exit...[/]");

		return 0;
	}

	private async Task RunLiveAsync(IMotorTestClient motorTestClient, CancellationToken token)
	{
		var initialLayout = BuildLayout(motorTestClient);

		await AnsiConsole.Live(initialLayout)
			.AutoClear(false)
			.StartAsync(async ctx =>
			{
				while (!token.IsCancellationRequested)
				{
					ctx.UpdateTarget(BuildLayout(motorTestClient));

					try
					{
						await Task.Delay(300, token);
					}
					catch (TaskCanceledException)
					{
						break;
					}
				}
			});
	}

	private IRenderable BuildLayout(IMotorTestClient motorTestClient)
	{
		var table = new Table();
		table.AddColumns("[blue]Motor[/]", "[blue]Servo[/]", "[blue]PWM[/]", "[blue]Test[/]");

		foreach (var motor in motorTestClient.TestMotors)
		{
			table.AddRow(
				new Markup($"[blue]{motor.Id}[/]"),
				new Markup($"[blue]{motor.ServoChannel}[/]"),
				new Markup($"[blue]{motor.Pwm.CurrentValue.ToString()}[/]"),
				new Markup($"[green]{motor.IsTestRun}[/]"));
		}

		var footer = new Panel(new Markup("[blue]Press [yellow]E[/] — test, [red]Q[/] — quit[/]"))
		{
			Border = BoxBorder.None,
			Padding = new Padding(0, 1, 0, 0)
		};

		var layout = new Rows(
			table,
			footer
		);

		return layout;
	}
}
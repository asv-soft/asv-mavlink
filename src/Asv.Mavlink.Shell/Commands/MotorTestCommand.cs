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
	/// <summary>
	///     Test motors at any autopilots
	/// </summary>
	/// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
	/// <param name="refreshTimeMs">-t, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
	/// <param name="cancellationToken">Cancellation token</param>
	[Command("motor-test")]
	public async Task<int> RunMotorTestAsync(string connection, int refreshTimeMs = 300, CancellationToken cancellationToken = default)
	{
		ShellCommandsHelper.CreateDeviceExplorer(connection, out var deviceExplorer);

		var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer, (uint)refreshTimeMs);
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

		using var appLifetimeCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		await RunMainLoopAsync(motorTestClient, refreshTimeMs, appLifetimeCts);
		return 0;
	}

	private async Task RunMainLoopAsync(IMotorTestClient motorTestClient, int refreshTimeMs, CancellationTokenSource appLifetimeCts)
	{
		while (!appLifetimeCts.IsCancellationRequested)
		{
			await RunInteractiveSessionAsync(motorTestClient, refreshTimeMs, appLifetimeCts);

			if (appLifetimeCts.IsCancellationRequested)
				break;

			AnsiConsole.Clear();
			await RunMotorConfigurationAsync(motorTestClient, appLifetimeCts.Token);
		}

		AnsiConsole.MarkupLine("[grey]Exit...[/]");
	}

	private async Task RunInteractiveSessionAsync(IMotorTestClient motorTestClient, int refreshTimeMs,
		CancellationTokenSource appLifetimeCts)
	{
		using var sessionCts = CancellationTokenSource.CreateLinkedTokenSource(appLifetimeCts.Token);

		var displayTask = RunLiveDisplayAsync(motorTestClient, refreshTimeMs, sessionCts.Token);
		var inputTask = HandleKeyInputAsync(sessionCts, appLifetimeCts);

		await Task.WhenAny(displayTask, inputTask);

		try
		{
			await displayTask;
		}
		catch
		{
			// ignored
		}
	}

	private async Task HandleKeyInputAsync(CancellationTokenSource sessionCts, CancellationTokenSource appLifetimeCts)
	{
		while (!sessionCts.Token.IsCancellationRequested)
		{
			if (Console.KeyAvailable)
			{
				var key = Console.ReadKey(intercept: true).Key;

				if (key == ConsoleKey.Q)
				{
					await sessionCts.CancelAsync();
					await appLifetimeCts.CancelAsync();
					return;
				}

				if (key == ConsoleKey.E)
				{
					await sessionCts.CancelAsync();
					break;
				}
			}

			try
			{
				await Task.Delay(80, sessionCts.Token).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				break;
			}
		}
	}

	private static async Task RunMotorConfigurationAsync(IMotorTestClient motorTestClient, CancellationToken cancellationToken)
	{
		AnsiConsole.MarkupLine("[bold cyan]Motor Configuration[/]\n");
		var motorsCount = motorTestClient.TestMotors.Count;

		var motor = AnsiConsole.Ask<int>($"Enter motor number (1-{motorsCount})");
		var testMotor = motorTestClient.TestMotors.First(m => m.Id == motor);

		var mode = AnsiConsole.Prompt(
			new SelectionPrompt<MotorTestMode>()
				.Title("Select mode:")
				.AddChoices(MotorTestMode.Start, MotorTestMode.Stop)
				.UseConverter(testMode => testMode switch
				{
					MotorTestMode.Start => "Start test",
					MotorTestMode.Stop => "Stop test",
					_ => testMode.ToString()
				}));

		if (mode == MotorTestMode.Stop)
		{
			await testMotor.StopTest(cancellationToken);
			AnsiConsole.MarkupLine("[yellow]Motor test stopped.[/]");
			return;
		}

		var throttle = AnsiConsole.Ask<int>("Throttle level, % (0-100)");
		var timeout = AnsiConsole.Ask<int>("Test duration, s");

		if (!await AnsiConsole.ConfirmAsync("Start test?", defaultValue: true, cancellationToken))
			return;

		AnsiConsole.MarkupLine($"[green]Starting motor test # {motor} ... ({timeout}s)[/]");

		await testMotor.StartTest(throttle, timeout, cancellationToken);

		AnsiConsole.MarkupLine("[green]Test started.[/]");
	}

	private async Task RunLiveDisplayAsync(IMotorTestClient motorTestClient, int refreshTimeMs, CancellationToken sessionToken)
	{
		var initialLayout = BuildLayout(motorTestClient);

		await AnsiConsole.Live(initialLayout)
			.AutoClear(false)
			.StartAsync(async ctx =>
			{
				while (!sessionToken.IsCancellationRequested)
				{
					ctx.UpdateTarget(BuildLayout(motorTestClient));

					try
					{
						await Task.Delay(refreshTimeMs, sessionToken).ConfigureAwait(false);
					}
					catch (OperationCanceledException)
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
				new Markup($"[blue]{motor.Pwm}[/]"),
				new Markup($"[green]{motor.IsTestRun}[/]"));
		}

		var footer = new Panel(new Markup("[blue]Press [yellow]E[/] — test, [red]Q[/] — quit[/]"))
		{
			Border = BoxBorder.None,
			Padding = new Padding(0, 1, 0, 0)
		};

		var layout = new Rows(table, footer);

		return layout;
	}

	private enum MotorTestMode
	{
		Start = 0,
		Stop
	}
}
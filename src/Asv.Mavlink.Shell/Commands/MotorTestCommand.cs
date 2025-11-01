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
	private IMotorTestClient _motorTestClient;

	[Command("motor-test")]
	public async Task RunMotorTestAsync(string connection = "tcp://127.0.0.1:5760")
	{
		ShellCommandsHelper.CreateDeviceExplorer(connection, out var deviceExplorer);

		var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer, 3000);
		if (device is null)
		{
			AnsiConsole.MarkupLine("[red]error:[/] cannot connect to the device.");
			return;
		}

		var motorTestClient = device.GetMicroservice<IMotorTestClient>();
		if (motorTestClient is null)
		{
			AnsiConsole.MarkupLine("[red]error:[/] this device is unsupported yet.");
			return;
		}

		_motorTestClient = motorTestClient;

		var appExit = false;

		// Основной цикл: запускаем Live, параллельно слушаем клавиши
		while (!appExit)
		{
			// Создаём токен для текущего Live-режима
			using var liveCts = new CancellationTokenSource();

			// Task для Live-рендера
			var liveTask = RunLiveAsync(liveCts.Token);

			// Task для обработки клавиш (работает параллельно)
			var keyTask = Task.Run(async () =>
			{
				// Пока Live не отменён и приложение не выходит, проверяем клавиши
				while (!liveCts.IsCancellationRequested && !appExit)
				{
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey(intercept: true).Key;
						if (key == ConsoleKey.Q)
						{
							// Выход: отменяем Live и ставим флаг выхода
							liveCts.Cancel();
							appExit = true;
							break;
						}
						else if (key == ConsoleKey.E)
						{
							// Переход в режим редактирования: отменяем Live и выходим из loop
							liveCts.Cancel();
							break;
						}
					}

					// Небольшая задержка, чтобы не жрать CPU
					await Task.Delay(80);
				}
			});

			// Ждём окончания Live (либо по отмене, либо по ошибке)
			await Task.WhenAny(liveTask, keyTask);

			// Если keyTask отменил live (например, нажали E), дожидаемся завершения liveTask
			try
			{
				await liveTask;
			}
			catch
			{
				/* игнорируем исключения от отмены */
			}

			// Если пользователь нажал Q — выходим
			if (appExit)
				break;

			// Теперь: переходим в режим промта (редактирование параметров)
			// (Live уже остановлен — безопасно вызывать блокирующие промты)
			Console.Clear();
			AnsiConsole.MarkupLine("[bold cyan]Режим редактирования параметров[/]\n");

			var motorsCount = _motorTestClient.TestMotors.Count;

			// Пример промтов — можно менять/расширять
			var motor = AnsiConsole.Ask<int>($"Введите номер мотора (1-{motorsCount})");
			// newCount = Math.Clamp(newCount, 1, 20);

			var throttle = AnsiConsole.Ask<int>("Мощность, % (0-100)");
			// throttle = Math.Clamp(throttle, 0, 100);

			var timeout = AnsiConsole.Ask<int>("Продолжительность, с");
			// timeout = Math.Clamp(throttle, 0, 1000);

			// Короткое сообщение о старте фоновой задачи (например, запуск теста)
			var startTask = AnsiConsole.Confirm("Запустить тест?");
			if (startTask)
			{
				var testMotor = _motorTestClient.TestMotors.First(m => m.Id == motor);

				AnsiConsole.MarkupLine($"[green]Запуск теста для мотора № {motor} ... ({timeout}с)[/]");

				// Простейшая фоновая задача, эмулирующая работу
				await Task.Run(async () => { await testMotor.StartTest(throttle, timeout, CancellationToken.None); },
					CancellationToken.None);
				AnsiConsole.MarkupLine("[green]Тест запущен.[/]");
			}
		}

		AnsiConsole.MarkupLine("[grey]Выход...[/]");
	}

	// Запускает Live-рендер и возвращает задачу, завершится когда токен отменят
	private async Task RunLiveAsync(CancellationToken token)
	{
		// Инициализация начального тултайпа (пустая)
		var initialLayout = BuildLayout();

		await AnsiConsole.Live(initialLayout)
			.AutoClear(false)
			.StartAsync(async ctx =>
			{
				while (!token.IsCancellationRequested)
				{
					// Строим новую таблицу/макет и обновляем
					ctx.UpdateTarget(BuildLayout());

					// Пробуем обновлять с ~300ms интервалом
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

	private IRenderable BuildLayout()
	{
		var table = new Table();
		table.AddColumns("[blue]Motor[/]", "[blue]Servo[/]", "[blue]PWM[/]", "[blue]Test[/]");

		foreach (var item in _motorTestClient.TestMotors)
		{
			table.AddRow(
				new Markup($"[blue]{item.Id}[/]"),
				new Markup($"[blue]{item.ServoChannel}[/]"),
				new Markup($"[blue]{item.Pwm.CurrentValue.ToString()}[/]"),
				new Markup($"[green]{item.IsTestRun}[/]"));
		}

		var footer = new Panel(new Markup("[blue]Нажмите [yellow]E[/] — испытание, [red]Q[/] — выйти[/]"))
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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using R3;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Asv.Mavlink.Shell;

public class MototTestCommand
{
	private List<DisplayRow> _items = new List<DisplayRow>();

	private static LiveDisplay _telemetryDisplay;

	private ArduCopterMotorTestClient _motorTestClient;
	private readonly Lock _obj = new Lock();

	[Command("motor-test")]
	public async Task RunMotorTestAsync(string connection = "tcp://127.0.0.1:5760")
	{
		// Task.Factory.StartNew(KeyListen);

		// await RunInputLoop();

		await using var conn = Protocol.Create(builder => { builder.RegisterMavlinkV2Protocol(); })
			.CreateRouter("ROUTER");
		await using var port = conn.AddPort(connection);

		var identity = new MavlinkClientIdentity(255, 255, 1, 1);
		var seq = new PacketSequenceCalculator();
		var core = new CoreServices(conn, seq, null, TimeProvider.System, new DefaultMeterFactory());

		var commandConfig = new CommandProtocolConfig()
		{
			CommandTimeoutMs = 1000,
			CommandAttempt = 5
		};

		var paramConfig = new ParameterClientConfig()
		{
			ReadTimeouMs = 1000,
			ReadAttemptCount = 3,
		};

		var commandClient = new CommandClient(identity, commandConfig, core);
		var paramsClient = new ParamsClient(identity, paramConfig, core);

		_motorTestClient = new ArduCopterMotorTestClient(commandClient, paramsClient, identity, core);
		await _motorTestClient.RunTelemetry();
		var motorsTestLoads = _motorTestClient.MotorsTelemetry
			.Subscribe(telemetry =>
			{
				_items = new List<DisplayRow>();
				
				foreach (var motorPwm in telemetry.MotorPwms)
				{
					lock (_obj)
					{
						_items.Add(new DisplayRow()
						{
							Motor = motorPwm.Id.HasValue ? motorPwm.Id.Value.ToString() : "Unspecified",
							Servo = motorPwm.Servo,
							Pwm = motorPwm.Pwm
						});
					}
				}
			});

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
            try { await liveTask; } catch { /* игнорируем исключения от отмены */ }

            // Если пользователь нажал Q — выходим
            if (appExit) break;

            // Теперь: переходим в режим промта (редактирование параметров)
            // (Live уже остановлен — безопасно вызывать блокирующие промты)
            Console.Clear();
            AnsiConsole.MarkupLine("[bold cyan]Режим редактирования параметров[/]\n");

            // Пример промтов — можно менять/расширять
            var motor = AnsiConsole.Ask<int>("Введите номер мотора (1-20)");
            // newCount = Math.Clamp(newCount, 1, 20);


            var throttle = AnsiConsole.Ask<int>("Мощность (50-2000)");
			// throttle = Math.Clamp(throttle, 0, 100);
			
			var timeout = AnsiConsole.Ask<int>("Продолжительность в с (50-2000)");
			// timeout = Math.Clamp(throttle, 0, 1000);

            // Короткое сообщение о старте фоновой задачи (например, запуск теста)
            var startTask = AnsiConsole.Confirm("Запустить тест (эмулируется фоновая задача)?");
            if (startTask)
            {
                AnsiConsole.MarkupLine("[green]Запуск теста... (эмуляция 3с)[/]");
                // Простейшая фоновая задача, эмулирующая работу
                // await Task.Run(async () =>
                // {
					await _motorTestClient.StartMotor(motor, throttle, timeout, CancellationToken.None);

                // }, liveCts.Token);
                AnsiConsole.MarkupLine("[green]Тест запущен.[/]");
            }

            AnsiConsole.MarkupLine("\nНажмите любую клавишу, чтобы вернуться в режим наблюдения...");
            Console.ReadKey(intercept: true);

            // Перед следующим запуском Live можно изменить скорость обновления телеметрии:
            // для простоты — пересоздадим telemetryTask с новым интервалом (если необходимо).
            // В данном примере мы не меняем реальный интервал обновляющего таска,
            // но показать как можно его адаптировать — можно по желанию.
            Console.Clear();
            // loop повторится и заново стартует Live
        }

        // Завершаем фоновый обновлятор
        // telemetryCts.Cancel();
        // try { await telemetryTask; } catch { }
        AnsiConsole.MarkupLine("[grey]Выход...[/]");

	}
	
	// Запускает Live-рендер и возвращает задачу, завершится когда токен отменят
	private async Task RunLiveAsync( CancellationToken token)
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
					try { await Task.Delay(300, token); }
					catch (TaskCanceledException) { break; }
				}
			});
	}
	
	// Строит Layout: таблица + подсказка внизу
	private IRenderable BuildLayout()
	{
		DisplayRow[] items;
		MavlinkMessage[] packets;
		lock (_obj)
		{
			items = _items.ToArray();
		}

		AnsiConsole.Clear();

		var table = new Table();
		table.AddColumns("[blue]Motor[/]", "[blue]Servo[/]", "[blue]PWM[/]");
		
		foreach (var item in items)
		{
			table.AddRow(
				new Markup($"[blue]{item.Motor}[/]"),
				new Markup($"[blue]{item.Servo}[/]"),
				new Markup($"[blue]{item.Pwm.ToString()}[/]"));
		}

		var footer = new Panel(new Markup("[blue]Нажмите [yellow]E[/] — редактировать, [red]Q[/] — выйти[/]"))
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

	private class DisplayRow
	{
		public string Motor { get; set; }
		public int Servo { get; set; }
		public double Pwm { get; set; }

	}


}
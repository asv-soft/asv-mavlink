using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.XUnit;
using R3;
using TimeProviderExtensions;

using Xunit;
namespace Asv.Mavlink.Test;

public abstract class ClientTestBase<TClient> : IDisposable
{
    private TClient? _client;

    protected ClientTestBase(ITestOutputHelper log)
    {
        Log = log;
        
        Time = new ManualTimeProvider();
        Seq = new PacketSequenceCalculator();
        Identity = new MavlinkClientIdentity(1, 2, 3, 4);
        var loggerFactory = new TestLoggerFactory(log, Time, "SERVER");
        var messageFactory = MavlinkV2Protocol.CreateMessageFactory();
        var protocol = Protocol.Create(builder =>
        {
            builder.SetLog(loggerFactory);
            builder.SetTimeProvider(Time);
            builder.RegisterMavlinkV2Protocol(messageFactory);
            
        });
        Link = protocol.CreateVirtualConnection();
        Context = new CoreServices(Link.Client,messageFactory, Seq, new TestLoggerFactory(log, Time, "CLIENT"), Time, new DefaultMeterFactory());
    }
    
    protected abstract TClient CreateClient(MavlinkClientIdentity identity, CoreServices core);
    protected MavlinkClientIdentity Identity { get; }
    protected ITestOutputHelper Log { get; }
    protected TClient Client => _client ??= CreateClient(Identity, Context);
    protected CoreServices Context { get; }
    protected PacketSequenceCalculator Seq { get; }
    protected ManualTimeProvider Time { get; }
    protected IVirtualConnection Link { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Link.Dispose();
            if (_client is IDisposable client)
            {
                client.Dispose();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

internal static class RetryTimeoutTestHelper
{
    public static async Task AssertTimeoutAsync(
        Func<CancellationToken, Task> action,
        IProtocolConnection connection,
        ManualTimeProvider time,
        int attempts,
        TimeSpan timeout
    )
    {
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(
            Xunit.TestContext.Current.CancellationToken
        );
        deadline.CancelAfter(TimeSpan.FromSeconds(10));
        using var requests = new SemaphoreSlim(0);
        using var subscription = connection.OnTxMessage.Subscribe(_ => requests.Release());
        var operation = Assert.ThrowsAsync<TimeoutException>(() => action(deadline.Token));
        try
        {
            for (var attempt = 0; attempt < attempts; attempt++)
            {
                // InternalCall registers each timeout before sending its request. Wait for
                // that request because retry continuations can run after Advance returns.
                await requests.WaitAsync(deadline.Token);
                time.Advance(timeout);
            }

            await operation.WaitAsync(deadline.Token);
        }
        finally
        {
            await deadline.CancelAsync();
        }
    }
}

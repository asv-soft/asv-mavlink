using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Moq;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

public class ParamsExtClientExLifetimeTest(ITestOutputHelper log) : ClientTestBase<ParamsExtClientEx>(log)
{
    private readonly Subject<ParamExtValuePayload> _updates = new();

    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var client = new Mock<IParamsExtClient>();
        client.SetupGet(x => x.Identity).Returns(identity);
        client.SetupGet(x => x.Core).Returns(core);
        client.SetupGet(x => x.OnParamExtValue).Returns(_updates);
        return new ParamsExtClientEx(client.Object,
            new ParamsExtClientExConfig { ChunkUpdateBufferMs = 0 }, []);
    }

    [Fact]
    public void RepeatedUpdates_KeepOneSyncSubscriptionPerItem()
    {
        var client = Client;
        var payload = CreatePayload();
        _updates.OnNext(payload);
        var item = client.Items["TEST"];
        Assert.Equal(1, SyncSubscriptionCount(item));

        for (var i = 0; i < 1000; i++)
        {
            payload.ParamValue[0] = (char)(i % 100);
            _updates.OnNext(payload);
        }

        Assert.Same(item, Assert.Single(client.Items).Value);
        Assert.Equal(1, SyncSubscriptionCount(item));
        item.Value.Value = (byte)123;
        Assert.False(client.IsSynced.Value);
        _updates.OnNext(payload);
        Assert.True(client.IsSynced.Value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Dispose_ReleasesOwnedItemsAndSubscriptions(bool asyncDispose)
    {
        var client = Client;
        _updates.OnNext(CreatePayload());
        var item = client.Items["TEST"];

        if (asyncDispose) await client.DisposeAsync();
        else client.Dispose();

        Assert.Empty(client.Items);
        Assert.Equal(0, SyncSubscriptionCount(item));
        Assert.Throws<ObjectDisposedException>(() => item.Value.Value = (byte)123);
        _updates.OnNext(CreatePayload());
        Assert.Empty(client.Items);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Dispose_ReleasesClientWhileConnectionRemainsAlive(bool asyncDispose)
    {
        var reference = CreateDisposedClient(asyncDispose);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(reference.IsAlive);
        GC.KeepAlive(Context);
        GC.KeepAlive(_updates);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference CreateDisposedClient(bool asyncDispose)
    {
        var client = CreateClient(Identity, Context);
        _updates.OnNext(CreatePayload());
        var reference = new WeakReference(client);
        if (asyncDispose) client.DisposeAsync().AsTask().GetAwaiter().GetResult();
        else client.Dispose();
        return reference;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Dispose_CancelsPendingReadAll(bool asyncDispose)
    {
        var client = Client;
        var read = client.ReadAll(cancel: Xunit.TestContext.Current.CancellationToken);
        Assert.False(read.IsCompleted);

        if (asyncDispose) await client.DisposeAsync();
        else client.Dispose();

        Assert.True(read.IsCompleted);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => read);
    }

    [Fact]
    public async Task ReadAll_DisposesPreviousItemsBeforeReloading()
    {
        var client = Client;
        var payload = CreatePayload();
        _updates.OnNext(payload);
        var oldItem = client.Items["TEST"];
        var read = client.ReadAll(cancel: Xunit.TestContext.Current.CancellationToken);
        Assert.Empty(client.Items);
        Assert.Equal(0, SyncSubscriptionCount(oldItem));
        Assert.Throws<ObjectDisposedException>(() => oldItem.Value.Value = (byte)123);

        _updates.OnNext(payload);
        Assert.True(await read);
        var newItem = client.Items["TEST"];
        Assert.NotSame(oldItem, newItem);
        Assert.Equal(1, SyncSubscriptionCount(newItem));
    }

    private static ParamExtValuePayload CreatePayload()
    {
        var payload = new ParamExtValuePayload
        {
            ParamCount = 1,
            ParamIndex = 0,
            ParamType = MavParamExtType.MavParamExtTypeUint8
        };
        MavlinkTypesHelper.SetString(payload.ParamId, "TEST");
        payload.ParamValue[0] = (char)1;
        return payload;
    }

    private static int SyncSubscriptionCount(ParamExtItem item)
    {
        // R3 exposes no observer count. Inspect its linked list to detect subscription
        // growth deterministically without timing or process-wide heap assertions.
        var rootField = typeof(ReactiveProperty<bool>).GetField("root", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var node = rootField.GetValue(item.IsSynced);
        var count = 0;
        while (node != null)
        {
            count++;
            node = node.GetType().GetProperty("Next", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(node);
        }
        return count;
    }
}

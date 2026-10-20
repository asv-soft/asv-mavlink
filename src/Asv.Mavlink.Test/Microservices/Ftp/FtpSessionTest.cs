using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Asv.Mavlink.Test;

public class FtpSessionTest
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task Close_ReusedSessionDisposesEachStreamOnce(bool firstAsync, bool secondAsync)
    {
        var session = new FtpSession(1);
        var first = new TrackingStream();
        var second = new TrackingStream();
        session.Open(FtpSession.SessionMode.OpenRead);
        session.Stream = first;
        await Close(session, firstAsync);
        Assert.Equal(FtpSession.SessionMode.Free, session.Mode);
        Assert.Null(session.Stream);
        session.Open(FtpSession.SessionMode.OpenWrite);
        session.Stream = second;
        await Close(session, secondAsync);
        await session.CloseAsync();
        session.Close();
        session.Dispose();
        await session.DisposeAsync();

        Assert.Null(session.Stream);
        Assert.Equal(1, first.DisposeCount);
        Assert.Equal(1, second.DisposeCount);
        Assert.Equal(firstAsync ? 1 : 0, first.AsyncDisposeCount);
        Assert.Equal(secondAsync ? 1 : 0, second.AsyncDisposeCount);
    }

    private static async Task Close(FtpSession session, bool asynchronous)
    {
        if (asynchronous) await session.CloseAsync();
        else session.Close();
    }

    private sealed class TrackingStream : MemoryStream
    {
        public int DisposeCount { get; private set; }
        public int AsyncDisposeCount { get; private set; }
        protected override void Dispose(bool disposing)
        {
            if (disposing) DisposeCount++;
            base.Dispose(disposing);
        }
        public override ValueTask DisposeAsync()
        {
            AsyncDisposeCount++;
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}

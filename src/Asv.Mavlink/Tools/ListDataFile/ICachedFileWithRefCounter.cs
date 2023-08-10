#nullable enable
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using Asv.IO;

namespace Asv.Mavlink;

public interface ICachedFileWithRefCounter<out TMetadata>:IDisposable 
    where TMetadata : ISpanSerializable
{
    IListDataFile<TMetadata> File { get; }
}

public class CachedFileWithRefCounter<TMetadata,TKey> : ICachedFileWithRefCounter<TMetadata> 
    where TMetadata : ISizedSpanSerializable, new()
{
    private int _refCount;
    private readonly ListDataFile<TMetadata> _file;

    public CachedFileWithRefCounter(ListDataFile<TMetadata> file, TKey id )
    {
        Id = id;
        _file = file;
    }
    
    public void AddRef()
    {
        Interlocked.Add(ref _refCount, 1);
    }
    
    public int RefCount => _refCount;

    public void Dispose()
    {
        var refCount = Interlocked.Add(ref _refCount, -1);
        Debug.Assert(refCount >= 0);
        if (refCount == 0)
        {
            DeadTimeBegin = DateTime.Now;
        }
    }

    public IListDataFile<TMetadata> File => _file;
    public TKey Id { get; set; }
    public DateTime DeadTimeBegin { get; private set; }

    public void ImmediateDispose()
    {
        _file.Dispose();
    }
}
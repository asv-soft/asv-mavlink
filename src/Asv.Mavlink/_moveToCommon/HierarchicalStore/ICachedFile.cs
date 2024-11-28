using System;
using System.Diagnostics;
using System.Threading;

namespace Asv.Mavlink;



public class CachedFile<TKey, TFile>: ICachedFile<TKey, TFile> 
    where TFile : IDisposable
{
    private readonly Action<CachedFile<TKey, TFile>> _disposedCallback;
    private readonly TimeProvider _timeProvider;
    private int _refCount;

    public CachedFile(TKey id, string name, TKey parentId, TFile file, Action<CachedFile<TKey, TFile>> disposedCallback, TimeProvider timeProvider)
    {
        _disposedCallback = disposedCallback;
        _timeProvider = timeProvider;
        Id = id;
        Name = name;
        ParentId = parentId;
        File = file;
    }

    public void Dispose()
    {
        var refCount = Interlocked.Add(ref _refCount, -1);
        Debug.Assert(refCount >= 0);
        if (refCount == 0)
        {
            DeadTimeBegin = _timeProvider.GetTimestamp();
            _disposedCallback(this);
        }
    }

    public TKey Id { get; }
    public string Name { get; }
    public TKey ParentId { get; }
    public TFile File { get; }
    public long DeadTimeBegin { get; private set; }
    public int RefCount => _refCount;
    public void AddRef()
    {
        Interlocked.Add(ref _refCount, 1);
    }
    
    public void ImmediateDispose()
    {
        File.Dispose();
    }
}
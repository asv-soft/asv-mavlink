using System;
using System.Diagnostics;
using System.Threading;

namespace Asv.Mavlink;



public class CachedFile<TKey, TFile>: ICachedFile<TKey, TFile> 
    where TFile : IDisposable
{
    private readonly Action<CachedFile<TKey, TFile>> _disposedCallback;
    private int _refCount;

    public CachedFile(TKey id, string name, TFile file, Action<CachedFile<TKey, TFile>> disposedCallback)
    {
        _disposedCallback = disposedCallback;
        Id = id;
        Name = name;
        File = file;
    }

    public void Dispose()
    {
        var refCount = Interlocked.Add(ref _refCount, -1);
        Debug.Assert(refCount >= 0);
        if (refCount == 0)
        {
            DeadTimeBegin = DateTime.Now;
            _disposedCallback(this);
        }
    }

    public TKey Id { get; set; }
    public string Name { get; }
    public TFile File { get; }
    public DateTime DeadTimeBegin { get; private set; }
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
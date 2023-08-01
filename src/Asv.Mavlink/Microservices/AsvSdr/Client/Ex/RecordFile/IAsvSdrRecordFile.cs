using System;
using System.Collections.Generic;

namespace Asv.Mavlink;

public interface IAsvSdrRecordFile:IDisposable
{
    void EditMetadata(Action<AsvSdrRecordFileMetadata> editCallback);
    uint Count { get; }
    long Size { get; }
    bool Exist(uint index);
    void Write(uint index, IPayload payload);
    bool Read(uint index, ref IPayload payload);
    
}

public static class AsvSdrRecordFileHelper
{
    public static IEnumerable<Chunk> GetNotExistChunks(this IAsvSdrRecordFile src, int maxPageSize)
    {
        var count = src.Count;
        var startedChunk = false;
        var skip = 0U;
        var take = 0U;
        for (var i = 0U; i < count; i++)
        {
            //simplify logic
            if (take >= maxPageSize)
            {
                yield return new Chunk{ Skip = skip, Take = take}; 
                startedChunk = false;
            }
            
            if (startedChunk == false)
            {
                if (src.Exist(i) == false)
                {
                    skip = i;
                    take = 1;
                    startedChunk = true;
                }
            }
            else
            {
                if (src.Exist(i))
                {
                    yield return new Chunk{ Skip = skip, Take = take}; 
                }
                else
                {
                    ++take;
                }
            }
        }
    }
}

public struct Chunk
{
    public uint Skip { get; init; }
    public uint Take { get; init; }
}
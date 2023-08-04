using System;
using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink;




public interface IListDataFile<out TMetadata>:IDisposable
    where TMetadata:ISpanSerializable
{
    uint Count { get; }
    long ByteSize { get; }
    void EditMetadata(Action<TMetadata> editCallback);
    TMetadata ReadMetadata();
    bool Exist(uint index);
    void Write(uint index, ISpanSerializable payload);
    bool Read(uint index, ISpanSerializable payload);
}

public static class ListDataFileHelper
{
    public struct Chunk
    {
        public uint Skip { get; init; }
        public uint Take { get; init; }
    }
    
    public static IEnumerable<Chunk> GetEmptyChunks<TMetadata>(this IListDataFile<TMetadata> src, int maxPageSize) 
        where TMetadata : ISpanSerializable
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


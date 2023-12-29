using System;
using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink;

/// <summary>
/// An abstraction over Stream that allows data to be stored and quickly accessed as a list by index.
/// The file starts with a fixed-format header signed with a checksum (it cannot be changed).
/// Next comes metadata of arbitrary format but limited in size by the maximum length, also signed by a checksum.
/// Then the data starts in the form of a list. Each element of the list has a fixed length and is signed with a checksum.
/// </summary>
/// <typeparam name="TMetadata"></typeparam>
public interface IListDataFile<out TMetadata>:IDisposable
    where TMetadata:ISpanSerializable
{
    IListDataFileFormat Header { get; }
    object Tag { get; set; }

    /// <summary>
    /// Count of items in list
    /// </summary>
    uint Count { get; }

    /// <summary>
    /// Byte size of all file
    /// </summary>
    long ByteSize { get; }

    /// <summary>
    /// Thread safe method to edit metadata
    /// </summary>
    /// <param name="editCallback"></param>
    void EditMetadata(Action<TMetadata> editCallback);

    /// <summary>
    /// Thread safe method to read metadata. Return copy of metadata 
    /// </summary>
    /// <returns></returns>
    TMetadata ReadMetadata();

    /// <summary>
    /// Check if item exist in list (include the checksum)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool Exist(uint index);

    /// <summary>
    /// Write item to list and sign it with checksum
    /// </summary>
    /// <param name="index"></param>
    /// <param name="payload"></param>
    void Write(uint index, ISpanSerializable payload);

    /// <summary>
    /// Try to read item from list and check it with checksum
    /// </summary>
    /// <param name="index"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    bool Read(uint index, ISpanSerializable payload);
}

public static class ListDataFileHelper
{
    public static uint GetItemsCount<TMetadata>(this IListDataFile<TMetadata> self, uint skip,uint take)
        where TMetadata : ISizedSpanSerializable, new()
    {
        var temp = (int)self.Count - skip;
        return (uint)(temp < 0 ? 0 : Math.Min(temp,(int) take));
    }
    
    public struct Chunk
    {
        public uint Skip { get; set; }
        public uint Take { get; set; }
    }
    
    public static IEnumerable<Chunk> GetEmptyChunks<TMetadata,TKey>(this IListDataFile<TMetadata> src, int maxPageSize) 
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


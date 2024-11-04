using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;

namespace Asv.Mavlink;

public class DefaultMeterFactory : IMeterFactory
{
    private readonly Dictionary<string, List<FactoryMeter>> _cachedMeters = new();
    private bool _disposed;
    public void Dispose()
    {
        lock (_cachedMeters)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            foreach (var meter in _cachedMeters.Values.SelectMany(meterList => meterList))
            {
                meter.Release();
            }

            _cachedMeters.Clear();
        }
    }

    public Meter Create(MeterOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (options.Scope is not null && !object.ReferenceEquals(options.Scope, this))
        {
            throw new InvalidOperationException("Meter scope");
        }

        Debug.Assert(options.Name is not null);

        lock (_cachedMeters)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DefaultMeterFactory));
            }

            if (_cachedMeters.TryGetValue(options.Name, out var meterList))
            {
                foreach (var meter in meterList)
                {
                    if (meter.Version == options.Version && CompareTags(meter.Tags as IList<KeyValuePair<string, object?>>, options.Tags))
                    {
                        return meter;
                    }
                }
            }
            else
            {
                meterList = new List<FactoryMeter>();
                _cachedMeters.Add(options.Name, meterList);
            }

            var scope = options.Scope;
            options.Scope = this;
            var m = new FactoryMeter(options);
            options.Scope = scope;

            meterList.Add(m);
            return m;
        }
    }

    static bool CompareTags(IList<KeyValuePair<string, object?>>? sortedTags,
        IEnumerable<KeyValuePair<string, object?>>? tags2)
    {
        if (sortedTags == tags2)
        {
            return true;
        }

        if (sortedTags is null || tags2 is null)
        {
            return false;
        }

        var count = sortedTags.Count;
        var size = count / (sizeof(ulong) * 8) + 1;
        var bitMapper = new BitMapper(size <= 100 ? stackalloc ulong[size] : new ulong[size]);

        if (tags2 is ICollection<KeyValuePair<string, object?>> tagsCol)
        {
            if (tagsCol.Count != count)
            {
                return false;
            }

            if (tagsCol is IList<KeyValuePair<string, object?>> secondList)
            {
                for (var i = 0; i < count; i++)
                {
                    var pair = secondList[i];

                    for (var j = 0; j < count; j++)
                    {
                        if (bitMapper.IsSet(j))
                        {
                            continue;
                        }

                        KeyValuePair<string, object?> pair1 = sortedTags[j];

                        var compareResult = string.CompareOrdinal(pair.Key, pair1.Key);
                        if (compareResult == 0 && object.Equals(pair.Value, pair1.Value))
                        {
                            bitMapper.SetBit(j);
                            break;
                        }

                        if (compareResult < 0 || j == count - 1)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        var listCount = 0;
        using (var enumerator = tags2.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                listCount++;
                if (listCount > sortedTags.Count)
                {
                    return false;
                }

                var pair = enumerator.Current;
                for (var j = 0; j < count; j++)
                {
                    if (bitMapper.IsSet(j))
                    {
                        continue;
                    }

                    var pair1 = sortedTags[j];

                    var compareResult = string.CompareOrdinal(pair.Key, pair1.Key);
                    if (compareResult == 0 && object.Equals(pair.Value, pair1.Value))
                    {
                        bitMapper.SetBit(j);
                        break;
                    }

                    if (compareResult < 0 || j == count - 1)
                    {
                        return false;
                    }
                }
            }

            return listCount == sortedTags.Count;
        }
    }

    private class FactoryMeter(MeterOptions options) : Meter(options)
    {
        public void Release() => base.Dispose(true); // call the protected Dispose(bool)

        protected override void Dispose(bool disposing)
        {
            // no-op, disallow users from disposing of the meters created from the factory.
        }
    }

    private readonly ref struct BitMapper
    {
        private readonly int _maxIndex;
        private readonly Span<ulong> _bitMap;

        public BitMapper(Span<ulong> bitMap)
        {
            _bitMap = bitMap;
            _bitMap.Clear();
            _maxIndex = bitMap.Length * sizeof(ulong) * 8;
        }

        public int MaxIndex => _maxIndex;

        private static void GetIndexAndMask(int index, out int bitIndex, out ulong mask)
        {
            bitIndex = index >> 6; // divide by 64 == (sizeof(ulong) * 8) bits
            var bit = index & (sizeof(ulong) * 8 - 1);
            mask = 1UL << bit;
        }

        public bool SetBit(int index)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(index < _maxIndex);

            GetIndexAndMask(index, out var bitIndex, out var mask);
            var value = _bitMap[bitIndex];
            _bitMap[bitIndex] = value | mask;
            return true;
        }

        public bool IsSet(int index)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(index < _maxIndex);

            GetIndexAndMask(index, out var bitIndex, out var mask);
            var value = _bitMap[bitIndex];
            return ((value & mask) != 0);
        }
    }
}
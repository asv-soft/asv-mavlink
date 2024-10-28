using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Asv.Mavlink.Shell;

[JsonConverter(typeof(StringEnumConverter))]
public enum MetricType
{
    Int,
    Float,
    FloatArray,
    SByteArray,
}

public class Metric
{
    public required string Name { get; init; }
    public required MetricType Type { get; init; }
    public required string Max { get; init; }
    public required string Min { get; init; }

    public int RandomIntValue()
    {
        var minInt = Convert.ToInt32(Min, CultureInfo.InvariantCulture);
        var maxInt = Convert.ToInt32(Max, CultureInfo.InvariantCulture);
        
        return Random.Shared.Next(minInt, maxInt);
    }

    public float RandomFloatValue()
    {
        var minFloat = Convert.ToSingle(Min, CultureInfo.InvariantCulture);
        var maxFloat = Convert.ToSingle(Max, CultureInfo.InvariantCulture);
        
        return Random.Shared.NextSingle() * (maxFloat - minFloat) + minFloat;
    }

    public float[] RandomFloatArrayValue()
    {
        var minInt = Convert.ToInt32(Min, CultureInfo.InvariantCulture);
        var maxInt = Convert.ToInt32(Max, CultureInfo.InvariantCulture);
        
        var arraySize = Random.Shared.Next(minInt, maxInt);
        
        var array = new float[arraySize];
        for (var i = 0; i < arraySize; i++)
        {
            array[i] = RandomFloatValue();
        }

        return array;
    }

    public sbyte[] RandomSByteArrayValue()
    {
        var minInt = Convert.ToInt32(Min);
        var maxInt = Convert.ToInt32(Max);
        
        var arraySize = Random.Shared.Next(minInt, maxInt);
        
        var array = new sbyte[arraySize];
        for (var i = 0; i < arraySize; i++)
        {
            array[i] = (sbyte) Random.Shared.Next(-128, 127);
        }

        return array;
    }
    
    public class MetricEqualityComparer : IEqualityComparer<Metric>
    {
        public static readonly Lazy<MetricEqualityComparer> Instance = new (() => new MetricEqualityComparer());

        private MetricEqualityComparer() { }
        
        public bool Equals(Metric? x, Metric? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }
            
            return x.Name == y.Name;
        }

        public int GetHashCode(Metric obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            return obj.Name.GetHashCode();
        }
    }
}
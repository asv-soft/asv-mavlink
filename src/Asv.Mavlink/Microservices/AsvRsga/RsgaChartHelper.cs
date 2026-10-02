using System;
using Asv.IO;
using Asv.Mavlink.AsvRsga;

namespace Asv.Mavlink;

public static class RsgaChartHelper
{
    public const int ChartDataHeaderByteSize = 17;

    public static int GetMaxChartSamples(AsvRsgaRttChartDataFormat format)
    {
        return format switch
        {
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit =>
                AsvRsgaRttChartPayload.DataMaxItemsCount - ChartDataHeaderByteSize,
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat16bit =>
                (AsvRsgaRttChartPayload.DataMaxItemsCount - ChartDataHeaderByteSize) / sizeof(ushort),
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat =>
                (AsvRsgaRttChartPayload.DataMaxItemsCount - ChartDataHeaderByteSize) / sizeof(float),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
        };
    }

    public static AsvRsgaRttChartDataFormat GetDataFormat(RsgaChartEncoding encoding)
    {
        return encoding switch
        {
            RsgaChartEncoding.Auto => AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit,
            RsgaChartEncoding.RangeFloat8Bit => AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit,
            RsgaChartEncoding.RangeFloat16Bit => AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat16bit,
            RsgaChartEncoding.Float => AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat,
            _ => throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null),
        };
    }

    public static void WriteChartData(
        AsvRsgaRttChartPayload payload,
        ReadOnlySpan<double> data,
        RsgaChartSendOptions? options = null
    )
    {
        if (data.IsEmpty)
        {
            throw new ArgumentException("At least one chart sample required.", nameof(data));
        }

        options ??= new RsgaChartSendOptions();
        var format = GetDataFormat(options.Encoding);
        var capacity = GetMaxChartSamples(format);
        var sampleCount = Math.Min(data.Length, capacity);
        if (options.MaxSamples is { } maxSamples)
        {
            if (maxSamples <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.MaxSamples));
            }
            sampleCount = Math.Min(sampleCount, maxSamples);
        }
        if (sampleCount > byte.MaxValue)
        {
            throw new InvalidOperationException("RSGA chart sample count must fit into one byte.");
        }

        var samples = new double[sampleCount];
        Resample(data, samples, options.Resampling);

        var xRange = options.XRange ?? new RsgaChartRange(0, Math.Max(0, data.Length - 1));
        var yRange = NormalizeRange(options.YRange ?? CalculateFiniteRange(samples));

        Array.Clear(payload.Data);
        var byteSpan = payload.Data.AsSpan();
        WriteHeader(ref byteSpan, xRange, yRange, (byte)sampleCount);
        WriteSamples(ref byteSpan, samples, yRange, format);

        payload.Format = format;
        payload.ChartType = options.ChartType;
        payload.DataIndex = options.DataIndex;
        payload.TimeUnixUsec = MavlinkTypesHelper.ToUnixTimeUs(options.Timestamp ?? DateTime.UtcNow);
        payload.Flags = options.Flags;
    }

    public static RsgaChartFrame ReadChartData(in AsvRsgaRttChartPayload payload)
    {
        var readSpan = new ReadOnlySpan<byte>(payload.Data);
        var xMin = BinSerialize.ReadFloat(ref readSpan);
        var xMax = BinSerialize.ReadFloat(ref readSpan);
        var yMin = BinSerialize.ReadFloat(ref readSpan);
        var yMax = BinSerialize.ReadFloat(ref readSpan);
        var size = BinSerialize.ReadByte(ref readSpan);
        var sampleSize = GetSampleByteSize(payload.Format);
        var requiredSize = ChartDataHeaderByteSize + (size * sampleSize);
        if (size == 0 || requiredSize > AsvRsgaRttChartPayload.DataMaxItemsCount)
        {
            throw new MavlinkException($"Invalid RSGA chart payload size {size} for format {payload.Format:G}");
        }

        var values = new double[size];
        var yRange = new RsgaChartRange(yMin, yMax);
        for (var i = 0; i < values.Length; i++)
        {
            values[i] = ReadSample(ref readSpan, yRange, payload.Format);
        }

        return new RsgaChartFrame(
            MavlinkTypesHelper.FromUnixTimeUs(payload.TimeUnixUsec),
            payload.DataIndex,
            payload.Flags,
            payload.ChartType,
            payload.Format,
            new RsgaChartRange(xMin, xMax),
            yRange,
            values
        );
    }

    public static int ReadChartDataFloat8Bit(
        in AsvRsgaRttChartPayload payload,
        Span<double> output,
        out float xMin,
        out float xMax
    )
    {
        if (payload.Format != AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit)
        {
            throw new ArgumentException("Payload format mismatch", nameof(payload));
        }

        var frame = ReadChartData(payload);
        xMin = (float)frame.XRange.Min;
        xMax = (float)frame.XRange.Max;
        var size = frame.Values.Length;
        var copySize = Math.Min(size, output.Length);

        for (var i = 0; i < copySize; i++)
        {
            output[i] = frame.Values[i];
        }

        // Fill the unused output tail when the destination buffer is longer than the payload data.
        for (var i = copySize; i < output.Length; i++)
        {
            output[i] = 0;
        }

        return size;
    }

    public static void WriteChartDataFloat8Bit(
        AsvRsgaRttChartPayload payload,
        ReadOnlySpan<double> data,
        double xMin,
        double xMax,
        AsvRsgaRttChartType chartType,
        uint dataIndex,
        DateTime timestamp,
        AsvRsgaDataFlags flags
    )
    {
        WriteChartData(
            payload,
            data,
            new RsgaChartSendOptions
            {
                ChartType = chartType,
                Encoding = RsgaChartEncoding.RangeFloat8Bit,
                XRange = new RsgaChartRange(xMin, xMax),
                DataIndex = dataIndex,
                Timestamp = timestamp,
                Flags = flags,
            }
        );
    }

    public static void LinearInterpolate(ReadOnlySpan<double> input, Span<double> output)
    {
        if (input.Length < 2)
        {
            throw new ArgumentException(
                "At least 2 points required for interpolation.",
                nameof(input)
            );
        }

        if (output.Length == input.Length)
        {
            input.CopyTo(output);
            return;
        }
        if (output.Length == 1)
        {
            output[0] = input[0];
            return;
        }

        var nIn = input.Length;
        var nOut = output.Length;

        output[0] = input[0];
        output[nOut - 1] = input[nIn - 1];

        for (var i = 1; i < nOut - 1; i++)
        {
            var t = (double)i / (nOut - 1);
            var pos = t * (nIn - 1);
            var idx = (int)Math.Floor(pos);
            var frac = pos - idx;

            var v0 = input[idx];
            var v1 = input[idx + 1];
            output[i] = v0 + (frac * (v1 - v0));
        }
    }

    private static int GetSampleByteSize(AsvRsgaRttChartDataFormat format)
    {
        return format switch
        {
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit => sizeof(byte),
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat16bit => sizeof(ushort),
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat => sizeof(float),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
        };
    }

    private static void WriteHeader(ref Span<byte> span, RsgaChartRange xRange, RsgaChartRange yRange, byte size)
    {
        BinSerialize.WriteFloat(ref span, (float)xRange.Min);
        BinSerialize.WriteFloat(ref span, (float)xRange.Max);
        BinSerialize.WriteFloat(ref span, (float)yRange.Min);
        BinSerialize.WriteFloat(ref span, (float)yRange.Max);
        BinSerialize.WriteByte(ref span, size);
    }

    private static void WriteSamples(
        ref Span<byte> span,
        ReadOnlySpan<double> samples,
        RsgaChartRange yRange,
        AsvRsgaRttChartDataFormat format
    )
    {
        foreach (var sample in samples)
        {
            var value = double.IsFinite(sample) ? sample : yRange.Min;
            switch (format)
            {
                case AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit:
                    BinSerialize.Write8BitRange(ref span, (float)yRange.Min, (float)yRange.Max, (float)value);
                    break;
                case AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat16bit:
                    BinSerialize.Write16BitRange(ref span, (float)yRange.Min, (float)yRange.Max, (float)value);
                    break;
                case AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat:
                    BinSerialize.WriteFloat(ref span, (float)value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }

    private static double ReadSample(
        ref ReadOnlySpan<byte> span,
        RsgaChartRange yRange,
        AsvRsgaRttChartDataFormat format
    )
    {
        return format switch
        {
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat8bit =>
                BinSerialize.Read8BitRange(ref span, (float)yRange.Min, (float)yRange.Max),
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatRangeFloat16bit =>
                BinSerialize.Read16BitRange(ref span, (float)yRange.Min, (float)yRange.Max),
            AsvRsgaRttChartDataFormat.AsvRsgaRttChartDataFormatFloat => BinSerialize.ReadFloat(ref span),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
        };
    }

    private static void Resample(
        ReadOnlySpan<double> input,
        Span<double> output,
        RsgaChartResampling resampling
    )
    {
        if (output.Length == input.Length)
        {
            input.CopyTo(output);
            return;
        }
        if (output.Length > input.Length)
        {
            input.CopyTo(output);
            output[input.Length..].Clear();
            return;
        }

        switch (resampling)
        {
            case RsgaChartResampling.Auto:
            case RsgaChartResampling.MinMaxEnvelope:
                MinMaxEnvelope(input, output);
                break;
            case RsgaChartResampling.Linear:
                LinearInterpolate(input, output);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resampling), resampling, null);
        }
    }

    private static void MinMaxEnvelope(ReadOnlySpan<double> input, Span<double> output)
    {
        if (output.Length == 1)
        {
            output[0] = input[0];
            return;
        }

        var bucketCount = Math.Max(1, output.Length / 2);
        var outputIndex = 0;
        for (var bucket = 0; bucket < bucketCount && outputIndex < output.Length; bucket++)
        {
            var start = bucket * input.Length / bucketCount;
            var end = Math.Max(start + 1, (bucket + 1) * input.Length / bucketCount);
            if (end > input.Length)
            {
                end = input.Length;
            }

            var min = double.PositiveInfinity;
            var max = double.NegativeInfinity;
            var minIndex = start;
            var maxIndex = start;
            for (var i = start; i < end; i++)
            {
                var value = input[i];
                if (value < min)
                {
                    min = value;
                    minIndex = i;
                }
                if (value > max)
                {
                    max = value;
                    maxIndex = i;
                }
            }

            if (minIndex <= maxIndex)
            {
                output[outputIndex++] = min;
                if (outputIndex < output.Length)
                {
                    output[outputIndex++] = max;
                }
            }
            else
            {
                output[outputIndex++] = max;
                if (outputIndex < output.Length)
                {
                    output[outputIndex++] = min;
                }
            }
        }

        if (outputIndex < output.Length)
        {
            output[outputIndex] = input[^1];
        }
    }

    private static RsgaChartRange CalculateFiniteRange(ReadOnlySpan<double> data)
    {
        var min = double.PositiveInfinity;
        var max = double.NegativeInfinity;
        foreach (var value in data)
        {
            if (!double.IsFinite(value))
            {
                continue;
            }
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        if (double.IsInfinity(min) || double.IsInfinity(max))
        {
            throw new ArgumentException("Chart data must contain at least one finite sample.", nameof(data));
        }

        return new RsgaChartRange(min, max);
    }

    private static RsgaChartRange NormalizeRange(RsgaChartRange range)
    {
        if (!double.IsFinite(range.Min) || !double.IsFinite(range.Max))
        {
            throw new ArgumentException("Chart range must be finite.", nameof(range));
        }
        if (range.Min > range.Max)
        {
            throw new ArgumentException("Chart range min must be less than or equal to max.", nameof(range));
        }
        if (range.Min < range.Max)
        {
            return range;
        }

        var padding = Math.Abs(range.Min) > 0 ? Math.Abs(range.Min) * 0.001 : 1.0;
        return new RsgaChartRange(range.Min - padding, range.Max + padding);
    }
}

using Asv.Mavlink.AsvChart;


namespace Asv.Mavlink;

public class AsvChartOptions(ushort chartId, AsvChartDataTrigger trigger, float rateMs)
{
    internal AsvChartOptions(AsvChartDataRequestPacket packet):this(packet.Payload.ChatId,packet.Payload.DataTrigger,packet.Payload.DataRate)
    {
    }

    internal AsvChartOptions(AsvChartDataResponsePacket packet):this(packet.Payload.ChatId,packet.Payload.DataTrigger,packet.Payload.DataRate)
    {
    
    }

    public float Rate { get;  } = rateMs;
    public AsvChartDataTrigger Trigger { get; } = trigger;
    public ushort ChartId { get; } = chartId;
}
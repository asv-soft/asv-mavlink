using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IStatusTextClient
{
    IRxEditableValue<string> Name { get; }
    IRxValue<StatusMessage> OnMessage { get; }
}

public class StatusMessage
{
    public string Sender { get; set; }
    public string Text { get; set; }
    public MavSeverity Type { get; set; }
}
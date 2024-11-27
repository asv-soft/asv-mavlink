using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.AsvGbs;
 using R3;

 namespace Asv.Mavlink
 {
     /// <summary>
     /// This interface represents an AsvGbs client.
     /// </summary>
     public interface IAsvGbsClient:IMavlinkMicroserviceClient
     {
         /// <summary>
         /// Gets the raw status of the AsvGbsOutStatusPayload.
         /// </summary>
         ReadOnlyReactiveProperty<AsvGbsOutStatusPayload?> RawStatus { get; }
     }
 }
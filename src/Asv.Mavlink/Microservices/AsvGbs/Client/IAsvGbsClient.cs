using Asv.Common;
 using Asv.Mavlink.V2.AsvGbs;

 namespace Asv.Mavlink
 {
     /// <summary>
     /// This interface represents an AsvGbs client.
     /// </summary>
     public interface IAsvGbsClient
     {
         /// <summary>
         /// Gets the raw status of the AsvGbsOutStatusPayload.
         /// </summary>
         IRxValue<AsvGbsOutStatusPayload> RawStatus { get; }
     }
 }
namespace Asv.Mavlink
{
    public class MavlinkServerIdentity
    {
        public byte ComponentId { get; set; } = 13;
        public byte SystemId { get; set; } = 13;

        public override string ToString()
        {
            return $"[Server:{SystemId}.{ComponentId}]<==";
        }
    }
}

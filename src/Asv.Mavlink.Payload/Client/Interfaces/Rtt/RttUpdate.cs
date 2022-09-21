namespace Asv.Mavlink.Payload
{
    public class RttUpdate
    {
        public RttUpdate(Pv2RttStreamMetadata metadata, Pv2RttRecordDesc record, Pv2RttFieldDesc field, object value)
        {
            Metadata = metadata;
            Record = record;
            Field = field;
            Value = value;
        }

        public Pv2RttStreamMetadata Metadata { get; }
        public Pv2RttRecordDesc Record { get; }
        public Pv2RttFieldDesc Field { get; }
        public object Value { get; }
    }
}

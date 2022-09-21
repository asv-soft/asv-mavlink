namespace Asv.Mavlink.Payload
{
    public class Pv2RttStreamData
    {
        private readonly string _toString;

        public Pv2RttStreamData(string toString)
        {
            _toString = toString;
        }

        public Pv2RttStreamData(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }

        public override string ToString()
        {
            return _toString;
        }
    }
}

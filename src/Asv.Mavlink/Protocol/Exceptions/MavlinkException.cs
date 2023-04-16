using System;
using System.Runtime.Serialization;

namespace Asv.Mavlink
{
    [Serializable]
    public class MavlinkException : Exception
    {
        public MavlinkException()
        {
        }

        public MavlinkException(string message) : base(message)
        {
        }

        public MavlinkException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MavlinkException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

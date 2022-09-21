using System;

namespace Asv.Mavlink
{
    public class DeserializePackageException:MavlinkException
    {
        public int MessageId { get; }

        public DeserializePackageException(int messageId,string message, Exception innerException):base(message,innerException)
        {
            MessageId = messageId;
        }

        public DeserializePackageException(int messageId, string message):base(message)
        {
            MessageId = messageId;
        }
    }
}
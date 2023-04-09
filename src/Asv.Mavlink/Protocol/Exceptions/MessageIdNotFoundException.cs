namespace Asv.Mavlink
{
    public class MessageIdNotFoundException : DeserializePackageException
    {
        public MessageIdNotFoundException(int messageId) : base(messageId, string.Format(RS.MessageIdNotFoundException_MessageIdNotFoundException, messageId))
        {
        }
    }
}
using System;

namespace Asv.Mavlink.Payload
{
    [Serializable]
    public class InternalPv2Exception : Exception
    {
        public InternalPv2Exception(string remoteErrorMessage, string method, string clientIdentity)
            : base($"{clientIdentity} remote error to call {method}:{remoteErrorMessage}")
        {
            RemoteErrorMessage = remoteErrorMessage;
            Method = method;
            ClientIdentity = clientIdentity;
        }

        public string RemoteErrorMessage { get; }
        public string Method { get; }
        public string ClientIdentity { get; }
    }
}

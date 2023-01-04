using System;
using System.Runtime.Serialization;

namespace APS.DotNetSDK.Exceptions
{
    public class SignatureException : Exception
    {
        public SignatureException()
        {
        }

        public SignatureException(string message) : base(message)
        {

        }

        public SignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SignatureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SignatureException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}

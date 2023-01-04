using System;
using System.Runtime.Serialization;

namespace APS.DotNetSDK.Exceptions
{
    public class ApiCallException : Exception
    {
        public ApiCallException()
        {
        }

        public ApiCallException(string message) : base(message)
        {

        }

        public ApiCallException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ApiCallException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace APS.DotNetSDK.Exceptions
{
    public class InvalidNotification : Exception
    {
        public InvalidNotification()
        {
        }

        public InvalidNotification(string message) : base(message)
        {

        }

        public InvalidNotification(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidNotification(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidNotification(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}

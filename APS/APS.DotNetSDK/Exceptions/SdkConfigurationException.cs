using System;
using System.Runtime.Serialization;

namespace APS.DotNetSDK.Exceptions
{
    public class SdkConfigurationException : Exception
    {
        public SdkConfigurationException()
        {
        }

        public SdkConfigurationException(string message) : base(message)
        {

        }

        public SdkConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SdkConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SdkConfigurationException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}

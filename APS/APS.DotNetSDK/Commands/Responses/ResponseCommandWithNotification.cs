using APS.DotNetSDK.Exceptions;
using System.Collections.Generic;

namespace APS.DotNetSDK.Commands.Responses
{
    public abstract class ResponseCommandWithNotification : ResponseCommand
    {
        private const string SignatureKey = "signature";

        public void BuildNotification(IDictionary<string, string> dictionaryObject)
        {
            if (!dictionaryObject.Keys.Contains(SignatureKey))
            {
                throw new InvalidNotification(
                    "Response does not contain any signature. Please contact the SDK provider.");
            }

            BuildNotificationCommand(dictionaryObject);
        }

        public abstract void BuildNotificationCommand(IDictionary<string, string> dictionaryObject);

        public abstract string Command { get; }
    }
}
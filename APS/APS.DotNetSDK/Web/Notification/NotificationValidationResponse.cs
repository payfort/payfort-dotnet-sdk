using System.Collections.Generic;

namespace APS.DotNetSDK.Web.Notification
{
    public class NotificationValidationResponse
    {
        public bool IsValid { get; set; }

        public IDictionary<string, string> RequestData { get; set; }
    }
}

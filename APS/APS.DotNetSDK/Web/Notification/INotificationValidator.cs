using Microsoft.AspNetCore.Http;

namespace APS.DotNetSDK.Web.Notification
{
    public interface INotificationValidator
    {
        /// <summary>
        /// Extract the necessary information from the HttpRequest, validate form post
        /// parameters and signature
        /// </summary>
        /// <param name="httpRequest">The HttpRequest received from the gateway</param>
        /// <returns>The notification validation response</returns>
        /// <exception cref="Exceptions.InvalidNotification"> When the notification is invalid</exception>
        /// <exception cref="Exceptions.SignatureException">Get the exception when there are more than two levels of reference</exception>
        NotificationValidationResponse Validate(HttpRequest httpRequest, string nameAccount = null);

        /// <summary>
        /// Extract the necessary information from the HttpRequest, validate async notification
        /// Use this when the payment process is interrupt
        /// </summary>
        /// <param name="httpRequest">The HttpRequest received from the gateway</param>
        /// <returns>The notification validation response</returns>
        /// <exception cref="Exceptions.InvalidNotification"> When the notification is invalid</exception>
        /// <exception cref="Exceptions.SignatureException">Get the exception when there are more than two levels of reference</exception>
        NotificationValidationResponse ValidateAsyncNotification(HttpRequest httpRequest, string nameAccount = null);
    }
}
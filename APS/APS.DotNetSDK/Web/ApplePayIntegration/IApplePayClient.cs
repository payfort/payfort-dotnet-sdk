using System;
using System.Threading.Tasks;

namespace APS.DotNetSDK.Web.ApplePayIntegration
{
    public interface IApplePayClient : IDisposable
    {
        /// <summary>
        /// Get the ApplePay merchant session
        /// </summary>
        /// <param name="url">the url to make the call to ApplePay </param>
        /// <returns>the ApplePay session response object</returns>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        /// <exception cref="ArgumentException">Get the exception when the provided url is not valid with https scheme</exception>
        Task<PaymentSessionResponse> RetrieveMerchantSessionAsync(string url);
    }
}
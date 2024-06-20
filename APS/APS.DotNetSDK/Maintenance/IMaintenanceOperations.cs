using System;
using System.Threading.Tasks;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Commands.Requests.ApplePay;
using APS.DotNetSDK.Commands.Responses.ApplePay;

namespace APS.DotNetSDK.Maintenance
{
    public interface IMaintenanceOperations : IDisposable
    {
        /// <summary>
        /// An operation that allows the Merchant to make an authorization
        /// </summary>
        /// <param name="command">The request command to make an authorization</param>
        /// <returns>The authorize response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<AuthorizeResponseCommand> AuthorizeAsync(AuthorizeRequestCommand command, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to make a purchase
        /// </summary>
        /// <param name="command">The request command to make a purchase</param>
        /// <returns>The purchase response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<PurchaseResponseCommand> PurchaseAsync(PurchaseRequestCommand command, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to make an authorization with ApplePay
        /// </summary>
        /// <param name="authorizeRequestCommand">The request command to make an authorization</param>
        /// <param name="applePayRequestCommand">The Apple Pay request command</param>
        /// <returns>The authorize response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exceptions.SdkConfigurationException">Get the exception when SDK configuration for Apple Pay is not send</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<ApplePayAuthorizeResponseCommand> AuthorizeAsync(AuthorizeRequestCommand authorizeRequestCommand,
            ApplePayRequestCommand applePayRequestCommand, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to make a purchase with ApplePay
        /// </summary>
        /// <param name="purchaseRequestCommand">The request command to make a purchase</param>
        /// /// <param name="applePayRequestCommand">The Apple Pay request command</param>
        /// <returns>The purchase response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exceptions.SdkConfigurationException">Get the exception when SDK configuration for Apple Pay is not send</exception>
        Task<ApplePayPurchaseResponseCommand> PurchaseAsync(PurchaseRequestCommand purchaseRequestCommand, 
            ApplePayRequestCommand applePayRequestCommand, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to capture the authorized amount to their account.
        /// </summary>
        /// <param name="command">The request command for capture operation</param>
        /// <returns>The capture response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<CaptureResponseCommand> CaptureAsync(CaptureRequestCommand command, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to cancel the authorized amount after a successful authorize request
        /// </summary>
        /// <param name="command">The request command for void operation</param>
        /// <returns>The void response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<VoidResponseCommand> VoidAsync(VoidRequestCommand command, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to return the entire amount of a transaction, or to return a part of it
        /// </summary>
        /// <param name="command">The request command for refund operation</param>
        /// <returns>The refund response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<RefundResponseCommand> RefundAsync(RefundRequestCommand command, string nameAccount = null);

        /// <summary>
        /// An operation that allows the Merchant to check the status of a transaction
        /// </summary>
        /// <param name="command">The request command for check status operation</param>
        /// <returns>The check status response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<CheckStatusResponseCommand> CheckStatusAsync(CheckStatusRequestCommand command, string nameAccount = null);
    }
}
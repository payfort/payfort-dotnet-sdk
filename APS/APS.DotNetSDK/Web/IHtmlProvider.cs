using APS.DotNetSDK.Commands.Requests;

namespace APS.DotNetSDK.Web
{
    public interface IHtmlProvider
    {
        /// <summary>
        /// Provides the Html Form necessary for initiating a redirect using Authorize payment request
        /// </summary>
        /// <param name="command">The requested command</param>
        /// <returns>A string representing the built html form </returns>
        string GetHtmlForRedirectIntegration(AuthorizeRequestCommand command);

        /// <summary>
        /// Provides the Html Form necessary for initiating a redirect using Purchase payment request
        /// </summary>
        /// <param name="command">The requested command</param>
        /// <returns>A string representing the built html form </returns>
        string GetHtmlForRedirectIntegration(PurchaseRequestCommand command);

        /// <summary>
        /// Provides the Html Form necessary for initiating a standard IFrame integration
        /// </summary>
        /// <param name="command">The requested command</param>
        /// <returns>A string representing the built html form </returns>
        string GetHtmlTokenizationForStandardIframeIntegration(TokenizationRequestCommand command);

        /// <summary>
        /// Provides the Html Form necessary for initiating a custom integration
        /// </summary>
        /// <param name="command">The requested command</param>
        /// <returns>A string representing the built html form </returns>
        string GetHtmlTokenizationForCustomIntegration(TokenizationRequestCommand command);
        
        /// <summary>
        /// Provides the JavaScript to close the 3ds modal
        /// </summary>
        /// <returns>A string representing the JavaScript for closing the modal</returns>
        string GetJavaScriptToCloseModal();

        /// <summary>
        /// Provides the JavaScript to close the iframe and modal
        /// </summary>
        /// <param name="secure3dsUrl">The url to 3ds</param>
        /// <param name="useModal">If is set to true, returns the modal content for 3ds.</param>
        /// <param name="standardCheckout">Set to true when you want to use modal on standardCheckout</param>
        /// <returns>A string representing the JavaScript for closing iframe.
        /// Also provides the modal content or the redirect JavaScript</returns>
        string Handle3dsSecure(string secure3dsUrl, bool useModal, bool standardCheckout);

        /// <summary>
        /// Provides the JavaScript used for fingerPrint
        /// </summary>
        /// <returns>A string representing the JavaScript used for fingerprint</returns>
        string GetJavaScriptForFingerPrint();
    }
}
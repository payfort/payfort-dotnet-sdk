using WebApp.Util;
using APS.DotNetSDK.Web;
using Microsoft.AspNetCore.Mvc;
using APS.DotNetSDK.Maintenance;
using APS.DotNetSDK.Web.Notification;
using APS.DotNetSDK.Commands.Requests;

namespace WebApp.Controllers
{
    public class StandardCheckoutController : Controller
    {
        private readonly HtmlProvider _htmlProvider;
        private readonly NotificationValidator _notificationValidator;

        public StandardCheckoutController()
        {
            _htmlProvider = new HtmlProvider();
            _notificationValidator = new NotificationValidator();
        }

        public IActionResult StandardCheckout()
        {
            TokenizationRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                ReturnUrl = @"https://localhost:7137/StandardCheckout/ResponseTokenization"
            };

            var redirectFormPost = _htmlProvider.GetHtmlTokenizationForStandardIframeIntegration(command);

            ViewBag.FormPost = redirectFormPost;
            return View(false);
        }

        public IActionResult StandardCheckoutInstallments()
        {
            TokenizationRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                Amount = 2430000,
                Currency = "AED",
                ReturnUrl = @"https://localhost:7137/StandardCheckout/ResponseTokenizationInstallments",
                Installments = "STANDALONE",
            };

            var redirectFormPost = _htmlProvider.GetHtmlTokenizationForStandardIframeIntegration(command);

            ViewBag.FormPost = redirectFormPost;
            return View();
        }

        public async Task<IActionResult> ResponseTokenizationAsync()
        {
            var request = ValidateRequest(out var result);

            var url = string.Empty;
            if (result.IsValid)
            {
                WebResponse webResponse = new WebResponse(request);
                var purchaseCommand = new PurchaseRequestCommand()
                {
                    MerchantReference = webResponse.merchantRef,
                    Amount = 24300,
                    Currency = "AED",
                    TokenName = webResponse.tokenName,
                    CustomerEmail = "andreea.cuc@endava.com",
                    Language = "en",
                    ReturnUrl = @"https://localhost:7137/Home/Response",
                    PhoneNumber = "+1 (201)-234-6789",
                    Description = "test' with #'",
                    CustomerName = "test customer's name"
                };

                MaintenanceOperations operations = new();
                var response = await operations.PurchaseAsync(purchaseCommand); 
                url = ResponseHelper.GetRedirectUrl(response);

                if (!string.IsNullOrEmpty(response.Secure3dsUrl))
                {
                    ViewBag.CloseIframe = _htmlProvider.Handle3dsSecure(response.Secure3dsUrl, true, true);

                    return View("StandardCheckout", true);
                }
                else
                {
                    response.HtmlDecodeProperties().UrlEncodeProperties();
                    url = ResponseHelper.GetRedirectUrl(response);
                }
            }
            return Redirect(url);
        }
        
        public async Task<IActionResult> ResponseTokenizationInstallmentsAsync()
        {
            var request = ValidateRequest(out var result);

            var url = string.Empty;
            if (result.IsValid)
            {
                WebResponse webResponse = new WebResponse(request);
                var purchaseCommand = new PurchaseRequestCommand()
                {
                    MerchantReference = webResponse.merchantRef,
                    Amount = 2430000,
                    Currency = "AED",
                    TokenName = webResponse.tokenName,
                    CustomerEmail = "andreea.cuc@endava.com",
                    Language = "en",
                    Installments = "YES",
                    PlanCode = webResponse.planCode,
                    IssuerCode = webResponse.issuerCode,
                    ReturnUrl = @"https://localhost:7137/Home/Response"
                };

                MaintenanceOperations operations = new();
                var response = await operations.PurchaseAsync(purchaseCommand);
                url = ResponseHelper.GetRedirectUrl(response);
            }

            return Redirect(url);
        }

        private HttpRequest ValidateRequest(out NotificationValidationResponse result)
        {
            var request = this.HttpContext.Request;
            result = _notificationValidator.Validate(request);
            return request;
        }
    }
}

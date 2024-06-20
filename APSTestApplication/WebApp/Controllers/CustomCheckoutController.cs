using WebApp.Util;
using System.Text;
using APS.DotNetSDK.Web;
using Microsoft.AspNetCore.Mvc;
using APS.DotNetSDK.Maintenance;
using APS.DotNetSDK.Web.Installments;
using APS.DotNetSDK.Web.Notification;
using APS.DotNetSDK.Commands.Requests;

namespace WebApp.Controllers
{
    public class CustomCheckoutController : Controller
    {
        private readonly HtmlProvider _htmlProvider;
        private readonly NotificationValidator _notificationValidator;

        public CustomCheckoutController()
        {
            _htmlProvider = new HtmlProvider();
            _notificationValidator = new NotificationValidator();
        }

        public IActionResult CustomCheckout()
        {
            TokenizationRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                ReturnUrl = @"https://localhost:7137/CustomCheckout/ResponseCustomTokenization"
            };

            var redirectFormPost = _htmlProvider.GetHtmlTokenizationForCustomIntegration(command);

            ViewBag.FormPost = redirectFormPost;

            return View();
        }

        public async Task<IActionResult> CustomCheckoutInstallments()
        {
            InstallmentsProvider installmentsProvider = new();
            GetInstallmentsRequestCommand installmentsRequestCommand = new()
            {
                Amount = 243000,
                Currency = "AED",
                Language = "en",
            };

            var installmentsPlans = await installmentsProvider.GetInstallmentsPlansAsync(installmentsRequestCommand);

            var stringBuilder = new StringBuilder();

            foreach (var item in installmentsPlans.InstallmentDetail.InstallmentsIssuerDetail.Where(item => item.PlanDetails?.Count() > 0))
            {
                stringBuilder.AppendLine($"IssuerCode: {item.IssuerCode}");
                stringBuilder.AppendLine($"PlanCode: {item.PlanDetails?.FirstOrDefault()?.PlanCode}");
                stringBuilder.AppendLine($"AmountPerMonth: {item.PlanDetails?.FirstOrDefault()?.AmountPerMonth.ToString()}");
            }

            ViewBag.InstallmentsIssuerDetail = stringBuilder.ToString();

            TokenizationRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                ReturnUrl = @"https://localhost:7137/CustomCheckout/ResponseCustomTokenizationInstallments"
            };

            var redirectFormPost = _htmlProvider.GetHtmlTokenizationForCustomIntegration(command);

            ViewBag.FormPost = redirectFormPost;
            return View();
        }

        public async Task<IActionResult> ResponseCustomTokenizationAsync()
        {
            var request = ValidateRequest(out var result);

            var url = string.Empty;
            if (result.IsValid)
            {
                WebResponse webResponse = new WebResponse(request);
                var authorizeCommand = new AuthorizeRequestCommand()
                {
                    MerchantReference = webResponse.merchantRef,
                    Amount = 24300,
                    Currency = "AED",
                    TokenName = webResponse.tokenName,
                    CustomerEmail = "andreea.cuc@endava.com",
                    Language = "en",
                    ReturnUrl = @"https://localhost:7137/Home/Response",
                    CustomerIp = "172.254.12.5",
                    PhoneNumber = "+1 (201)-234-6789",
                    CustomerName = "test customer's name"
                };

                MaintenanceOperations operations = new();
                var response = await operations.AuthorizeAsync(authorizeCommand);

                if (!string.IsNullOrEmpty(response.Secure3dsUrl))
                {
                    var modal = _htmlProvider.Handle3dsSecure(response.Secure3dsUrl, true);
                    ViewBag.Modal = modal;

                    return View("CustomCheckout");
                }
                else
                {
                    response.HtmlDecodeProperties().UrlEncodeProperties();
                    url = ResponseHelper.GetRedirectUrl(response);
                }
            }

            return Redirect(url);
        }

        private HttpRequest ValidateRequest(out NotificationValidationResponse result)
        {
            var request = this.HttpContext.Request;
            result = _notificationValidator.Validate(request);
            return request;
        }

        public async Task<IActionResult> ResponseCustomTokenizationInstallmentsAsync()
        {
            var request = ValidateRequest(out var result);

            var url = string.Empty;
            if (result.IsValid)
            {
                WebResponse webResponse = new WebResponse(request);

                var purchaseCommand = new PurchaseRequestCommand()
                {
                    MerchantReference = webResponse.merchantRef,
                    Amount = 243000,
                    Currency = "AED",
                    TokenName = webResponse.tokenName,
                    CustomerEmail = "andreea.cuc@endava.com",
                    Language = "en",
                    ReturnUrl = @"https://localhost:7137/Home/Response",

                    Installments = "HOSTED",
                    IssuerCode = "zaQnN1",
                    PlanCode = "JyeZ9a"
                };

                MaintenanceOperations operations = new();
                var response = await operations.PurchaseAsync(purchaseCommand);
                url = ResponseHelper.GetRedirectUrl(response);
            }

            return Redirect(url);
        }
    }
}

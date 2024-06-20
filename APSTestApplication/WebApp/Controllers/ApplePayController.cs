using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using APS.DotNetSDK.Maintenance;
using APS.DotNetSDK.Web.Notification;
using System.Text.Json.Serialization;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Web.ApplePayIntegration;
using APS.DotNetSDK.Commands.Requests.ApplePay;

namespace WebApp.Controllers
{
    public class ApplePayController : Controller
    {
        private ILogger<ApplePayController> _logger;
        private readonly IConfiguration _configuration;

        public ApplePayController(ILogger<ApplePayController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        public IActionResult ApplePay()
        {
            IJavascriptProvider provider = new JavascriptProvider();
            ViewBag.JavascriptContent = provider.GetJavaScriptForApplePayIntegration(
                "/ApplePay/RetrieveMerchantSession",
                "/ApplePay/AuthorizeOrPurchase",
                "AE",
                "AED",
                new List<string>
                {
                    "visa", "masterCard", "amex", "mada"
                });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RetrieveMerchantSession(string url)
        {
            _logger.LogInformation($"Ajax RetrieveMerchantSessionUrl:{url}");
            var client = new ApplePayClient();
            var result = await client.RetrieveMerchantSessionAsync(url);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AuthorizeOrPurchase(ApplePayRequestCommand inputData)
        {
            string req = JsonSerializer.Serialize(inputData,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

            _logger.LogInformation($"Ajax AuthorizeOrPurchase:{req}");

            var purchaseRequestCommand = new PurchaseRequestCommand()
            {
                MerchantReference = _configuration.GetSection("SdkConfig:MerchantReference").Value,
                Amount = 100,
                Currency = "AED",
                CustomerEmail = "andreea.cuc@endava.com",
                Language = "en",
                ReturnUrl = _configuration.GetSection("SdkConfig:ReturnUrl").Value
            };

            var maintenanceOperation = new MaintenanceOperations();

            var response = await maintenanceOperation.PurchaseAsync(purchaseRequestCommand, inputData);

            return this.Json(JsonSerializer.Serialize(response,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }));
        }

        public IActionResult ApplePayApsResponse()
        {
            NotificationValidator notificationValidator = new();

            var request = this.HttpContext.Request;

            var result = notificationValidator.Validate(request);

            _logger.LogInformation($"Notification Response:{result.IsValid}");

            return View("ApplePay");
        }
    }
}

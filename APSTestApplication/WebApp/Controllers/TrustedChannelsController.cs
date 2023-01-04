using WebApp.Util;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using APS.DotNetSDK.Maintenance;
using System.Text.Json.Serialization;
using APS.DotNetSDK.Commands.Requests;

namespace WebApp.Controllers
{
    public class TrustedChannelsController : Controller
    {
        public async Task<IActionResult> MotoCheck()
        {
            const string tokenName = "07f5cade52ed4f95b0b7582fb33e10c2";

            AuthorizeRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                Amount = 24300,
                Currency = "AED",
                CustomerEmail = "test@mail.com",
                ReturnUrl = @"https://localhost:7137/Home/Response",
                TokenName = tokenName,
                Eci = "MOTO"
            };
            MaintenanceOperations operations = new();
            var response = await operations.AuthorizeAsync(command);

            var serialized = JsonSerializer.Serialize(response,
                  new JsonSerializerOptions
                  {
                      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                  });

            ViewBag.Response = serialized;

            return View("Response");
        }

        public async Task<IActionResult> RecurringCheck()
        {
            const string tokenName = "07f5cade52ed4f95b0b7582fb33e10c2";

            PurchaseRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                Amount = 24300,
                Currency = "AED",
                CustomerEmail = "test@mail.com",
                ReturnUrl = @"https://localhost:7137/Home/Response",
                TokenName = tokenName,
                Eci = "RECURRING"
            };

            MaintenanceOperations operations = new();
            var response = await operations.PurchaseAsync(command);

            var serialized = JsonSerializer.Serialize(response,
                  new JsonSerializerOptions
                  {
                      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                  });

            ViewBag.Response = serialized;

            return View("Response");
        }

        public async Task<IActionResult> TrustedCheck()
        {
            PurchaseRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                Amount = 24300,
                Currency = "AED",
                CustomerEmail = "test@mail.com",
                ReturnUrl = @"https://localhost:7137/Home/Response",
                SecurityCode = "123",
                CardNumber = "4000000000000001",
                ExpiryDate = "3505",
                CardHolderName = "Name",
                Eci = "ECOMMERCE"
            };

            MaintenanceOperations operations = new();
            var response = await operations.PurchaseAsync(command);

            var serialized = JsonSerializer.Serialize(response,
                  new JsonSerializerOptions
                  {
                      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                  });

            if (!string.IsNullOrEmpty(response.Secure3dsUrl))
            {
                return Redirect(response.Secure3dsUrl);
            }
            ViewBag.Response = serialized;

            return View("Response");
        }
    }
}

using WebApp.Util;
using APS.DotNetSDK.Web;
using Microsoft.AspNetCore.Mvc;
using APS.DotNetSDK.Commands.Requests;

namespace WebApp.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult Redirect()
        {
            HtmlProvider provider = new();
            AuthorizeRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                Amount = 243,
                Currency = "AED",
                CustomerEmail = "test@mail.com",
                ReturnUrl = @"https://localhost:7137/Home/Response",
                Description = "order ' description",
                CustomerName = "customer name's"
            };

            var redirectFormPost = provider.GetHtmlForRedirectIntegration(command);

            ViewBag.FormPost = redirectFormPost;

            return View();
        }

        public IActionResult RedirectInstallments()
        {
            HtmlProvider provider = new();
            PurchaseRequestCommand command = new()
            {
                MerchantReference = RandomMerchRefGenerator.GetRandomMerchRef(),
                Language = "en",
                Amount = 2430000,
                Currency = "AED",
                CustomerEmail = "test@mail.com",
                ReturnUrl = @"https://localhost:7137/Home/Response",
                Installments = "STANDALONE",
                Description = "order ' description",
                CustomerName = "customer name's"
            };

            var redirectFormPost = provider.GetHtmlForRedirectIntegration(command);

            ViewBag.FormPost = redirectFormPost;

            return View();
        }
    }
}

using System.Text;
using WebApp.Models;
using APS.DotNetSDK.Web;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using APS.DotNetSDK.Web.Notification;
using WebApp.Util;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HtmlProvider _htmlProvider;

        public HomeController()
        {
            _htmlProvider = new HtmlProvider();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ResponseAsync()
        {
            NotificationValidator notificationValidator = new();
            var request = this.HttpContext.Request;

            NotificationValidationResponse result;
            if (request.ContentType == "application/json")
            {
                result = notificationValidator.ValidateAsyncNotification(request);
            }
            else
            {
                var decodedQueryString = DecodeEncode.DecodeQueryString(request.QueryString);

                request.QueryString = decodedQueryString;

                result = notificationValidator.Validate(request);
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Response is valid: {result.IsValid}");

            foreach (var item in result.RequestData)
            {
                stringBuilder.AppendLine($"{item.Key}: {item.Value}");
            }

            ViewBag.Response = stringBuilder.ToString();
            ViewBag.JavaScriptToCloseIframe = _htmlProvider.Handle3dsSecure();
            ViewBag.JavaScriptToCloseModal = _htmlProvider.GetJavaScriptToCloseModal();

            return View(true);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
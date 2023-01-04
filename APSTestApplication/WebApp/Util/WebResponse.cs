using System.Collections;
using System.Web;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Web.Notification;
using Microsoft.Extensions.Primitives;

namespace WebApp.Util;

public class WebResponse
{
    public string tokenName { get; set; }
    public string merchantRef { get; set; }
    public string planCode { get; set; }
    public string issuerCode { get; set; }

    private static string SUCCESS_STATUS_CODE = "18";

    public WebResponse(HttpRequest request)
    {
        if (request.Method == "POST") {
            var resultResponse = request.Form;
            if (resultResponse["status"].Equals(SUCCESS_STATUS_CODE)){
                tokenName = resultResponse["token_name"];
                merchantRef = resultResponse["merchant_reference"];
                planCode = resultResponse["plan_code"];
                issuerCode = resultResponse["issuer_code"];
            }
            else
            {
                throw new InvalidNotification("Error response received");
            }
        }
        if (request.Method == "GET")
        {
            var resultResponse = HttpUtility.ParseQueryString(request.QueryString.Value);
            if (resultResponse["status"].Equals(SUCCESS_STATUS_CODE))
            {
                tokenName = resultResponse["token_name"];
                merchantRef = resultResponse["merchant_reference"];
                planCode = resultResponse["plan_code"];
                issuerCode = resultResponse["issuer_code"];
            }
            else
            {
                throw new InvalidNotification("Error response received");
            }
        }
    }
    
}
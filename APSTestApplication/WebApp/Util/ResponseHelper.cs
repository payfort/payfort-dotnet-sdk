using APS.DotNetSDK.Commands.Responses;

namespace WebApp.Util;

public class ResponseHelper
{
    public static string GetRedirectUrl(PurchaseResponseCommand response)
    {
        var url = String.Empty;
        if (string.IsNullOrEmpty(response.Secure3dsUrl))
        {
            url = $"/Home/Response?{string.Join("&", ObjectToDictionaryHelper.ToDictionary(response).Select(kvp => $"{kvp.Key.ToLower()}={kvp.Value}"))}";
                 
        } else
        {
            url = response.Secure3dsUrl;
        }

        return url;
    }
    
    public static string GetRedirectUrl(AuthorizeResponseCommand response)
    {
        var url = String.Empty;
        if (string.IsNullOrEmpty(response.Secure3dsUrl))
        {
            url = $"/Home/Response?{string.Join("&", ObjectToDictionaryHelper.ToDictionary(response).Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
                 
        } else
        {
            url = response.Secure3dsUrl;
        }

        return url;
    }
    
}
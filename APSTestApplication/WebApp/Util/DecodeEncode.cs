using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApp.Util
{
    public static class DecodeEncode
    {
        public static object HtmlDecodeProperties(this object responseObject)
        {
            var properties = responseObject.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propertyValue = prop.GetValue(responseObject, null);

                if (propertyValue == null || prop.GetSetMethod() == null || prop.PropertyType != typeof(string))
                {
                    continue;
                }

                prop.SetValue(responseObject, HttpUtility.HtmlDecode(propertyValue.ToString()), null);
            }

            return responseObject;
        }

        public static object UrlEncodeProperties(this object responseObject)
        {
            var properties = responseObject.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propertyValue = prop.GetValue(responseObject, null);

                if (propertyValue == null || prop.GetSetMethod() == null || prop.PropertyType != typeof(string))
                {
                    continue;
                }

                prop.SetValue(responseObject, HttpUtility.UrlEncode(propertyValue.ToString()), null);
            }

            return responseObject;
        }

        public static QueryString DecodeQueryString(QueryString requestQueryString)
        {
            var encodedDictionary = new Dictionary<string, string>();
            var queryStringCollection = ParseQueryString(requestQueryString);

            foreach (var item in queryStringCollection)
            {
                encodedDictionary.Add(item.Key, HttpUtility.UrlEncode(HttpUtility.HtmlEncode(item.Value), Encoding.UTF8));
            }

            var querystring = new QueryString($"?{string.Join("&", encodedDictionary.Select(kvp => $"{kvp.Key.ToLower()}={kvp.Value}"))}");
            
            return querystring;
        }

        private static IDictionary<string, string> ParseQueryString(QueryString queryString)
        {
            var collection = HttpUtility.ParseQueryString(queryString.Value);
            return collection.AllKeys.ToDictionary(k => k, k => collection[k]);
        }
    }
}

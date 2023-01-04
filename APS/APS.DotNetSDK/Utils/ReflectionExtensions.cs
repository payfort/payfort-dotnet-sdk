using System.Reflection;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Utils
{
    public static class ReflectionExtensions
    {
        public static string GetJsonPropertyName(this MemberInfo prop)
        {
            var attribute =
                (JsonPropertyNameAttribute[])prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true);
            var jsonPropertyName = attribute.Length > 0 ? attribute[0].Name : prop.Name;
            return jsonPropertyName;
        }

        public static bool GetJsonPropertyIgnoreSignatureValidation(this MemberInfo prop)
        {
            var attribute =
                (IgnoreOnSignatureCalculationAttribute[])prop.GetCustomAttributes(
                    typeof(IgnoreOnSignatureCalculationAttribute), false);

            return attribute.Length > 0;
        }
    }
}

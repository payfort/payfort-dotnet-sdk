using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Utils
{
    public static class DictionaryExtensions
    {
        public static T CreateObject<T>(this IDictionary<string, string> dictionaryObject)
        {
            var jsonObject = JsonSerializer.Serialize(dictionaryObject);
            var responseCommand = JsonSerializer.Deserialize<T>(jsonObject,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                });

            return responseCommand;
        }

        public static string JoinElements(this IDictionary<string, string> dictionary, string separator)
        {
            if(dictionary == null)
            {
               return string.Empty;
            }

            return string.Join(separator, dictionary.Select(x => x.Key + "=" + x.Value).ToArray());
        }
    }
}

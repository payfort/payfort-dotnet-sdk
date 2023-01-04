using System.Linq;
using System.Text.Json;
using APS.DotNetSDK.Utils;

namespace APS.DotNetSDK.AcquirerResponseMessage
{
    public class AcquirerResponseMapping
    {
        private readonly AcquirerJsonResponseMapping _acquirerJsonResponseMapping;
        
        public AcquirerResponseMapping()
        {
            var fileContent = FileReader
                .GetEmbeddedResourceContent("APS.DotNetSDK.AcquirerResponseMessage.AcquirerResponseMappingJson.json");
            
            _acquirerJsonResponseMapping =
                JsonSerializer.Deserialize<AcquirerJsonResponseMapping>(fileContent);
        }

        public string GetAcquirerResponseDescription(string acquirerResponseCode)
        {
            if (acquirerResponseCode == null)
            {
                return null;
            }

            var acquirerResponse = _acquirerJsonResponseMapping.AcquirerResponse
                .FirstOrDefault(x => x.AcquirerResponseCode == acquirerResponseCode);

            return acquirerResponse == null ? "Unrecognized acquirer response code" : acquirerResponse.AcquirerResponseDescription;
        }
    }
}

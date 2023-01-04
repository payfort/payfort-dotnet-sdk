using System.Linq;
using System.Text.Json;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Exceptions;

namespace APS.DotNetSDK.Configuration
{
    public class ApsConfiguration
    {
        private readonly bool _isTestEnvironment;
        private readonly ApsJsonConfiguration _apsJsonConfiguration;
        

        public ApsConfiguration(bool isTestEnvironment)
        {
            _isTestEnvironment = isTestEnvironment;

            var fileContent = FileReader.GetEmbeddedResourceContent("APS.DotNetSDK.Configuration.ApsConfiguration.json");

            _apsJsonConfiguration =
                JsonSerializer.Deserialize<ApsJsonConfiguration>(fileContent);
        }

        public ApsEnvironmentConfiguration GetEnvironmentConfiguration()
        {
            string environmentName = _isTestEnvironment ? "Test" : "Production";

            var environment = _apsJsonConfiguration.Environments
                .FirstOrDefault(x => x.Name == environmentName);
            if (environment == null)
            {
                throw new ConfigurationException(
                    "There was an issue with the .NET SDK configuration. Please contact the SDK provider.");
            }

            return environment;
        }

        public ApplePaySessionRetrievalConfiguration GetApplePayConfiguration()
        {
            return _apsJsonConfiguration.ApplePaySessionRetrieval;
        }

        public string GetRedirectFormPostTemplate()
        {
            return _apsJsonConfiguration.RedirectFormTemplate;
        }

        public string GetStandardIframeFormPostTemplate()
        {
            return _apsJsonConfiguration.StandardFormTemplate;
        }

        public string GetCustomFormPostTemplate()
        {
            return _apsJsonConfiguration.CustomFormTemplate;
        }

        public string GetCloseModalJavaScript()
        {
            return _apsJsonConfiguration.CloseModalJavaScript;
        }

        public string GetCloseIframeJavaScript()
        {
            return _apsJsonConfiguration.CloseIframeJavaScript;
        }
    }
}
using System;
using APS.DotNetSDK.Service;
using System.Threading.Tasks;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace APS.DotNetSDK.Web.ApplePayIntegration
{
    public class ApplePayClient : IApplePayClient
    {
        private readonly IApiProxy _apiProxy;
        private readonly ILogger<ApplePayClient> _logger;
        private readonly ApplePaySessionRetrievalConfiguration _configuration;
       

        private readonly object _lockObject = new object();
        private bool _disposed;


        internal ApplePayClient(IApiProxy apiProxy)
        {
            var configuration = new ApsConfiguration(SdkConfiguration.AllConfigurations.First().IsTestEnvironment);
            _configuration = configuration.GetApplePayConfiguration();
            
            _logger = SdkConfiguration.LoggerFactory.CreateLogger<ApplePayClient>();

            _apiProxy = apiProxy;
        }

        /// <summary>
        /// Constructor for ApplePay Client
        /// </summary>
        /// <exception cref="Exceptions.SdkConfigurationException">Get the exception when sdk configuration is not set</exception>
        public ApplePayClient(string nameAccount = null)
        {
            var account = SdkConfiguration.GetAccount(nameAccount);
            var configuration = new ApsConfiguration(account.IsTestEnvironment);
            _configuration = configuration.GetApplePayConfiguration();

            SdkConfiguration.ValidateApplePayConfiguration(account);

            _apiProxy = new ApiProxy(new HttpClientWrapper(account.ApplePayConfiguration.SecurityCertificate,
                _configuration.SslProtocol));
            
            _logger = SdkConfiguration.LoggerFactory.CreateLogger<ApplePayClient>();

        }

        public async Task<PaymentSessionResponse> RetrieveMerchantSessionAsync(string url, string nameAccount = null)
        {
            var account = SdkConfiguration.GetAccount(nameAccount);

            bool result = Uri.TryCreate(url, UriKind.Absolute, out var uri);
            if (result == false || uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new ArgumentException("Please provide a valid url with https scheme");
            }

            var request = new PaymentSessionRequest()
            {
                MerchantIdentifier = account.ApplePayConfiguration.MerchantUid,
                Initiative = _configuration.Initiative,
                InitiativeContext = account.ApplePayConfiguration.DomainName,
                DisplayName = account.ApplePayConfiguration.DisplayName
            };

            _logger.LogDebug($"Start retrieving merchant session:[Url:{url}]");

            var baseUrl = uri.GetLeftPart(UriPartial.Authority);

            _logger.LogInformation($"Sending the request to retrieve merchant session:[Request:{@request.ToAnonymizedJson()}]");
            _logger.LogDebug($"Sending the request to retrieve merchant session:[Request:{@request.ToAnonymizedJson()}]");

            try
            {
                var response = await _apiProxy.PostAsync<PaymentSessionRequest, PaymentSessionResponse>(request,
                    baseUrl, uri.AbsolutePath);

                _logger.LogDebug($"Response received from the payment gateway:[Response:{@response.ToAnonymizedJson()}]");
                _logger.LogInformation($"Successfully retrieved merchant session:[Url:{url}]");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}. [Request:{@request.ToAnonymizedJson()}]");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObject)
            {
                if (disposing && _disposed == false)
                {
                    _apiProxy.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}
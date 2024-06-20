using APS.Signature;

namespace APS.DotNetSDK.Configuration
{
    public  class SdkConfigurationDto
    {
        public bool IsTestEnvironment { get; set; }

        public  string AccessCode { get; set; }
        public  string AccountType { get; set; }

        public  string MerchantIdentifier { get; set; }

        public  string RequestShaPhrase { get; set; }

        public  string ResponseShaPhrase { get; set; }

        public  ShaType ShaType { get; set; }

        public  ApplePayConfiguration ApplePayConfiguration { get; set; }

    }
}

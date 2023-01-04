using System;
using System.IO;
using System.Text.Json;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Logging;
using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace APS.DotNetSDK.Configuration
{
    public static class SdkConfiguration
    {
        private static readonly object LockObject = new object();

        public static bool IsTestEnvironment { get; private set; }

        public static string AccessCode { get; set; }

        public static string MerchantIdentifier { get; set; }

        public static string RequestShaPhrase { get; set; }

        public static string ResponseShaPhrase { get; set; }

        public static ShaType ShaType { get; set; }

        public static ApplePayConfiguration ApplePayConfiguration { get; private set; }

        public static bool IsConfigured { get; private set; }

        internal static ServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Configure the SDK
        /// </summary>
        /// <param name="filePath">The MerchantSdkConfiguration.json file path</param>
        /// <param name="loggingConfiguration">The logging configuration</param>
        /// <param name="applePayConfiguration">The ApplePay configuration</param>
        /// <exception cref="ArgumentNullException">Get the exception when one of the mandatory parameters are null or empty</exception>
        public static ServiceProvider Configure(
            string filePath,
            LoggingConfiguration loggingConfiguration,
            ApplePayConfiguration applePayConfiguration = null)
        {
            lock (LockObject)
            {
                if (!File.Exists(filePath))
                {
                    throw new SdkConfigurationException("The file \"MerchantSdkConfiguration.json\" is needed for SDK configuration");
                }

                var jsonContent = FileReader.ReadFromFile(filePath);
                var merchantConfiguration =
                    JsonSerializer.Deserialize<MerchantSdkConfiguration>(jsonContent);

                AccessCode = merchantConfiguration?.SdkConfiguration.AccessCode;
                MerchantIdentifier = merchantConfiguration?.SdkConfiguration.MerchantIdentifier;
                RequestShaPhrase = merchantConfiguration?.SdkConfiguration.RequestShaPhrase;
                ResponseShaPhrase = merchantConfiguration?.SdkConfiguration.ResponseShaPhrase;

                if (!Enum.TryParse(merchantConfiguration?.SdkConfiguration.ShaTypeAsString, out ShaType resultShaType))
                {
                    //details
                    throw new SdkConfigurationException("Please provide one of the shaType \"Sha512\" or \"Sha256\". " +
                        "Is needed in Sdk Configuration. Please check file \"MerchantSdkConfiguration.json\"");
                }
                ShaType = resultShaType;

                if (!Enum.TryParse(merchantConfiguration?.SdkConfiguration.IsTestEnvironmentAsString, out Environment resultEnvironment))
                {
                    //details
                    throw new SdkConfigurationException("Please provide one of IsTestEnvironment \"Test\" or \"Production\". " +
                        "Is needed in Sdk Configuration. Please check file \"MerchantSdkConfiguration.json\"");
                }

                IsTestEnvironment = resultEnvironment == Environment.Test;

                if (string.IsNullOrEmpty(merchantConfiguration?.SdkConfiguration.ApplePay?.MerchantIdentifier)
                    && merchantConfiguration?.SdkConfiguration.ApplePay != null)
                {
                    merchantConfiguration.SdkConfiguration.ApplePay.MerchantIdentifier = merchantConfiguration.SdkConfiguration.MerchantIdentifier;
                }

                ApplePayConfiguration = merchantConfiguration?.SdkConfiguration.ApplePay;

                if (merchantConfiguration.SdkConfiguration.ApplePay != null)
                {
                    if (!Enum.TryParse(merchantConfiguration.SdkConfiguration.ApplePay?.ShaTypeAsString, out ShaType resultApplePayShaType))
                    {
                        //details
                        throw new SdkConfigurationException("Please provide one of the shaType \"Sha512\" or \"Sha256\". " +
                                                            "Is needed in Apple Pay Configuration. Please check file \"MerchantSdkConfiguration.json\"");
                    }

                    merchantConfiguration.SdkConfiguration.ApplePay.ShaType = resultApplePayShaType;
                }

                if (applePayConfiguration != null)
                {
                    if (applePayConfiguration.SecurityCertificate == null)
                    {
                        throw new ArgumentNullException($"SecurityCertificate",
                            "SecurityCertificate is needed for ApplePay configuration");
                    }

                    ApplePayConfiguration.SecurityCertificate = applePayConfiguration?.SecurityCertificate;
                }

                ValidateSdkConfiguration();

                loggingConfiguration.Validate();

                loggingConfiguration.ServiceCollection
                    .AddSerilogLogger(loggingConfiguration.JsonLoggingPathConfig, loggingConfiguration.ApplicationName);

                ServiceProvider = loggingConfiguration.ServiceCollection.BuildServiceProvider();

                return ServiceProvider;
            }
        }

        internal static void Validate()
        {
            if (!IsConfigured)
            {
                throw new SdkConfigurationException("Sdk Configuration is required. " +
                    "Please check MerchantSdkConfiguration.json file");
            }
        }

        internal static void ValidateApplePayConfiguration()
        {
            if (ApplePayConfiguration == null)
            {
                throw new SdkConfigurationException("Sdk Configuration for ApplePay is required. " +
                   "Please check MerchantSdkConfiguration.json file");
            }
        }

        #region private methods
        private static void ValidateSdkConfiguration()
        {
            if (string.IsNullOrEmpty(AccessCode))
            {
                throw new ArgumentNullException("AccessCode", "AccessCode is needed for SDK configuration");
            }

            if (string.IsNullOrEmpty(MerchantIdentifier))
            {
                throw new ArgumentNullException("MerchantIdentifier", "MerchantIdentifier is needed for SDK configuration");
            }

            if (string.IsNullOrEmpty(RequestShaPhrase))
            {
                throw new ArgumentNullException("RequestShaPhrase", "RequestShaPhrase is needed for SDK configuration");
            }

            if (string.IsNullOrEmpty(ResponseShaPhrase))
            {
                throw new ArgumentNullException("ResponseShaPhrase", "ResponseShaPhrase is needed for SDK configuration");
            }

            if (ApplePayConfiguration != null)
            {
                ApplePayConfiguration.Validate();
            }

            IsConfigured = true;
        }
        #endregion
    }
}

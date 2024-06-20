using System;
using System.IO;
using System.Text.Json;
using APS.DotNetSDK.Utils;
//using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Exceptions;
using APS.Signature;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace APS.DotNetSDK.Configuration
{
    public static class SdkConfiguration
    {
        private static readonly object LockObject = new object();
        public static List<SdkConfigurationDto> AllConfigurations;
        private static List<SdkJsonConfiguration> AllJsonConfigurations;

        public static bool IsConfigured { get; private set; }

        public static ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// Configure the SDK
        /// </summary>
        /// <param name="filePath">The MerchantSdkConfiguration.json file path</param>
        /// <param name="loggerFactory"></param>
        /// <param name="applePayConfiguration">The ApplePay configuration</param>
        /// <exception cref="ArgumentNullException">Get the exception when one of the mandatory parameters are null or empty</exception>
        public static void Configure(
            string filePath,
            ILoggerFactory loggerFactory,
            ApplePayConfiguration applePayConfiguration = null)
        {

            AllConfigurations = new List<SdkConfigurationDto>();
            AllJsonConfigurations = new List<SdkJsonConfiguration>();

            lock (LockObject)
            {
                filePath = filePath.Replace(@"\", Path.DirectorySeparatorChar.ToString());
                if (!File.Exists(filePath))
                {
                    throw new SdkConfigurationException("The file \"MerchantSdkConfiguration.json\" is needed for SDK configuration");
                }

                var jsonContent = FileReader.ReadFromFile(filePath);
                var merchantConfigurations =
                    JsonSerializer.Deserialize<MerchantSdkConfiguration>(jsonContent);

                AllJsonConfigurations.Add(merchantConfigurations.SdkConfiguration);

                if (merchantConfigurations.SubAccountsList != null && merchantConfigurations.SubAccountsList.Any())
                {
                    foreach (var merchantConfiguration in merchantConfigurations.SubAccountsList)
                    {
                        AllJsonConfigurations.Add(merchantConfiguration);
                    }
                }

                foreach (var jsonConfiguration in AllJsonConfigurations)
                {
                    var config = new SdkConfigurationDto();


                    config.AccessCode = jsonConfiguration?.AccessCode;
                    config.AccountType = jsonConfiguration?.AccountType;
                    config.MerchantIdentifier = jsonConfiguration?.MerchantIdentifier;
                    config.RequestShaPhrase = jsonConfiguration?.RequestShaPhrase;
                    config.ResponseShaPhrase = jsonConfiguration?.ResponseShaPhrase;

                    if (!Enum.TryParse(jsonConfiguration?.ShaTypeAsString, out ShaType resultShaType))
                    {
                        //details
                        throw new SdkConfigurationException("Please provide one of the shaType \"Sha512\" or \"Sha256\". " +
                            "Is needed in Sdk Configuration. Please check file \"MerchantSdkConfiguration.json\"");
                    }
                    config.ShaType = resultShaType;

                    if (!Enum.TryParse(jsonConfiguration?.IsTestEnvironmentAsString, out Environment resultEnvironment))
                    {
                        //details
                        throw new SdkConfigurationException("Please provide one of IsTestEnvironment \"Test\" or \"Production\". " +
                            "Is needed in Sdk Configuration. Please check file \"MerchantSdkConfiguration.json\"");
                    }

                    config.IsTestEnvironment = resultEnvironment == Environment.Test;

                    if (string.IsNullOrEmpty(jsonConfiguration?.ApplePay?.MerchantIdentifier)
                        && jsonConfiguration?.ApplePay != null)
                    {
                        jsonConfiguration.ApplePay.MerchantIdentifier = jsonConfiguration.MerchantIdentifier;
                    }

                    config.ApplePayConfiguration = jsonConfiguration?.ApplePay;

                    if (jsonConfiguration.ApplePay != null)
                    {
                        if (!Enum.TryParse(jsonConfiguration.ApplePay?.ShaTypeAsString, out ShaType resultApplePayShaType))
                        {
                            //details
                            throw new SdkConfigurationException("Please provide one of the shaType \"Sha512\" or \"Sha256\". " +
                                                                "Is needed in Apple Pay Configuration. Please check file \"MerchantSdkConfiguration.json\"");
                        }

                        jsonConfiguration.ApplePay.ShaType = resultApplePayShaType;
                    }

                    if (applePayConfiguration != null)
                    {
                        if (applePayConfiguration.SecurityCertificate == null)
                        {
                            throw new ArgumentNullException($"SecurityCertificate",
                                "SecurityCertificate is needed for ApplePay configuration");
                        }

                        config.ApplePayConfiguration.SecurityCertificate = applePayConfiguration?.SecurityCertificate;
                    }

                    ValidateSdkConfiguration(config);
                    AllConfigurations.Add(config);
                }

                LoggerFactory = loggerFactory;
                
            }
        }

        public static SdkConfigurationDto GetAccount(string nameAccount)
        {
            var account = new SdkConfigurationDto();
            if (nameAccount != null)
                account = AllConfigurations.Where(x => x.AccountType == nameAccount).FirstOrDefault();
            else
                account = AllConfigurations.FirstOrDefault();

            return account;
        }

        public static void ClearConfiguration()
        {
            AllConfigurations.Clear();
            AllJsonConfigurations.Clear();
        }

        internal static void Validate()
        {
            if (!IsConfigured)
            {
                throw new SdkConfigurationException("Sdk Configuration is required. " +
                    "Please check MerchantSdkConfiguration.json file");
            }
        }

        internal static void ValidateApplePayConfiguration(SdkConfigurationDto currentAccount)
        {
            if (currentAccount.ApplePayConfiguration == null)
            {
                throw new SdkConfigurationException("Sdk Configuration for ApplePay is required. " +
                   "Please check MerchantSdkConfiguration.json file");
            }
        }

        #region private methods
        private static void ValidateSdkConfiguration(SdkConfigurationDto configuration)
        {
            if (string.IsNullOrEmpty(configuration.AccessCode))
            {
                throw new ArgumentNullException("AccessCode", "AccessCode is needed for SDK configuration");
            }

            if (string.IsNullOrEmpty(configuration.MerchantIdentifier))
            {
                throw new ArgumentNullException("MerchantIdentifier", "MerchantIdentifier is needed for SDK configuration");
            }

            if (string.IsNullOrEmpty(configuration.RequestShaPhrase))
            {
                throw new ArgumentNullException("RequestShaPhrase", "RequestShaPhrase is needed for SDK configuration");
            }

            if (string.IsNullOrEmpty(configuration.ResponseShaPhrase))
            {
                throw new ArgumentNullException("ResponseShaPhrase", "ResponseShaPhrase is needed for SDK configuration");
            }

            if (configuration.ApplePayConfiguration != null)
            {
                configuration.ApplePayConfiguration.Validate();
            }

            IsConfigured = true;
        }
        #endregion
    }
}

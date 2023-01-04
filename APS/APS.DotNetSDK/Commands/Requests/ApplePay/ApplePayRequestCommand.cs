using System;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayRequestCommand
    {
        [JsonPropertyName("data")]
        public ApplePayData Data { get; set; }

        public  void ValidateMandatoryProperties()
        {
            if (Data == null) 
            {
                throw new ArgumentNullException($"AppleData", "AppleData is mandatory");
            }

            if (Data.PaymentData == null)
            {
                throw new ArgumentNullException($"AppleData.PaymentData", "AppleData.PaymentData is mandatory");
            }

            if (Data.PaymentData.Header == null)
            {
                throw new ArgumentNullException($"AppleData.PaymentData.Header",
                    "AppleData.PaymentData.Header is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentData.Header.EphemeralPublicKey))
            {
                throw new ArgumentNullException($"Data.PaymentData.Header.EphemeralPublicKey",
                    "Data.PaymentData.Header.EphemeralPublicKey is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentData.Header.PublicKeyHash))
            {
                throw new ArgumentNullException($"Data.PaymentData.Header.PublicKeyHash",
                    "Data.PaymentData.Header.PublicKeyHash is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentData.Header.TransactionId))
            {
                throw new ArgumentNullException($"Data.PaymentData.Header.TransactionId",
                    "Data.PaymentData.Header.TransactionId is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentData.Signature))
            {
                throw new ArgumentNullException($"Data.PaymentData.Signature",
                    "Data.PaymentData.Signature is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentData.Version))
            {
                throw new ArgumentNullException($"Data.PaymentData.Version",
                    "Data.PaymentData.Version is mandatory");
            }

            if (Data.PaymentMethod == null)
            {
                throw new ArgumentNullException($"AppleData.PaymentData", "AppleData.PaymentData is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentMethod.DisplayName))
            {
                throw new ArgumentNullException($"Data.PaymentMethod.DisplayName",
                    "Data.PaymentMethod.DisplayName is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentMethod.Network))
            {
                throw new ArgumentNullException($"Data.PaymentMethod.Network",
                    "Data.PaymentMethod.Network is mandatory");
            }

            if (string.IsNullOrEmpty(Data.PaymentMethod.Type))
            {
                throw new ArgumentNullException($"Data.PaymentMethod.Type",
                    "Data.PaymentMethod.Type is mandatory");
            }
        }
    }
}
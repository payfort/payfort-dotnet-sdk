using System.Text;
using System.Linq;
using System.Reflection;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Exceptions;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace APS.DotNetSDK.Signature
{
    public class SignatureProvider : ISignatureProvider
    {
        public string GetSignature<T>(T input, string shaPhrase, ShaType shaType) where T : Command
        {
            var hash = GetHashSha(PrepareSignature(input, shaPhrase), shaType);
            return hash;
        }

        #region private methods
        private static string GetHashSha(string text, ShaType type)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string hashString = string.Empty;
            byte[] hash;

            if (type == ShaType.Sha512)
            {
                var sha512Managed = new SHA512Managed();
                hash = sha512Managed.ComputeHash(bytes);
            }
            else
            {
                var sha256Managed = new SHA256Managed();
                hash = sha256Managed.ComputeHash(bytes);
            }

            return hash.Aggregate(hashString, (current, x) => current + $"{x:x2}");
        }

        private string PrepareSignature<T>(T input, string shaPhrase)
        {
            var orderedProperties = input.GetType().GetProperties().OrderBy(prop => prop.GetJsonPropertyName());

            var signatureBuilder = new StringBuilder(shaPhrase);

            foreach (var prop in orderedProperties)
            {
                var jsonPropertyName = prop.GetJsonPropertyName();
                var propertyValue = prop.GetValue(input, null);

                if (propertyValue == null || propertyValue.ToString() == string.Empty || prop.GetJsonPropertyIgnoreSignatureValidation())
                {
                    continue;
                }

                if (prop.PropertyType.IsSimpleObject())
                {
                    var formattedPropertyValue = propertyValue.ToString().Trim();
                    signatureBuilder.Append($"{jsonPropertyName}={formattedPropertyValue}");
                }
                else
                {
                    BuildSignatureForComplexItem(prop, propertyValue, signatureBuilder);
                }
            }

            signatureBuilder.Append(shaPhrase);

            return signatureBuilder.ToString();
        }

        private static void BuildSignatureForComplexItem(PropertyInfo propertyInfo, object propertyValue, StringBuilder signatureBuilder)
        {
            var propertyName = propertyInfo.GetJsonPropertyName();

            if (propertyInfo.PropertyType.IsListObject())
            {
                BuildSignatureForListItem(propertyName, propertyValue, signatureBuilder);
            }
            else
            {
                var csvFormattedProperties = GetCsvFormattedProperties(propertyValue);

                if (!string.IsNullOrEmpty(csvFormattedProperties))
                {
                    signatureBuilder.Append($"{propertyName}={csvFormattedProperties}");
                }
            }
        }

        private static void BuildSignatureForListItem(string propertyName, object propertyValue, StringBuilder signatureBuilder)
        {
            var propertyList = propertyValue as IEnumerable<object>;
            if (!propertyList.Any())
            {
                return;
            }

            signatureBuilder.Append($"{propertyName}=[");

            foreach (var listItem in propertyList)
            {
                var csvFormattedProperties = GetCsvFormattedProperties(listItem);

                if (!string.IsNullOrEmpty(csvFormattedProperties))
                {
                    signatureBuilder.Append($"{csvFormattedProperties}, ");
                }
            }

            signatureBuilder.Remove(signatureBuilder.Length - 2, 2);

            signatureBuilder.Append("]");
        }

        private static string GetCsvFormattedProperties(object input)
        {
            var properties = input.GetType().GetProperties();
            if (!properties.Any())
            {
                return string.Empty;
            }

            StringBuilder csvFormattedPropertiesBuilder = new StringBuilder();
            csvFormattedPropertiesBuilder.Append("{");

            foreach (var prop in properties)
            {
                string jsonPropertyName = prop.GetJsonPropertyName();
                var propertyValue = prop.GetValue(input, null);

                if (!prop.PropertyType.IsSimpleObject())
                {
                    throw new SignatureException("Only two levels of reference types are allowed for signature calculation.");
                }

                if (propertyValue == null || propertyValue.ToString() == string.Empty)
                {
                    continue;
                }

                csvFormattedPropertiesBuilder.Append($"{jsonPropertyName}={propertyValue.ToString().Trim()}, ");
            }

            csvFormattedPropertiesBuilder.Remove(csvFormattedPropertiesBuilder.Length - 2, 2);
            csvFormattedPropertiesBuilder.Append("}");

            return csvFormattedPropertiesBuilder.ToString();
        }
        #endregion
    }
}
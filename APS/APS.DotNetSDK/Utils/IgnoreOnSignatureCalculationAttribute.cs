using System;

namespace APS.DotNetSDK.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreOnSignatureCalculationAttribute : Attribute
    {
        public IgnoreOnSignatureCalculationAttribute(bool ignoreOnSignatureCalculation)
        {
            IgnoreOnSignatureCalculation = ignoreOnSignatureCalculation;
        }

        public bool IgnoreOnSignatureCalculation { get; set; }
    }
}
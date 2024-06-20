
namespace APS.Signature
{
    public interface ISignatureValidator
    {
        /// <summary>
        /// Calculates the signature and validates against the expectedSignature
        /// </summary>
        /// <typeparam name="T">The request command type to calculate signature</typeparam>
        /// <param name="input">The request command object to calculate signature</param>
        /// <param name="shaPhrase">The SHA phrase</param>
        /// <param name="shaType">The SHA type</param>
        /// <param name="expectedSignature">The expected signature</param>
        /// <returns>Returns true or false. True when the signature are the same, false is case of mismatching signature</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when there are more than two levels of reference</exception>
        bool ValidateSignature<T>(T input, string shaPhrase, ShaType shaType, string expectedSignature);
    }
}


namespace APS.Signature
{
    public interface ISignatureProvider
    {
        /// <summary>
        /// Calculate signature
        /// </summary>
        /// <typeparam name="T">The request command type to calculate signature</typeparam>
        /// <param name="input">The request command object to calculate signature</param>
        /// <param name="shaPhrase">The SHA phrase</param>
        /// <param name="shaType">The SHA type</param>
        /// <returns>Returns the signature generated based on parameters</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when there are more than two levels of reference</exception>
        string GetSignature<T>(T input, string shaPhrase, ShaType shaType) ;
    }
}
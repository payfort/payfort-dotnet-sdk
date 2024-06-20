
namespace APS.Signature
{
    public class SignatureValidator : ISignatureValidator
    {
        private readonly ISignatureProvider _signatureProvider;

        public SignatureValidator() : this(new SignatureProvider())
        {

        }

        public SignatureValidator(ISignatureProvider signatureProvider)
        {
            this._signatureProvider = signatureProvider;
        }

        public bool ValidateSignature<T>(T input, string shaPhrase, ShaType shaType, string expectedSignature)
        {
            var actualSignature = _signatureProvider.GetSignature(input, shaPhrase, shaType);
            return actualSignature == expectedSignature;
        }
    }
}
using System.Threading.Tasks;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Responses;

namespace APS.DotNetSDK.Web.Installments
{
    public interface IInstallmentsProvider
    {
        /// <summary>
        /// An operation that allows the Merchant to get the installments plans
        /// </summary>
        /// <param name="command">The request command for get installments plans operation</param>
        /// <returns>The get installments response received from the provider</returns>
        /// <exception cref="Exceptions.SignatureException">Get the exception when signature mismatch</exception>
        /// <exception cref="System.Exception">Get the exception when there is an issue to the payment gateway</exception>
        Task<GetInstallmentsResponseCommand> GetInstallmentsPlansAsync(GetInstallmentsRequestCommand command);
    }
}

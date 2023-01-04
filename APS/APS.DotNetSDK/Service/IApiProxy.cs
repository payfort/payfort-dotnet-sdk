using System;
using System.Threading.Tasks;

namespace APS.DotNetSDK.Service
{
    public interface IApiProxy : IDisposable
    {
        /// <summary>
        /// Creates a Post request
        /// </summary>
        /// <typeparam name="TRequest">The request object</typeparam>
        /// <typeparam name="TResponse">The response object</typeparam>
        /// <param name="request">The request object</param>
        /// <param name="baseAddress">The base address</param>
        /// <param name="requestUri">The request uri</param>
        /// <returns>Returns the response received from the provider</returns>
        /// <exception cref="Exceptions.ApiCallException">There is an issue to the payment gateway</exception>
        Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string baseAddress, string requestUri);
    }
}

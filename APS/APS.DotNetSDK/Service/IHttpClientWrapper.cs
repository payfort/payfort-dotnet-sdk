using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APS.DotNetSDK.Service
{
    public interface IHttpClientWrapper : IDisposable
    {
        /// <summary>
        /// Http Client for calling the gateway
        /// </summary>
        /// <param name="baseAddress">The base address</param>
        /// <param name="requestUri">The request uri</param>
        /// <param name="content">The Http Content</param>
        /// <returns>Returns a HttpResponseMessage</returns>
        Task<HttpResponseMessage> PostAsync(string baseAddress, string requestUri, HttpContent content);
    }
}

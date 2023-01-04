using System;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using APS.DotNetSDK.Exceptions;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Service
{
    internal class ApiProxy : IApiProxy
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        private readonly object _lockObject = new object();
        private bool _disposed;

        internal ApiProxy(IHttpClientWrapper httpClientWrapperWrapper)
        {
            _httpClientWrapper = httpClientWrapperWrapper;
        }

        internal ApiProxy()
        {
            _httpClientWrapper = new HttpClientWrapper();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string baseAddress, string requestUri)
        {
            var serialized = new StringContent(JsonSerializer.Serialize(request,
                    new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }),
                Encoding.UTF8,
                "application/json");

            try
            {
                var httpResponseMessage = await _httpClientWrapper.PostAsync(baseAddress, requestUri, serialized);
                var responseCommand = await ProcessResponseAsync<TResponse>(httpResponseMessage);
                
                return responseCommand;
            }
            catch (Exception ex)
            {
                throw new ApiCallException($"There was an issue when calling the APS. Error: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObject)
            {
                if (disposing && _disposed == false)
                {
                    _httpClientWrapper.Dispose();
                    _disposed = true;
                }
            }
        }

        #region private methods
        private static async Task<TResponse> ProcessResponseAsync<TResponse>(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();

                var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<TResponse>(responseMessage,
                    new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        NumberHandling = JsonNumberHandling.AllowReadingFromString
                    });
                return response;
            }
            catch (Exception exception)
            {
                throw new ApiCallException("There was an error when calling the payment gateway", exception);
            }
        }
        #endregion
    }
}
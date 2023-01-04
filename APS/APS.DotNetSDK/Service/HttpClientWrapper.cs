using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace APS.DotNetSDK.Service
{
    internal class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientHandler _handler = new HttpClientHandler();

        private readonly object _lockObject = new object();
        private bool _disposed;

        internal HttpClientWrapper(X509Certificate2 certificate, SslProtocols sslProtocol)
        {
            _handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            _handler.SslProtocols = sslProtocol;
            _handler.ClientCertificates.Add(certificate);

            _httpClient = new HttpClient(_handler);
        }

        internal HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        public Task<HttpResponseMessage> PostAsync(string baseAddress, string requestUri, HttpContent content)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
            return _httpClient.PostAsync(requestUri, content);
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
                    _httpClient.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}

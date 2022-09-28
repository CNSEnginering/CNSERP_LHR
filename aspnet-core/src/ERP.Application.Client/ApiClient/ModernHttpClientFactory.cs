using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http.Configuration;
using ModernHttpClient;

namespace ERP.ApiClient
{
    public class ModernHttpClientFactory : DefaultHttpClientFactory
    {
        /// <summary>
        /// Callback function for refresh token is expired
        /// </summary>
        public Func<Task> OnSessionTimeOut { get; set; }

        /// <summary>
        /// Callback function for access token refresh
        /// </summary>
        public Func<string, Task> OnAccessTokenRefresh { get; set; }

        public override HttpMessageHandler CreateMessageHandler()
        {
            return new AuthenticationHttpHandler(new NativeMessageHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            {
                OnSessionTimeOut = OnSessionTimeOut,
                OnAccessTokenRefresh = OnAccessTokenRefresh
            };
        }
    }
}
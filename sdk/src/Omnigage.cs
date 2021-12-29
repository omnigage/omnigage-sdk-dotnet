using System.Net.Http;
using Omnigage.Runtime;

namespace Omnigage
{
    public class OmnigageClient : Client
    {
        private OmnigageClient() { }

        public static void Init(string tokenKey, string tokenSecret = null, string host = null, HttpClient httpClient = null)
        {
            AuthContext auth = new AuthContext();
            if (tokenSecret != null)
            {
                auth.TokenKey = tokenKey;
                auth.TokenSecret = tokenSecret;
            }
            else
            {
                auth.JWT = tokenKey;
            }

            if (host == null)
            {
                host = "https://api.omnigage.io/api/v1/";
            }

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            Auth = auth;
            Host = host;
            HttpClient = httpClient;
            IsTesting = false;
            TestRequestIncremental = 0;
        }
    }
}

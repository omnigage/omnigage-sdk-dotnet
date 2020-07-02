using System.Net.Http;
using Omnigage.Auth;

namespace Omnigage
{
    public class Client
    {
        public static HttpClient HttpClient { get; set; }
        public static AuthContext Auth { get; set; }
        public static string Host { get; set; }
        public static string AccountKey { get; set; }

        public static bool IsTesting { get; set; }
        public static int TestRequestIncremental { get; set; }

        private Client() {}

        public static void Init(string tokenKey, string tokenSecret, string accountKey, string host = null, HttpClient httpClient = null)
        {
            AuthContext auth = new AuthContext();
            auth.TokenKey = tokenKey;
            auth.TokenSecret = tokenSecret;

            if (host == null)
            {
                host = "https://api.omnigage.io/api/v1/";
            }

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            Auth = auth;
            AccountKey = accountKey;
            Host = host;
            HttpClient = httpClient;
            IsTesting = false;
            TestRequestIncremental = 0;
        }
    }
}

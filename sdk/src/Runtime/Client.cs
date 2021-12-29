using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Omnigage.Runtime
{
    public class Client
    {
        public static HttpClient HttpClient { get; set; }
        public static AuthContext Auth { get; set; }
        public static string Host { get; set; }

        public static bool IsTesting { get; set; }
        public static int TestRequestIncremental { get; set; }

        /// <summary>
        /// Create a GET request to the Omnigage API and return an object for retrieving tokens
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>JObject</returns>
        public static async Task<string> GetRequest(string uri)
        {
            return await SendClientRequest("GET", uri);
        }

        /// <summary>
        /// Create a POST request to the Omnigage API and return an object for retrieving tokens
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns>JObject</returns>
        public static async Task<string> PostRequest(string uri, string content)
        {
            return await SendClientRequest("POST", uri, content);
        }


        /// <summary>
        /// Create a PATCH request to the Omnigage API and return an object for retrieving tokens
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns>JObject</returns>
        public static async Task<string> PatchRequest(string uri, string content)
        {
            return await SendClientRequest("PATCH", uri, content);
        }

        /// <summary>
        /// Create a bulk request to the Omnigage API and return an object
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns>IRestResponse</returns>
        public static async Task<string> PostBulkRequest(string uri, string content)
        {
            return await SendClientRequest("POST", uri, content, "application/vnd.api+json;ext=bulk");
        }

        /// <summary>
        /// Send a request using the Omnigage client.
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> SendClientRequest(string httpMethod, string uri, string content = null, string contentType = null)
        {
            var request = RequestHandler(httpMethod, uri, content, contentType);

            HttpResponseMessage response = await HttpClient.SendAsync(request);

            return await ResponseHandler(response);
        }

        /// <summary>
        /// Handler for formulating HttpRequestMessage
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected static HttpRequestMessage RequestHandler(string httpMethod, string uri, string content = null, string contentType = null)
        {
            StringContent payload = null;

            if (content != null)
            {
                payload = new StringContent(content, Encoding.UTF8, "application/json");
            }


            if (IsTesting)
            {
                TestRequestIncremental++;
                uri += $"?test_request_number={TestRequestIncremental}";
            }

            var method = new HttpMethod(httpMethod);
            var request = new HttpRequestMessage(method, Host + uri)
            {
                Content = payload
            };

            request.Headers.Add("Authorization", Auth.Authorization);

            if (contentType != null)
            {
                request.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);
            }

            return request;
        }

        /// <summary>
        /// Response handler for throwing exceptions and converting response to a string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected static async Task<string> ResponseHandler(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 400:
                    throw new ValidationException();
                case 401:
                    throw new AuthException();
            }

            return await response.Content.ReadAsStringAsync();
        }
    }

    /// <summary>
    /// Validation exception
    /// </summary>
    public class ValidationException : Exception { }

    /// <summary>
    /// Auth exception
    /// </summary>
    public class AuthException : Exception { }
}

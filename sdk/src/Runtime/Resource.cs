using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonApiSerializer;
using AutoMapper;

namespace Omnigage.Runtime
{
    /// <summary>
    /// Adapter for faciliating serializing model instances and making requests.
    /// </summary>
    abstract public class Adapter
    {
        public string Id { get; set; }
        public abstract string Type { get; }

        public override string ToString()
        {
            return this.Serialize();
        }

        /// <summary>
        /// Serialize the current model instance.
        /// </summary>
        /// <returns>string</returns>
        public virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this, new JsonApiSerializerSettings());
        }

        /// <summary>
        /// Helper method for creating a new instance.
        /// </summary>
        public virtual async Task Create()
        {
            string payload = this.Serialize();
            string response = await PostRequest(this.Type, payload);
            this.LoadResponse(response);
        }

        /// <summary>
        /// Helper method for updating an instance.
        /// </summary>
        public virtual async Task Update()
        {
            string payload = this.Serialize();
            string response = await PatchRequest($"{this.Type}/{this.Id}", payload);
            this.LoadResponse(response);
        }

        /// <summary>
        /// Helper method for reloading a single instance.
        /// </summary>
        public virtual async Task Reload()
        {
            await this.Find(this.Id);
        }

        /// <summary>
        /// Helper method for requesting a single instance.
        /// </summary>
        public virtual async Task Find(string id)
        {
            string response = await GetRequest($"{this.Type}/{id}");
            Type type = this.GetType();

            object instance = JsonConvert.DeserializeObject(response, type, new JsonApiSerializerSettings());

            this.CopyProperties(instance);
        }

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
            StringContent payload = null;

            if (content != null)
            {
                payload = new StringContent(content, Encoding.UTF8, "application/json");
            }


            if (Client.IsTesting)
            {
                Client.TestRequestIncremental++;
                uri += $"?test_request_number={Client.TestRequestIncremental}";
            }

            var method = new HttpMethod(httpMethod);
            var request = new HttpRequestMessage(method, Client.Host + uri)
            {
                Content = payload
            };

            request.Headers.Add("Authorization", "Basic " + Client.Auth.Authorization);

            if (contentType != null)
            {
                request.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);
            }

            HttpResponseMessage response = await Client.HttpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Convert raw JSON response to loaded properties on instance.
        /// </summary>
        /// <param name="response"></param>
        protected void LoadResponse(string response)
        {
            Type type = this.GetType();

            object instance = JsonConvert.DeserializeObject(response, type, new JsonApiSerializerSettings());

            this.CopyProperties(instance);
        }

        /// <summary>
        /// Copy source properties to current instance.
        /// </summary>
        /// <param name="source"></param>
        protected void CopyProperties(object source)
        {
            Type type = this.GetType();

            MapperConfiguration _configuration = new MapperConfiguration(cnf =>
            {
                cnf.CreateMap(type, type).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            });
            var mapper = new Mapper(_configuration);
            mapper.Map(source, this);
        }
    }
}
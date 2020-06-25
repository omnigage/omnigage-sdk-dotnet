using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using JsonApiSerializer;
using AutoMapper;
using Omnigage.Auth;

namespace Omnigage.Util
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
        /// <param name="client"></param>
        public virtual async Task Create(HttpClient client)
        {
            string payload = this.Serialize();
            string response = await PostRequest(client, this.Type, payload);
            Type type = this.GetType();

            object instance = JsonConvert.DeserializeObject(response, type, new JsonApiSerializerSettings());

            this.CopyProperties(instance);
        }

        /// <summary>
        /// Helper method for updating an instance.
        /// </summary>
        /// <param name="client"></param>
        public virtual async Task Update(HttpClient client)
        {
            string payload = this.Serialize();
            string response = await PatchRequest(client, $"{this.Type}/{this.Id}", payload);
            Type type = this.GetType();

            object instance = JsonConvert.DeserializeObject(response, type, new JsonApiSerializerSettings());

            this.CopyProperties(instance);
        }

        /// <summary>
        /// Create a POST request to the Omnigage API and return an object for retrieving tokens
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns>JObject</returns>
        public static async Task<string> PostRequest(HttpClient client, string uri, string content)
        {
            StringContent payload = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage request = await client.PostAsync(uri, payload);
            return await request.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// Create a PATCH request to the Omnigage API and return an object for retrieving tokens
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns>JObject</returns>
        public static async Task<string> PatchRequest(HttpClient client, string uri, string content)
        {
            StringContent payload = new StringContent(content, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, uri)
            {
                Content = payload
            };

            HttpResponseMessage response = await client.SendAsync(request);

            return await request.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Create a bulk request to the Omnigage API and return an object
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="uri"></param>
        /// <param name="payload"></param>
        /// <returns>IRestResponse</returns>
        public static IRestResponse PostBulkRequest(AuthContext auth, string uri, string payload)
        {
            string bulkRequestHeader = "application/vnd.api+json;ext=bulk";
            var bulkClient = new RestClient(auth.Host + uri);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", bulkRequestHeader);
            request.AddHeader("Content-Type", bulkRequestHeader);
            request.AddHeader("X-Account-Key", auth.AccountKey);
            request.AddHeader("Authorization", "Basic " + auth.Authorization);
            request.AddParameter(bulkRequestHeader, payload, ParameterType.RequestBody);
            return bulkClient.Execute(request);
        }

        /// <summary>
        /// Copy source properties to current instance.
        /// </summary>
        /// <param name="source"></param>
        public void CopyProperties(object source)
        {
            Type type = this.GetType();

            MapperConfiguration _configuration = new MapperConfiguration(cnf =>
            {
                cnf.CreateMap(type, type).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            });
            var mapper = new Mapper(_configuration);
            mapper.DefaultContext.Mapper.Map(source, this);
        }
    }
}
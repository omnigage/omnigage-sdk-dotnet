using System;
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
            string response = await Client.PostRequest(this.Type, payload);
            this.LoadResponse(response);
        }

        /// <summary>
        /// Helper method for updating an instance.
        /// </summary>
        public virtual async Task Update()
        {
            string payload = this.Serialize();
            string response = await Client.PatchRequest($"{this.Type}/{this.Id}", payload);
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
            string response = await Client.GetRequest($"{this.Type}/{id}");
            Type type = this.GetType();

            object instance = JsonConvert.DeserializeObject(response, type, new JsonApiSerializerSettings());

            this.CopyProperties(instance);
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
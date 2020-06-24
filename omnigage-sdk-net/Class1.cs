using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using JsonApiSerializer;
using AutoMapper;
using HeyRed.Mime;

namespace omnigage
{
    /// <summary>
    /// Authentication and request context
    /// </summary>
    public class AuthContext
    {
        public string TokenKey { get; set; }
        public string TokenSecret { get; set; }
        public string Host { get; set; }
        public string AccountKey { get; set; }

        /// <summary>
        /// Create Authorization token following RFC 2617 
        /// </summary>
        /// <returns>Base64 encoded string</returns>
        public string Authorization
        {
            get
            {
                byte[] authBytes = System.Text.Encoding.UTF8.GetBytes($"{this.TokenKey}:{this.TokenSecret}");
                return System.Convert.ToBase64String(authBytes);
            }
        }
    }

    /// <summary>
    /// Resource: `/voice-templates` - https://omnigage.docs.apiary.io/#reference/call-resources/voice-template
    /// </summary>
    public class VoiceTemplateModel : Adapter
    {
        public override string Type { get; } = "voice-templates";

        public string Name { get; set; }

        public string Kind { get; set; }

        public UploadModel Upload { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }
    }

    /// <summary>
    /// Resource `/engagements` - https://omnigage.docs.apiary.io/#reference/engagement-resources
    /// </summary>
    public class EngagementModel : Adapter
    {
        public override string Type { get; } = "engagements";

        public string Name;

        public string Direction;

        public string Status;
    }

    /// <summary>
    /// Resource: `/activities` - https://omnigage.docs.apiary.io/#reference/engagement-resources/activity-collection
    /// </summary>
    public class ActivityModel : Adapter
    {
        public override string Type { get; } = "activities";

        public string Name;

        public string Kind;

        public EngagementModel Engagement;

        [JsonProperty(propertyName: "caller-id")]
        public CallerIdModel CallerId;
    }

    /// <summary>
    /// Resource: `/triggers` - https://omnigage.docs.apiary.io/#reference/engagement-resources/trigger-collection
    /// </summary>
    public class TriggerModel : Adapter
    {
        public override string Type { get; } = "triggers";

        public string Kind;

        [JsonProperty(propertyName: "on-event")]
        public string OnEvent;

        [JsonProperty(propertyName: "voice-template")]
        public VoiceTemplateModel VoiceTemplate;

        public ActivityModel Activity;
    }

    /// <summary>
    /// Resource: `/envelopes` - https://omnigage.docs.apiary.io/#reference/engagement-resources/envelope-collection
    /// </summary>
    public class EnvelopeModel : Adapter
    {
        public override string Type { get; } = "envelopes";

        [JsonProperty(propertyName: "phone-number")]
        public string PhoneNumber;

        [JsonProperty(propertyName: "meta_prop")]
        public Dictionary<string, string> Meta;

        public EngagementModel Engagement;

        public static string SerializeBulk(List<EnvelopeModel> records)
        {
            string payload = JsonConvert.SerializeObject(records, new JsonApiSerializerSettings());
            // Work around `JsonApiSerializer` moving properties named "meta" above "attributes"
            return payload.Replace("meta_prop", "meta");
        }
    }

    /// <summary>
    /// Resource: `/uploads` - https://omnigage.docs.apiary.io/#reference/media-resources/upload
    /// </summary>
    public class UploadModel : Adapter
    {
        public override string Type { get; } = "uploads";

        [JsonProperty(propertyName: "request-url")]
        public string RequestUrl;

        [JsonProperty(propertyName: "request-method")]
        public string RequestMethod;

        [JsonProperty(propertyName: "request-headers")]
        public List<object> RequestHeaders;

        [JsonProperty(propertyName: "request-form-data")]
        public List<object> RequestFormData;

        [JsonIgnore]
        public string FileName { get; set; }

        [JsonIgnore]
        public string MimeType { get; set; }

        [JsonIgnore]
        public long FileSize { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

        public override string Serialize()
        {
            return @"{
                'name': '" + this.FileName + @"',
                'type': '" + this.MimeType + @"',
                'size': " + this.FileSize + @"
            }";
        }

        public override async Task Create(HttpClient client)
        {
            this.FileName = Path.GetFileName(this.FilePath);
            this.FileSize = new System.IO.FileInfo(this.FilePath).Length;
            this.MimeType = MimeTypesMap.GetMimeType(this.FileName);

            await base.Create(client);

            using (var clientS3 = new HttpClient())
            {
                // Create multipart form including setting form data and file content
                MultipartFormDataContent form = await this.CreateMultipartForm();

                // Upload to S3
                await this.PostS3Request(clientS3, form);
            };
        }

        /// <summary>
        /// For debugging.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonApiSerializerSettings());
        }

        /// <summary>
        /// Create a multipart form using form data from the Omnigage `upload` instance along with the specified file path.
        /// </summary>
        /// <param name="uploadInstance"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="mimeType"></param>
        /// <returns>A multipart form</returns>
        public async Task<MultipartFormDataContent> CreateMultipartForm()
        {
            MultipartFormDataContent form = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));

            foreach (JObject formData in this.RequestFormData)
            {
                foreach (KeyValuePair<string, JToken> prop in formData)
                {
                    form.Add(new StringContent((string)prop.Value), prop.Key);
                }
            }

            // Set the content type(required by presigned URL)
            form.Add(new StringContent(this.MimeType), "Content-Type");

            byte[] fileBytes;
            using (FileStream stream = File.Open(this.FilePath, FileMode.Open))
            {
                fileBytes = new byte[stream.Length];
                await stream.ReadAsync(fileBytes, 0, (int)stream.Length);
            }

            // Add file content to form
            ByteArrayContent fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(fileContent, "file", this.FileName);

            return form;
        }

        /// <summary>
        /// Make a POST request to S3 using presigned headers and multipart form
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uploadInstance"></param>
        /// <param name="form"></param>
        /// <param name="url"></param>
        async Task PostS3Request(HttpClient client, MultipartFormDataContent form)
        {
            // Set each of the `upload` instance headers
            foreach (JObject header in this.RequestHeaders)
            {
                foreach (KeyValuePair<string, JToken> prop in header)
                {
                    client.DefaultRequestHeaders.Add(prop.Key, (string)prop.Value);
                }
            }

            // Make S3 request
            HttpResponseMessage responseS3 = await client.PostAsync(this.RequestUrl, form);
            string responseContent = await responseS3.Content.ReadAsStringAsync();

            if ((int)responseS3.StatusCode == 204)
            {
                Console.WriteLine("Successfully uploaded file.");
            }
            else
            {
                Console.WriteLine(responseS3);
                throw new S3UploadFailed();
            }
        }
    }

    /// <summary>
    /// Resource: `/caller-ids` - https://omnigage.docs.apiary.io/#reference/identity-resources/caller-id-collection
    /// </summary>
    public class CallerIdModel : Adapter
    {
        public override string Type { get; } = "caller-ids";
    }

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

    /// <summary>
    /// S3 Upload Failed exception
    /// </summary>
    public class S3UploadFailed : Exception { }
}
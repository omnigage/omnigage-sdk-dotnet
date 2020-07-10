using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JsonApiSerializer;
using HeyRed.Mime;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/uploads` - https://omnigage.docs.apiary.io/#reference/media-resources/upload
    /// </summary>
    public class UploadResource : Adapter
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

        public override async Task Create()
        {
            this.FileName = Path.GetFileName(this.FilePath);
            this.FileSize = new FileInfo(this.FilePath).Length;
            this.MimeType = MimeTypesMap.GetMimeType(this.FileName);

            await base.Create();

            // Create multipart form including setting form data and file content
            MultipartFormDataContent form = await this.CreateMultipartForm();

            // Upload to S3
            await this.PostS3Request(form);
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
        async Task PostS3Request(MultipartFormDataContent form)
        {
            var method = new HttpMethod("POST");
            var request = new HttpRequestMessage(method, this.RequestUrl)
            {
                Content = form
            };

            // Set each of the `upload` instance headers
            foreach (JObject header in this.RequestHeaders)
            {
                foreach (KeyValuePair<string, JToken> prop in header)
                {
                    request.Headers.Add(prop.Key, (string)prop.Value);
                }
            }

            // Make S3 request
            HttpResponseMessage responseS3 = await Client.HttpClient.SendAsync(request);
            // string responseContent = await responseS3.Content.ReadAsStringAsync();

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
    /// S3 Upload Failed exception
    /// </summary>
    public class S3UploadFailed : Exception { }
}

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CSCAssignment.Task5.Controller
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("api/t5/upload")]
        public async Task<HttpResponseMessage> PostFormData()
        {

            string aws_id = "ASIA52Z7VJWQHGQQNROR";
            string aws_key = "7/ieFvcWxGfufsxya/u9gG42dKuHVUbveBKx834+";
            string aws_token = "FwoGZXIvYXdzEHYaDGKcV1S7RaeJ3sLI3yLJAaz958N/D81ERhRl3Q7X1xmHKCsPqrM1HImZfoFTGhsow7BW5Hmg5eEtZGvCw8/IlNGj0gndJTS48U25RzO2VBASHws6wTHJNMDr/Mhs2keQxuhawjBPCdw8nJqBFUKY4EB2xemKIe0sTq0/4BJjigUHdQe7a/7HKxamu6BynrbArI4c7iNOlO1cJ+tbjupcTrqJfkrdXEMQlBnqrjLv4g0niyvpPuB8oWfPoKPMC1PSNlvUwytg/018Kw+kh9//qhDvdsRcXA79gCiEwp7/BTItKdAAsnjNJLpe3x8nkzbz8hS4839Cft3i5pzL3e6PNnRnIN3hJT7a9F8518uB";
            SessionAWSCredentials aws_cred = new  SessionAWSCredentials(aws_id, aws_key, aws_token);
            string bitly_token = "796c69a2ff3e69e196b1ea4a7cb4b5bd29bb3093";

            RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var formDatas = new List<byte[]>();

                // Scan the Multiple Parts 
                foreach (HttpContent contentPart in provider.Contents)
                {
                    formDatas.Add(await contentPart.ReadAsByteArrayAsync());
                }

                byte[] photoByte = formDatas[0];

                MemoryStream photoStream = new MemoryStream();
                photoStream.Write(photoByte, 0, photoByte.Length);

                string S3PhotoKey = Guid.NewGuid().ToString();
                string S3BucketName = "task5photobucket";

                IAmazonS3 s3Client = new AmazonS3Client(aws_cred, bucketRegion);

                TransferUtility fileTransferUtility = new TransferUtility(s3Client);

                fileTransferUtility.Upload(photoStream, S3BucketName, S3PhotoKey);

                string itemUrl = String.Format("https://{0}.s3.{1}.amazonaws.com/{2}", S3BucketName, bucketRegion.SystemName, S3PhotoKey);

                UriBuilder url = new UriBuilder();
                url.Scheme = "https";

                url.Host = "api-ssl.bitly.com";
                url.Path = "v4/shorten";

                JsonDocument wsResponse = BitlyMakeRequest(url.ToString(), bitly_token, itemUrl);
                JsonElement json = wsResponse.RootElement;

                string shortenedUrl = json.GetProperty("link").GetString();

                return Request.CreateResponse(HttpStatusCode.OK, new { url = shortenedUrl });
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        public static JsonDocument BitlyMakeRequest(string requestUrl, string token, string itemUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + token);
                
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var data = new { long_url = itemUrl};
                    string jsonStr = JsonSerializer.Serialize(data);

                    streamWriter.Write(jsonStr);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                JsonDocument jsonDoc = JsonDocument.Parse(response.GetResponseStream());
                return jsonDoc;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

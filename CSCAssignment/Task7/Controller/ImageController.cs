using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSCAssignment.Task7.Controller
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("api/t7/upload")]
        public async Task<HttpResponseMessage> PostFormData()
        {

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



                /*
                                UriBuilder url = new UriBuilder();
                                url.Scheme = "https";

                                url.Host = "api.clarifai.com";
                                url.Path = "v4/shorten";
                */


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }


        }
    }
}

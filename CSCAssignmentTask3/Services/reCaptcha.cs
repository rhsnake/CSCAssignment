using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace CSCAssignmentTask3.Services
{
    public class reCaptcha
    {
        public bool Verify(string captchaToken)
        {
            string captchaSecret = "";
            string verifyUrl = "https://www.google.com/recaptcha/api/siteverify";
            try
            {
                HttpWebRequest request = WebRequest.Create(verifyUrl) as HttpWebRequest;
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                string postData = string.Format("secret={0}&response={1}", captchaSecret, captchaToken);
                byte[] buffer = Encoding.Default.GetBytes(postData);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                JsonDocument jsonDoc = JsonDocument.Parse(response.GetResponseStream());
                JsonElement json = jsonDoc.RootElement;
                double score = json.GetProperty("score").GetDouble();
                if (score > 0.5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
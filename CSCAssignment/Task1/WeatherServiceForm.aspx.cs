using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSCAssignment.Task1
{
    public partial class WeatherServiceForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UriBuilder url = new UriBuilder();
            url.Scheme = "https";

            url.Host = "api.data.gov.sg";
            url.Path = "v1/environment/air-temperature";

            //Make a HTTP request to the NEA Weather service
            JsonDocument wsResponse = MakeRequest(url.ToString());
            Response.ContentType = "text/html";
            if (wsResponse != null)
            {
                JsonElement json = wsResponse.RootElement;
                CSharpJSONResponse.InnerText = (json.ToString());

            }
            else
            {
                CSharpJSONResponse.InnerText = ("Failed to fetch data using C#");
            }
        }

        public static JsonDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                JsonDocument jsonDoc = JsonDocument.Parse(response.GetResponseStream());
                return jsonDoc;
            }
            catch(Exception)
            {
                return null;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// HttpClient wrapper lols
// For parsing json data
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpRequests
{
    public class MicaRequests
    {
        public static JObject? PostData(string Url, Object PayloadObject, Dictionary<string, string>? HeadersObject=null)
        { // Function to post data 2 server and parse the response 
            // Mica >> Xylo
            using (HttpClient client = new HttpClient())
            {
                string        JsonContent    =     JsonConvert.SerializeObject(PayloadObject);
                StringContent stringContent  = new StringContent(JsonContent, Encoding.UTF8, "application/json");

                if (HeadersObject != null)
                { // add headers to the request if added in inputs
                    foreach (KeyValuePair<string, string> entry in HeadersObject)
                        client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
                // exec the request 
                HttpResponseMessage response = client.PostAsync(Url, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    return JObject.Parse(response.Content.ReadAsStringAsync().Result);
                }
                else
                    return null;
            }
        }
        public static bool Ping(string Uri)
        { // Function to ping endp & ret a bool
            using (HttpClient client = new HttpClient())
                return client.GetAsync(Uri).Result.IsSuccessStatusCode;
        }
    }
}

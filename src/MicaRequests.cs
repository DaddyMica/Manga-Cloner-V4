// Function needed to make the plan work
using System;
using System.Text;
// HttpClient wrapper lols
// For parsing json data
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpRequests
{
    public class MicaRequests
    { // 1 method is all i need lols
        public static JObject? PostData(string Url, Object PayloadObject, Dictionary<string, string>? HeadersObject=null)
        { // Function to post data 2 server and parse the response 
            // Mica >> Xylo
            using (HttpClient client = new HttpClient())
            {
                // add the headers to the request object
                if (HeadersObject != null)
                    foreach (KeyValuePair<string, string> entry in HeadersObject)
                        client.DefaultRequestHeaders.Add(entry.Key, entry.Value);

                // exec the request 
                HttpResponseMessage response = client.PostAsync(Url, new StringContent(JsonConvert.SerializeObject(PayloadObject), Encoding.UTF8, "application/json")).Result;

                if (response.IsSuccessStatusCode)
                    return JObject.Parse(response.Content.ReadAsStringAsync().Result);
                else
                    return null;
            }
        }
    }
}

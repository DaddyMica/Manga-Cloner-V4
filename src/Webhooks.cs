using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HttpRequests;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MicaWebhooks
{
    public class Webhooks
    {
        public string? WebhookUri;
        public string? UserName;
        public string? Content;
        public string? AvatarUrl;
        public Dictionary<string, string>? Payload;

        public Webhooks(string WebhookUri, string? UserName=null, string? Content=null, string? AvatarUrl=null)
        { // Constructor for webhooks class lol
            Dictionary<string, string> Payload = new Dictionary<string, string>();
            // ping uri
            if (MicaRequests.Ping(WebhookUri))
                this.WebhookUri = WebhookUri;

            if (UserName != null)
                Payload.Add("username", UserName);

            if (Content != null)
                Payload.Add("content", Content);

            if (AvatarUrl != null)
                Payload.Add("avatar_url", AvatarUrl);
        }

        public JObject? ExecuteWebhook(string? Content=null, string? UserName=null, string? AvatarUrl=null)
        { // Function to execute webhook lols
            if (Content != null)
                if (Payload.ContainsKey("content"))
                    Payload["content"] = Content;
            
            if (UserName != null)
                if (Payload.ContainsKey("username"))
                    Payload["username"] = UserName;

            if (AvatarUrl != null)
                if (Payload.ContainsKey("avatar_url"))
                    Payload["avatar_url"] = AvatarUrl;

            using (HttpClient client = new HttpClient())
            {
                string        jsonString     =     JsonConvert.SerializeObject(Payload);
                StringContent stringContent  = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(WebhookUri, stringContent).Result;

                if (response.IsSuccessStatusCode)
                    return JObject.Parse(response.Content.ReadAsStringAsync().Result);
                else
                    return null;
            }
        }
    }
}

// Webhooks module i had to write to fix dis bitch lols
// modeled after discord-webhook
using System;
using System.Text;

// Parsing Json Content
using Newtonsoft.Json;

namespace MangaClonerV4
{
    public class Webhooks
    {
        public string? WebhookUri;
        public Dictionary<string, string>? Payload;

        private static bool Ping(string uri, Dictionary<string, string>? HeadersObject = null, Object? AnonHeadersObj = null)
        { // Static method to validate uri
            using (HttpClient client = new HttpClient())
            { // Add headers to request if inputted
                if (HeadersObject != null)
                    foreach (KeyValuePair<string, string> entry in HeadersObject)
                        client.DefaultRequestHeaders.Add(entry.Key, entry.Value);

                return client.GetAsync(uri).Result.IsSuccessStatusCode;
            }
        }
        public Webhooks(string Webhookuri, string? Content = null, string? UserName = null)
        { // Constructor for webhooks class
            if (Ping(Webhookuri))
            { // ya digg?
                Payload = new Dictionary<string, string>();
                this.WebhookUri = Webhookuri;

                if (Content != null)
                    Payload.Add("content", Content);

                if (UserName != null)
                    Payload.Add("username", UserName);
            }
        }

        public static Dictionary<string, string> OverwriteHeadersObject(Dictionary<string, string> PayloadObject, string? Content=null, string? UserName=null)
        {
            if (Content == null && UserName == null)
                throw new Exception("supply more args!!!!");

            if (Content != null)
                PayloadObject["content"] = Content;
            
            if (UserName != null)
                PayloadObject["username"] = UserName;

            return PayloadObject;
        }

        public bool Execute(string? Content = null, string? UserName = null)
        { // If inputted, overwrite constructor inps
            // designed after the discord_webhook py module
            if (Payload != null)
            {
                Dictionary<string, string> PayloadObject = OverwriteHeadersObject(Payload, Content, UserName);

                using (HttpClient client = new HttpClient())
                {
                    dynamic? initialResponse = client.PostAsync(WebhookUri, new StringContent(JsonConvert.SerializeObject(PayloadObject), Encoding.UTF8, "application/json")).Result;

                    if (initialResponse != null)
                        return initialResponse.IsSuccessStatusCode;

                    return false;
                }
            } else { return false; }
        }
    }
}

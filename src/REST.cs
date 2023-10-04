// REST class for discord http reqs
using System;
using System.Text;

// custom req lib
using HttpRequests;

namespace REST_API
{
    public class REST
    {
        public string? Token;
        public string? Guild;

        public static bool CheckToken(string Token)
        { // Method to check bot token for validity
            using (HttpClient client = new HttpClient())
            { // add headers
                client.DefaultRequestHeaders.Add("Authorization", $"Bot {Token}");
                // exec req & ret
                return client.GetAsync("https://discord.com/api/v9/users/@me").Result.IsSuccessStatusCode;
            }
        }
        public REST(string AuthToken, string Guild)
        { // REST class for manga cloner V4!!
            if (CheckToken(AuthToken) && (Guild != null))
            {
                Token      = AuthToken;
                this.Guild = Guild;
            }
        }

        public Dictionary<string, string> Headers
        { // Property for headers object
            get
            {
                return new Dictionary<string, string> { {"Authorization", $"Bot {Token}"} };
            }
        }

        public string CreateChannel(string ChannelName)
        { // Function to create new channel and parse response
            dynamic? Response = MicaRequests.PostData($"https://discord.com/api/v9/guilds/{Guild}/channels", new { name = ChannelName }, Headers);

            if (Response != null)
            {
                return (string)Response.id;
            }
            else
                return string.Empty;
        }

        public string CreateWebhook(string Channel, string WebhookName)
        { // Function to create new webhook and return new url
            dynamic? Response = MicaRequests.PostData($"https://discord.com/api/v9/channels/{Channel}/webhooks", new { name = WebhookName }, Headers);

            if (Response != null)
            {
                return $"https://discord.com/api/webhooks/{Response.id}/{Response.token}";
            }
            else
                return string.Empty;
        }

    }
}

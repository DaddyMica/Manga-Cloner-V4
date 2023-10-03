// MangaSee123Api-Client In C#
// Ycl on top type shi
using System;
using System.Text;
using System.Threading.Tasks;
// HttpReq class i made
using HttpRequests;
// json parsing
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MangaSee123
{
    public class MangaSee123Client
    {
        public string? Tunnel;
        public string? Authkey;
        public string? ApiKey;
        public string? _uuid;
        public JObject InitialHandshakePayload;

        public MangaSee123Client(string Tunnel, string Authkey, string? ApiKey=null, string? uuid=null)
        { // Official MangaSee123Api-Client In CSharp!
            // Made by Mica ofc
            // Check the tunnel 2 b if its active
            if (MicaRequests.Ping($"{Tunnel}/api"))
            { // so if we're in an active session
                if (ApiKey == null && uuid == null)
                { // we have to create an api key
                    dynamic? json = MicaRequests.PostData($"{Tunnel}/api/gateway/authorize", new { auth_key = Authkey, client = "cs0.0.1", host = "localhost", os = "win" });

                    if (json != null)
                    {
                        InitialHandshakePayload = json;
                        this.Tunnel  = Tunnel;
                        this.Authkey = Authkey;
                        this.ApiKey  = json.api_key;
                        this._uuid   = json.uuid;
                    }
                }
            }
        }

        public Object JsonPayload
        { // headers property
            get
            {
                return new { api_key = ApiKey, uuid = _uuid};
            }
        }
        public string GetPage(string title, string chapter, string page)
        { // Function to get a page of manga back with a request
            // uses manga see api
            // I made the api too lols
            // get on ma level btch
            dynamic? json = MicaRequests.PostData($"{Tunnel}/api/mangas/{title}/chapters/{chapter}/pages/{page}", JsonPayload);

            if (json != null && (json.tags) == 0)
                return (string)json.url;
            else
                return string.Empty;
        }

        public List<string>? GetChapter(string title, string chapter)
        {
            int          page = 1;
            List<string> urls = new List<string> { };

            while (true)
            {   // get newpage object
                string NewPage = GetPage(title, chapter, page.ToString());
                // check it
                if (NewPage != string.Empty)
                {
                    Console.WriteLine($"[*] New Page: {NewPage} [*]");
                    // add to list
                    urls.Add(NewPage);
                    page++;
                }
                
                else if (NewPage == string.Empty)
                    break;
            }

            if ((urls.Count) > 0)
                return urls;
            else
                return null;
        }
    }
}

using System;

// Discord wrapper in C#
using DSharpPlus; 
// custom libs
using MangaSee123;
using REST_API;
using MicaWebhooks;

public class Program
{  // TODO: write cloner class when well rested
    public static DiscordClient CreateNewClient(string Token)
    {
        return new DiscordClient(new DiscordConfiguration()
        {
            Token = Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });
    }

    public static void CloneManga(string[] argv)
    { // Function to clone mangas
        // Made by mica
        // argv[0] = TUNNEL; argv[1] = AUTHKEY; argv[2] = TOKEN; argv[3] = GUILD; argv[4] = TITLE
        Console.WriteLine($"[*] Cloning: {argv[4]} [*]");
        // new shit
        MangaSee123Client MangaSeeClient = new MangaSee123Client(argv[0], argv[1]);
        REST              rest           = new REST(argv[2], argv[3]);
        int               chapter        = 1;

        while (true)
        {
            List<string>? NewChapter = MangaSeeClient.GetChapter(argv[4], chapter.ToString());

            if (NewChapter != null)
            { // make channel for new chapter n shit
                string NewChannel = rest.CreateChannel($"{argv[4]}-{chapter}");

                if (NewChannel != null)
                {
                    string NewWebhook = rest.CreateWebhook(NewChannel, $"{argv[4]}-{chapter}");

                    Console.WriteLine($"[*] Created New Channel & Webhook: {NewWebhook} [*]");
                    Console.WriteLine($"[!] Sending Chapter [!]");

                    for (int i = 0; i < NewChapter.Count; i++)
                    {
                        new Webhooks(NewWebhook).ExecuteWebhook(NewChapter[i], $"{argv[4]}-{chapter}-{i + 1}");
                        Thread.Sleep(3000);
                    }
                    chapter++;
                }
            }
            else
                break;
        }
    }
    public static async Task Main(string[] args)
    {
        var discord = CreateNewClient(args[2]);

        discord.MessageCreated += async (s, e) =>
        {
            if (e.Message.Content.ToLower().StartsWith("ping"))
                await e.Message.RespondAsync("pong!");
        };

        discord.SocketOpened += async (s, e) =>
        {
            CloneManga(args);
        };

        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}

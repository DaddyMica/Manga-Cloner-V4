// FREE SEX!
using System;

// Discord wrapper in C#
using DSharpPlus; 
// custom libs
using MangaSee123;
using REST_API;
using MicaWebhooks;

public class MicaApp
{  // Class that clones manga in discord servers
    // Made fully by mica
    // the csharpist god
    public static DiscordClient CreateNewClient(string Token)
    { // Function to create a new discord client w intents
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
        int               chapter        = 1; // start at ch 1 ofc

        while (true)
        {
            List<string>? NewChapter = MangaSeeClient.GetChapter(argv[4], chapter.ToString());

            if (NewChapter != null)
            { // make channel for new chapter n shit
                string NewChannel = rest.CreateChannel($"{argv[4]}-{chapter}");

                if (NewChannel != null)
                { // Create new webhook upon channel creation
                    string NewWebhook = rest.CreateWebhook(NewChannel, $"{argv[4]}-{chapter}");

                    Console.WriteLine($"[*] Created New Channel & Webhook: {NewWebhook} [*]");
                    Console.WriteLine($"[!] Sending Chapter [!]");

                    for (int i = 0; i < NewChapter.Count; i++)
                    {
                        if (new Webhooks(NewWebhook).Execute(NewChapter[i], $"{argv[4]}-{chapter}-{i + 1}"))
                        {
                            Console.WriteLine($"[*] Sent: {NewChapter[i]} tohook! [*]");
                            Thread.Sleep(3000);
                        }
                    }
                    chapter++;
                }
            }
            else
                break;
        }
    }

    public static async Task Main(string[] args)
    { // Main Function!
        DiscordClient discord = CreateNewClient(args[2]);

        discord.MessageCreated += async (s, e) =>
        {
            if (e.Message.Content.ToLower().StartsWith("ping"))
                await e.Message.RespondAsync("pong!");
        };

        discord.SocketOpened += async (s, e) =>
        {
            try
            { // Clone it type shi     
                CloneManga(args);
                // catch exc
            } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                
        };

        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}

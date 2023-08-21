using ClassLibrary;
using ClassLibrary.Configuration;
using Config.Net;
using DSharpPlus;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // Check if config file exists
        if (!File.Exists("./config.ini"))
        {
            Console.WriteLine("Config file does not exist, creating....");
            ConfigWriter.WriteConfigFile();
            return;
        }
        IConfig config = new ConfigurationBuilder<IConfig>().UseIniFile("./config.ini").Build();

        var discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = config.BotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });
        
        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}
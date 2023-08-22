using ClassLibrary;
using ClassLibrary.Configuration;
using Config.Net;
using DSharpPlus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        var services = new ServiceCollection();
        ConfigureServices(services, config);    
    
        


        var discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = config.BotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });
        
        await discord.ConnectAsync();
        await Task.Delay(-1);
    }

    private static void ConfigureServices(IServiceCollection services, IConfig config)
    {
        string _connectionString =
            $@"Host={config.DatabaseHost};Username={config.DatabaseUser};Password={config.DatabasePassword};Database={config.DatabaseName}";

        services.AddDbContext<AuroraDBContext>(options => options.UseNpgsql(_connectionString));
        services.AddSingleton(config);
    }
}
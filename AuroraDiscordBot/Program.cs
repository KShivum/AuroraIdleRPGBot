using AuroraLibrary.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

class Program
{
    private static Config _config = null!;

    static void Main(string[] args)
    {
        string configFile;
        // Opens and reads config file
        if (File.Exists("Config.json"))
        {
            configFile = File.ReadAllText("Config.json");
            if (String.IsNullOrWhiteSpace(configFile))
            {
                string outputConfigString = JsonConvert.SerializeObject(new Config(), Formatting.Indented);
                File.WriteAllText("Config.json", outputConfigString);
                Console.WriteLine("Config File Created, Please input required Data");
                return;
            }
        }
        else
        {
            string outputConfigString = JsonConvert.SerializeObject(new Config(), Formatting.Indented);
            File.WriteAllText("Config.json", outputConfigString);
            Console.WriteLine("Config File Created, Please input required Data");
            return;
        }

        _config = JsonConvert.DeserializeObject<Config>(configFile)!;

        // This is so any new data that may not need to required will be written to the file
        string rewriteFile = JsonConvert.SerializeObject(_config, Formatting.Indented);
        File.WriteAllText("Config.json", rewriteFile);

        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);


        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetService<RPGBotDBContext>();

        var discord = new DiscordClient(new DiscordConfiguration
        {
            Token = _config.BotSettings.BotToken,
            TokenType = TokenType.Bot,
        });

        // Get prefix from config, if empty, default to !
        string prefix = _config.BotSettings.Prefix;


        var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { prefix },
            Services = serviceProvider,
        });

        discord.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromSeconds(120),
            ResponseBehavior = DSharpPlus.Interactivity.Enums.InteractionResponseBehavior.Ack,
        });


        commands.RegisterCommands<AuroraDiscordBot.Commands.CreatePlayer>();
        commands.RegisterCommands<AuroraDiscordBot.Commands.CreateItem>();
        commands.RegisterCommands<AuroraDiscordBot.Commands.Inventory>();
        commands.RegisterCommands<AuroraDiscordBot.Commands.AdventureCommands>();
        if (_config.BotSettings.Debug)
            commands.RegisterCommands<AuroraDiscordBot.Commands.DebugHelper>();


        await discord.ConnectAsync();
        await Task.Delay(-1);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<RPGBotDBContext>(
            options => options.UseSqlServer(_config.ConnectionSettings.ToString()));
        services.AddSingleton(_config);
    }
}
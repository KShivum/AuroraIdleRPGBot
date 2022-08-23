using System.Configuration;
using AuroraLibrary;
using AuroraLibrary.ConfigManager;
using AuroraLibrary.DatabaseModels;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        ConfigManager.Initialize();


        //Checking to see if any required config values are missing
        bool allRequiredDoesNotExist = ConfigManager.AddToConfigAndError("ConnectionString");
        allRequiredDoesNotExist = allRequiredDoesNotExist | ConfigManager.AddToConfigAndError("BotToken");

        if(allRequiredDoesNotExist)
        {
            throw new System.Exception("One or more required config values are missing");
        }

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
            Token = ConfigManager.Config["BotToken"],
            TokenType = TokenType.Bot,
        });

        string prefix;
        if(ConfigManager.AddToConfigAndError("Prefix"))
        {
            prefix = "!";
        }
        else
        {
            prefix = ConfigManager.Config["Prefix"];
        }

        var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { prefix },
            Services = serviceProvider,
        });

        discord.UseInteractivity(new InteractivityConfiguration(){
            Timeout = TimeSpan.FromSeconds(120),
        });


        commands.RegisterCommands<AuroraDiscordBot.Commands.CreatePlayer>();
        await discord.ConnectAsync();
        await Task.Delay(-1);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<RPGBotDBContext>(options => options.UseSqlServer(ConfigManager.Config["ConnectionString"]));
    }


}
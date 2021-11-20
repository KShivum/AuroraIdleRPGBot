using System;
using System.Threading.Tasks;
using System.Configuration;
using DSharpPlus;
using Microsoft.Extensions.Logging;
using DSharpPlus.CommandsNext;
using RPGBotMain.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;

namespace RPGBotMain
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            string connectionString = $"Server={ConfigurationManager.AppSettings.Get("DBHost")};Database=RPGBot;User Id={ConfigurationManager.AppSettings.Get("DBBotUsername")};Password={ConfigurationManager.AppSettings.Get("DBBotPassword")};";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }

            DatabaseManager dbManager = new DatabaseManager(connection);

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                MinimumLogLevel = LogLevel.Debug,
                Token = ConfigurationManager.AppSettings.Get("BotToken"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            discord.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2),
            });

            var services = new ServiceCollection()
            .AddSingleton<SqlConnection>(connection)
            .BuildServiceProvider();

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" },
                Services = services
            });

            

            //Loading Commands
            commands.RegisterCommands<CreatePlayer>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}

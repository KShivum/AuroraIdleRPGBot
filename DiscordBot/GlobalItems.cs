using System.Configuration;
using DSharpPlus.Entities;

namespace DiscordBot
{
    public static class GlobalItems
    {
        //Add global commands to this using static DiscordBot.GloablItems
        public static string ConString = $"Server={ConfigurationManager.AppSettings.Get("Server")}; userid={ConfigurationManager.AppSettings.Get("SQLUsername")}; password={ConfigurationManager.AppSettings.Get("SQLPassword")}; database=rpgbot;SslMode=none;";
    
        #region Methods

        public static DiscordEmbedBuilder CreateEmbed(string title, string description, DiscordColor color)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color
            };
            return embed;
        }

        public static  DiscordEmbedBuilder CreateEmbed(string title, string description = "")
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
            };
            return embed;
        }

        #endregion
    
    }
}
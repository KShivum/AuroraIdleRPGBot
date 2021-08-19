using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using DiscordBot.Models;
using System.IO;
using System.Configuration;
using DiscordBot.Handlers.Dialogue;
using DiscordBot.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System.Threading.Tasks;
using System.Data;
using DiscordBot.Adventures.Dungeons;
using DiscordBot.Adventures;
using static DiscordBot.GlobalItems;
namespace DiscordBot.Commands
{
    class RPGCommands : BaseCommandModule
    {
        
        [Command("createitem")]
        [Description("Mainly a test command, will be deleted later")]
        public async Task CreateItem(CommandContext ctx)
        {
            var item = new Item();
            Random rdm = new Random();

            item.Owner = ctx.User.Id.ToString();
            item.ItemType = "Sword";
            item.Stat1 = rdm.Next(0, 10);
            item.Stat2 = rdm.Next(0, 10);
            item.Stat3 = rdm.Next(0, 10);
            item.Stat4 = rdm.Next(0, 10);
            item.Name = "Test";

            var success = item.AddItem(new MySqlConnection(ConString));

            if (success == false)
            {
                DiscordEmbedBuilder failedEmbed = new DiscordEmbedBuilder
                {
                    Title = "Failed To Add Item",
                    Color = DiscordColor.Red
                };

                var failed = await ctx.Channel.SendMessageAsync(embed: failedEmbed).ConfigureAwait(false);
                return;
            }

            var message = await ctx.Channel.SendMessageAsync("Added Item").ConfigureAwait(false);


        }

       
       



    }

    
}













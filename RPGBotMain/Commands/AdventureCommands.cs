using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RPGBotMain.Models;

namespace RPGBotMain.Commands
{
    public class AdventureCommands
    {
        public SqlConnection con { private get; set; }

        [Command("adventure")]
        public async Task CheckAdventure(CommandContext ctx)
        {
            //Check to see if the player exists
            if (CreatePlayer.CheckIfPlayerExists(ctx.User.Id))
            {
                var message = await ctx.RespondAsync("You don't have a character yet! Use `!start` to create one!");
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { message, ctx.Message });
                return;
            }

            //Check to see if the player is in a adventure already
            Adventure adventure = Adventure.GetPlayerActiveAdventure(ctx.User.Id, con);
            if (adventure != null)
            {
                TimeSpan ts = Adventure.GetAdventures().Find(x => x.Id == adventure.AdventureId).GetAdventureLength();
                if (DateTime.Now > adventure.StartTime + ts)
                {
                    //TODO:
                    //Complete adventure and calculate events, loot, and xp
                }
                else
                {
                    TimeSpan timeLeft = adventure.StartTime + ts - DateTime.Now;
                    var message = await ctx.RespondAsync($"You are currently in an adventure! You have {timeLeft.Hours} hours, {timeLeft.Minutes} minutes, and {timeLeft.Seconds} seconds left!");
                    Thread.Sleep(5000);
                    await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { message, ctx.Message });
                    return;
                }
            }

            //TODO: Start the player's adventure
            try
            {
                ctx.Client.ComponentInteractionCreated += async (s, e) =>
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            int selectedAdventure = 0;

            var adventures = Adventure.GetAdventures();

            while (true)
            {
                var embed = new DiscordEmbedBuilder();

                embed.WithTitle("Adventure");
                embed.WithColor(DiscordColor.Blue);

                embed.AddField($"Id: {adventures[selectedAdventure].Id}", $"{adventures[selectedAdventure].Name}");
                //TODO: Add buttons to message
            }




        }
    }
}

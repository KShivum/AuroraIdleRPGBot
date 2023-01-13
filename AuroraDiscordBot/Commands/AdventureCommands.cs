using AuroraLibrary.DatabaseModels;
using AuroraLibrary.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace AuroraDiscordBot.Commands;

public class AdventureCommands : BaseCommandModule
{
    
    public RPGBotDBContext db { private get; set; }

    List<AdventureBase> Adventures { get; } = new()
    {
        new NewbieForest()
    };

    [Command("adventure")]
    public async Task AdventureCommand(CommandContext ctx)
    {
        Player? player = db.Players.FirstOrDefault(x => x.Id == ctx.User.Id);
        new GlobalFunctions().CheckIfUserExists(ctx, player);

        if (player.StartedAdventure != null)
        {
            //This should never be null, if it is, then somewhere an id is being inputted incorrectly
            var activeAdventure = Adventures.FirstOrDefault(x => x.Id == player.AdventureId);
            //If the player is still in the adventure, then they can't start a new one
            if (player.StartedAdventure + activeAdventure.Duration * (1 / (float)((float)player.Speed / (float)activeAdventure.SpeedRecommended)) > DateTime.Now)
            {
                string responseString = "You are still on an adventure for ";
                var timeLeft = (player.StartedAdventure + activeAdventure.Duration * (1 / (float)(player.Speed / activeAdventure.SpeedRecommended))) - DateTime.Now;
                if(timeLeft.Value.Hours > 0)
                {
                    responseString += $"{timeLeft.Value.Hours} hours, ";
                }
                if (timeLeft.Value.Minutes > 0)
                {
                    responseString += $"{timeLeft.Value.Minutes} minutes, ";
                }
                if (timeLeft.Value.Seconds > 0)
                {
                    responseString += $"{timeLeft.Value.Seconds} seconds";
                }
                var response = await ctx.RespondAsync(responseString);
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new[] { ctx.Message, response });
                return;
            }
            //Else is not needed as there is a return, but it reads a bit easier
            else
            {
                player.StartedAdventure = null;
                player.AdventureId = null;
                await db.SaveChangesAsync();

                if (player.PassedAdventure == false)
                {
                    var response = await ctx.RespondAsync("You failed your adventure.");
                    await Task.Delay(5000);
                    await ctx.Channel.DeleteMessagesAsync(new[] { ctx.Message, response });
                    player.PassedAdventure = null;
                    await db.SaveChangesAsync();
                    return;
                }
                player.PassedAdventure = null;

                var finishedEmbed = new DiscordEmbedBuilder()
                    .WithTitle("Adventure Finished!")
                    .WithDescription("You have finished your adventure, your loot is:")
                    .WithColor(DiscordColor.Green);

                bool hasLoot = false;

                foreach (var item in activeAdventure.DropableItems)
                {
                    if (new Random().NextInt64(0, 101) <= item.Chance)
                    {
                        finishedEmbed.AddField(item.Item.Name, item.Item.Description);
                        item.Item.Owner = player;
                        db.Items.Add(item.Item);
                        hasLoot = true;
                    }
                }
                if (!hasLoot)
                {
                    finishedEmbed.AddField("No Loot", "You didn't get any loot from this adventure");
                }
                await db.SaveChangesAsync();
                var finishedResponse = await ctx.RespondAsync(embed: finishedEmbed);
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new DiscordMessage[] { ctx.Message, finishedResponse });
                return;
            }

        }

        var interactivity = ctx.Client.GetInteractivity();
        int index = 0;
        while (true)
        {
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Adventures")
                .WithDescription("Select an adventure to start")
                .WithColor(DiscordColor.Green);

            var adventure = Adventures[index];
            embed.AddField(adventure.Name, adventure.Description);

            var realDuration = adventure.Duration * (1.0 / (float)((float)player.Speed / (float)adventure.SpeedRecommended));
            string duration = "";
            if (adventure.Duration.Days > 0)
            {
                duration += realDuration.Days + " days ";
            }
            if (adventure.Duration.Hours > 0)
            {
                duration += realDuration.Hours + " hours ";
            }
            if (adventure.Duration.Minutes > 0)
            {
                duration += realDuration.Minutes + " minutes ";
            }
            if (adventure.Duration.Seconds > 0)
            {
                duration += realDuration.Seconds + " seconds ";
            }
            foreach (var item in adventure.DropableItems)
            {
                embed.AddField(item.Item.Name, item.Chance + "%");
            }
            embed.AddField("Duration", duration);
            embed.WithFooter($"Page {index + 1} of {Adventures.Count}");

            var leftBool = index == 0;
            var rightBool = index == Adventures.Count - 1;

            var right = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnRight", "➡️", rightBool);
            var left = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnLeft", "⬅️", leftBool);
            var x = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Success, "btnCheck", "✅");

            var msg = await new DiscordMessageBuilder().WithEmbed(embed).AddComponents(new DiscordComponent[] { left, x, right }).SendAsync(ctx.Channel);

            var result = await interactivity.WaitForButtonAsync(msg, TimeSpan.FromMinutes(2)).ConfigureAwait(false);

            if (result.TimedOut)
            {
                await ctx.Channel.DeleteMessageAsync(msg);
                return;
            }

            if (result.Result.Id == "btnLeft")
            {
                index--;
            }
            else if (result.Result.Id == "btnRight")
            {
                index++;
            }
            else if (result.Result.Id == "btnCheck")
            {
                player.StartedAdventure = DateTime.Now;
                player.AdventureId = adventure.Id;
                bool completedDungeon = new Random().NextInt64(0, 101) <= (float)(.75 * (float)((float)player.Strength / (float)adventure.StrengthRecommended)) * 100;
                player.PassedAdventure = completedDungeon;
                await db.SaveChangesAsync();
                await ctx.Channel.DeleteMessageAsync(msg);
                var reply = await ctx.RespondAsync("Adventure started!");
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new[] { reply, ctx.Message });
                return;
            }


        }
    }

}

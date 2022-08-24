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

    List<AdventureBase> Adventures { get; set; } = new List<AdventureBase>
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
            if (player.StartedAdventure + activeAdventure.Duration > DateTime.Now)
            {
                var response = await ctx.RespondAsync("You are still on an adventure for " + (player.StartedAdventure + activeAdventure.Duration - DateTime.Now).ToString());
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new DiscordMessage[] { ctx.Message, response });
                return;
            }
            //Else is not needed as there is a return, but it reads a bit easier
            else
            {
                player.StartedAdventure = null;
                player.AdventureId = null;
                await db.SaveChangesAsync();

                var finishedEmbed = new DiscordEmbedBuilder()
                    .WithTitle("Adventure Finished!")
                    .WithDescription("You have finished your adventure, your loot is:")
                    .WithColor(DiscordColor.Green);

                bool hasLoot = false;
                foreach(var item in activeAdventure.DropableItems)
                {
                    if (new Random().NextInt64(0,101) <= item.Chance)
                    {
                        finishedEmbed.AddField(item.Item.Name, item.Item.Description);
                        item.Item.Owner = player;
                        db.Items.Add(item.Item);
                        hasLoot = true;
                    }
                }
                if(!hasLoot)
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
            embed.AddField("Duration", adventure.Duration.ToString("g"));
            foreach (var item in adventure.DropableItems)
            {
                embed.AddField(item.Item.Name, item.Chance + "%");
            }
            embed.WithFooter($"Page {index + 1} of {Adventures.Count}");

            DiscordButtonComponent left, right, x;

            bool leftBool, rightBool;
            leftBool = index == 0;
            rightBool = index == Adventures.Count - 1;

            right = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnRight", "➡️", rightBool);
            left = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnLeft", "⬅️", leftBool);
            x = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Danger, "btnCheck", "✅");

            var msg = await new DiscordMessageBuilder().WithEmbed(embed).AddComponents(new DiscordComponent[] { left, x, right }).SendAsync(ctx.Channel);

            var result = await interactivity.WaitForButtonAsync(msg, TimeSpan.FromMinutes(2)).ConfigureAwait(false);

            if (result.TimedOut)
            {
                await ctx.Channel.DeleteMessageAsync(msg);
                return;
            }
            else if (result.Result.Id == "btnLeft")
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
                await db.SaveChangesAsync();
                await ctx.Channel.DeleteMessageAsync(msg);
                var reply = await ctx.RespondAsync("Adventure started!");
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new DiscordMessage[] { reply, ctx.Message });
                return;
            }


        }
    }

}

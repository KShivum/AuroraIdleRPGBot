using AuroraLibrary.ConfigManager;
using AuroraLibrary.DatabaseModels;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace AuroraDiscordBot.Commands;

public class CreatePlayer : BaseCommandModule
{
    public RPGBotDBContext db {private get; set;}

    [Command("createplayer")]
    public async Task CreatePlayerCommand(CommandContext ctx)
    {
        if(db.Players.Any(x => x.Id == ctx.User.Id))
        {
            var failed = await ctx.RespondAsync("You already have a player");
            await Task.Delay(2000);
            await ctx.Channel.DeleteMessageAsync(failed);
            await ctx.Channel.DeleteMessageAsync(ctx.Message);
            return;
        }


        int startingGold = 0;
        //Get the starting gold from the config file
        if(!ConfigManager.AddToConfigAndError("StartingGold"))
        {
            startingGold = int.Parse(ConfigManager.Config["StartingGold"]);
        }


        List<DiscordMessage> messages = new List<DiscordMessage>();
        var message = await ctx.RespondAsync("What is your name?");
        messages.Add(message);
        var result = await ctx.Message.GetNextMessageAsync();
        if(result.TimedOut == true)
        {
            await ctx.RespondAsync("No response received");
            return;
        }
        messages.Add(result.Result);
        string playerName = result.Result.Content;

        var player = new Player()
        {
            Id = ctx.User.Id,
            PlayerName = playerName,
            Gold = startingGold,
            Level = 1,
            Experience = 0,
            Speed = 1,
            Strength = 1,
        };
        db.Players.Add(player);
        await db.SaveChangesAsync();
        var response = await ctx.RespondAsync($"{playerName} has been created with {startingGold} gold");
        await Task.Delay(5000);

        await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { message, result.Result, ctx.Message, response });
    }
}

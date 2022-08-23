using AuroraLibrary.DatabaseModels;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace AuroraDiscordBot.Commands;

public class CreateItem : BaseCommandModule
{

    public RPGBotDBContext db {private get; set;}

    [Command("createitem")]
    public async Task CreateItemCommand(CommandContext ctx)
    {
        Player? player = db.Players.FirstOrDefault(x => x.Id == ctx.User.Id);
        if(player == null)
        {
            var failed = await ctx.RespondAsync("You do not have a player, create one with !createplayer");
            await Task.Delay(2000);
            await ctx.Channel.DeleteMessageAsync(failed);
            await ctx.Channel.DeleteMessageAsync(ctx.Message);
            return;
        }

        Item item = new Item()
        {
            Owner = player,
            Name = "Sword",
            Description = "This is a new item",
            Value = 0,
            Type = ItemTypes.Sword,
            Equipped = false,
            Atttribute1Label = "Attack",
            Atttribute1Value = 3,
        };
        db.Items.Add(item);
        await db.SaveChangesAsync();
        var response = await ctx.RespondAsync($"{item.Name} has been created with {item.Value} gold");
        await Task.Delay(5000);
        await ctx.Channel.DeleteMessagesAsync(new DiscordMessage[] {ctx.Message, response});
    }

}

using AuroraLibrary.Config;
using AuroraLibrary.DatabaseModels;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace AuroraDiscordBot.Commands;

public class Inventory : BaseCommandModule
{

    public RPGBotDBContext db { private get; set; }
    public Config _config { private get; set; }

    [Command("inventory")]
    public async Task InventoryCommand(CommandContext ctx)
    {
        Player? player = db.Players.FirstOrDefault(x => x.Id == ctx.User.Id);
        await new GlobalFunctions().CheckIfUserExists(ctx, player);

        var interactivity = ctx.Client.GetInteractivity();

        int maxItemsToShow = _config.GameSettings.MaxItemsToShowInInventory;
        

        var items = db.Items.Where(x => x.Owner == player).ToList();

        int itemIndex = 0;
        while (1 == 1)
        {
            string inventoryString = "";
            inventoryString += $"{player.PlayerName}'s Inventory\n";
            inventoryString += $"{player.PlayerName} has {player.Gold} gold\n\n\n";
            for (int i = itemIndex; i < itemIndex + maxItemsToShow; i++)
            {
                if (i >= items.Count)
                {
                    break;
                }
                inventoryString += "\n";
                inventoryString += GetItemString(items[i]);
            }
            var embed = new DiscordEmbedBuilder
            {
                Title = "Inventory",
                Description = inventoryString,
                Color = new DiscordColor(0xFF0000)
            };

            embed.WithFooter($"Page {itemIndex / maxItemsToShow + 1} of {(items.Count - 1) / maxItemsToShow + 1}");

            DiscordButtonComponent left, right, x;

            bool leftBool, rightBool;

            //If adding the maxItemsToShow to itemIndex would go over the max number of items, set rightBool to true
            rightBool = itemIndex + maxItemsToShow > items.Count;
            right = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnRight", "➡️", rightBool);

            leftBool = false;

            if (itemIndex == 0)
            {
                leftBool = true;
            }

            left = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnLeft", "⬅️", leftBool);

            x = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnX", "❌");

            var msg = await new DiscordMessageBuilder().WithEmbed(embed).AddComponents(new DiscordComponent[] { left, x, right }).SendAsync(ctx.Channel);

            var result = await interactivity.WaitForButtonAsync(msg, TimeSpan.FromMinutes(2)).ConfigureAwait(false);

            if(result.TimedOut)
            {
                await ctx.Channel.DeleteMessageAsync(msg);
                return;
            }
            else if(result.Result.Id == "btnLeft")
            {
                
                itemIndex -= maxItemsToShow;
                if (itemIndex < 0)
                {
                    itemIndex = 0;
                }
                await ctx.Channel.DeleteMessageAsync(msg);
            }
            else if(result.Result.Id == "btnRight")
            {
                itemIndex += maxItemsToShow;
                if (itemIndex > items.Count - 1)
                {
                    itemIndex = items.Count - 1;
                }
                await ctx.Channel.DeleteMessageAsync(msg);
            }
            else if(result.Result.Id == "btnX")
            {
                await ctx.Channel.DeleteMessageAsync(msg);
                await ctx.Channel.DeleteMessageAsync(ctx.Message);
                return;
            }
            


        }




    }

    string GetItemString(Item item)
    {
        string returnString = "";
        returnString += $"{item.Name} ({item.Id}) - {item.Description}";
        returnString += $"\n    Value: {item.Value}";
        returnString += $"\n    Type: {item.Type}";
        if (item.Equipped != null)
        {
            returnString += $"\n    Equipped: {item.Equipped}";
        }
        if (item.Atttribute1Label != null)
        {
            returnString += $"\n    {item.Atttribute1Label}: {item.Atttribute1Value}";
        }
        if (item.Atttribute2Label != null)
        {
            returnString += $"\n    {item.Atttribute2Label}: {item.Atttribute2Value}";
        }
        if (item.Atttribute3Label != null)
        {
            returnString += $"\n    {item.Atttribute3Label}: {item.Atttribute3Value}";
        }
        if (item.Atttribute4Label != null)
        {
            returnString += $"\n    {item.Atttribute4Label}: {item.Atttribute4Value}";
        }
        returnString += "\n";
        return returnString;

    }

}

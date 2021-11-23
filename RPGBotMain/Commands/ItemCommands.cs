using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using RPGBotMain.Models;

namespace RPGBotMain.Commands
{
    public class ItemCommands : BaseCommandModule
    {
        public SqlConnection con { private get; set; }

        [Command("item")]
        public async Task CreateItem(CommandContext ctx)
        {
            Item item = new Item("Wooden Sword", (int)(new Random()).NextInt64(15), 0, null, null, false, ctx.User.Id.ToString());
            item.Create(con);
            await ctx.RespondAsync($"{ctx.User.Mention} created an item!");
        }



        [Command("inventory")]
        public async Task Inventory(CommandContext ctx)
        {
            int amountToShow = 15;
            var interactivity = ctx.Client.GetInteractivity();
            SqlDataAdapter adapter = new SqlDataAdapter($"Select * from Item WHERE Owner = '{ctx.User.Id}'", con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                var messages = await ctx.RespondAsync("You don't have any items!");
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { messages, ctx.Message });
                return;
            }
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

            int startingItem = 0;

            while (true)
            {
                var inventoryMessage = new DiscordMessageBuilder();
                var embed = new DiscordEmbedBuilder();
                embed.WithTitle("Inventory");
                embed.WithColor(DiscordColor.Blue);

                for (int i = startingItem; i < startingItem + amountToShow; i++)
                {
                    if (i >= dt.Rows.Count)
                    {
                        break;
                    }

                    embed.AddField($"Id: {dt.Rows[i]["Id"]}", $"{dt.Rows[i]["ItemName"]}: {dt.Rows[i]["Stat1"]} {dt.Rows[i]["Stat2"]} {dt.Rows[i]["Stat3"]} {dt.Rows[i]["Stat4"]}");
                }

                var LeftArrow = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Left", "Left", false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":arrow_left:")));
                var Exit = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Exit", "Exit", false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":x:")));
                var RightArrow = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Right", "Right", false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":arrow_right:")));

                if (startingItem == 0)
                {
                    LeftArrow.Disable();
                }

                if (startingItem + amountToShow >= dt.Rows.Count)
                {
                    RightArrow.Disable();
                }



                inventoryMessage.WithEmbed(embed);

                inventoryMessage.AddComponents(LeftArrow, Exit, RightArrow);

                var sentMessage = await ctx.Channel.SendMessageAsync(inventoryMessage);


                var buttonResult = await interactivity.WaitForButtonAsync(sentMessage, new List<DiscordButtonComponent> { LeftArrow, RightArrow, Exit }, TimeSpan.FromMinutes(2));

                if (buttonResult.TimedOut)
                {
                    await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { sentMessage, ctx.Message });
                    return;
                }

                if (buttonResult.Result.Id == "Left")
                {
                    startingItem -= amountToShow;
                }
                else if (buttonResult.Result.Id == "Right")
                {
                    startingItem += amountToShow;
                }
                else if (buttonResult.Result.Id == "Exit")
                {
                    await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { sentMessage, ctx.Message });
                    return;
                }

                await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { sentMessage });


            }

        }


    }
}

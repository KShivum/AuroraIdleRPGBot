using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MySql.Data.MySqlClient;
using static DiscordBot.GlobalItems;

namespace DiscordBot.Commands
{
    public class InventoryCommand : BaseCommandModule
    {
        [Command("inventory")]
        public async Task Inventory(CommandContext ctx, [Description("Does not work for card")] bool inChat = true, [Description("cards is not really supported")] string format = "list")
        {
            if (!(format.Equals("card") || format.Equals("list")))
            {
                var wrongFormatEmbed = new DiscordEmbedBuilder
                {
                    Title = "Unknown format",
                    Description = "Please use either card or list",
                    Color = DiscordColor.Red
                };
                var wrongFormat = await ctx.Channel.SendMessageAsync(embed: wrongFormatEmbed).ConfigureAwait(false);
                return;
            }

            List<Item> items = new List<Item>();
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter($"Select * from items Where Owner = '{ctx.User.Id}'", ConString);

            try
            {
                adapter.Fill(ds);
            }
            catch (Exception e)
            {
                var failedToConnect = await ctx.Channel.SendMessageAsync("Error connecting to server");
                Console.WriteLine(e.ToString());
                return;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var temp = new Item();

                temp.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ItemID"]);
                temp.ItemType = ds.Tables[0].Rows[i]["ItemType"].ToString();
                temp.Name = ds.Tables[0].Rows[i]["Name"].ToString();

                //We might need to convert those ints to nullable ints or check, which I will probably do
                if (ds.Tables[0].Rows[i]["Stat1"] != DBNull.Value)
                    temp.Stat1 = Convert.ToInt32(ds.Tables[0].Rows[i]["Stat1"]);
                if (ds.Tables[0].Rows[i]["Stat2"] != DBNull.Value)
                    temp.Stat2 = Convert.ToInt32(ds.Tables[0].Rows[i]["Stat2"]);
                if (ds.Tables[0].Rows[i]["Stat3"] != DBNull.Value)
                    temp.Stat3 = Convert.ToInt32(ds.Tables[0].Rows[i]["Stat3"]);
                if (ds.Tables[0].Rows[i]["Stat4"] != DBNull.Value)
                    temp.Stat4 = Convert.ToInt32(ds.Tables[0].Rows[i]["Stat4"]);
                items.Add(temp);
            }



            if (format.Equals("card"))
            {
                var dmChannel = await ctx.Member.CreateDmChannelAsync();
                int index = 0;

                List<DiscordMessage> messages = new List<DiscordMessage>();
                var interactivity = ctx.Client.GetInteractivity();


                while (true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (index >= items.Count)
                        {
                            break;
                        }
                        var tempMessage = await dmChannel.SendMessageAsync(embed: items[index].GetItemStats()).ConfigureAwait(false);
                        messages.Add(tempMessage);
                        index++;
                    }

                    var infoEmbed = new DiscordEmbedBuilder
                    {
                        Title = $"Showing items {index - 5} to {index} out of {items.Count}"
                    };

                    var info = await dmChannel.SendMessageAsync(embed: infoEmbed).ConfigureAwait(false);
                    DiscordEmoji left = DiscordEmoji.FromName(ctx.Client, ":arrow_left:");
                    DiscordEmoji right = DiscordEmoji.FromName(ctx.Client, ":arrow_right:");
                    DiscordEmoji xEmoji = DiscordEmoji.FromName(ctx.Client, ":x:");

                    if (index < items.Count - 1)
                    {
                        await info.CreateReactionAsync(right);
                    }
                    if (index >= 5)
                    {
                        await info.CreateReactionAsync(left);
                    }


                    await info.CreateReactionAsync(xEmoji);

                    var result = await interactivity.WaitForReactionAsync((x) => x.Emoji == left || x.Emoji == right || x.Emoji == xEmoji, info, ctx.User, TimeSpan.FromMinutes(2)).ConfigureAwait(false);

                    if (result.Result == null)
                    {
                        //TODO add timeout embed
                        for (int i = 0; i < messages.Count; i++)
                        {
                            await dmChannel.DeleteMessageAsync(messages[i]).ConfigureAwait(false);
                        }
                        messages.Clear();
                        await dmChannel.DeleteMessageAsync(info).ConfigureAwait(false);
                        return;
                    }

                    if (result.Result.Emoji == right)
                    {
                        //Too tired to fix it right now, but the user could possibly add the reaction themself and mess stuff up
                        for (int i = 0; i < messages.Count; i++)
                        {
                            await dmChannel.DeleteMessageAsync(messages[i]).ConfigureAwait(false);
                        }
                        messages.Clear();
                        await dmChannel.DeleteMessageAsync(info).ConfigureAwait(false);
                        continue;
                    }

                    if (result.Result.Emoji == left)
                    {
                        index -= 5;
                        for (int i = 0; i < messages.Count; i++)
                        {
                            await dmChannel.DeleteMessageAsync(messages[i]).ConfigureAwait(false);
                        }
                        messages.Clear();
                        await dmChannel.DeleteMessageAsync(info).ConfigureAwait(false);
                        continue;
                    }
                    if (result.Result.Emoji == xEmoji)
                    {
                        for (int i = 0; i < messages.Count; i++)
                        {
                            await dmChannel.DeleteMessageAsync(messages[i]).ConfigureAwait(false);
                        }
                        messages.Clear();
                        await dmChannel.DeleteMessageAsync(info).ConfigureAwait(false);
                        return;
                    }




                }



            }
            else if (format.Equals("list"))
            {
                int index = 0;
                int prevIndex = 0;

                var interactivity = ctx.Client.GetInteractivity();

                while (true)
                {
                    prevIndex = index;
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "Inventory"
                    };
                    int target = index + 20;
                    for (int i = index; i < target; i++)
                    {
                        if (index >= items.Count)
                        {
                            break;
                        }
                        embed.AddField(items[i].Name, items[i].GetItemStatStringWithID());
                        index++;
                    }

                    embed.WithFooter($"{prevIndex + 1} - {index} out of {items.Count}");

                    //var info = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                    //DiscordEmoji left = DiscordEmoji.FromName(ctx.Client, ":arrow_left:");
                    //DiscordEmoji right = DiscordEmoji.FromName(ctx.Client, ":arrow_right:");
                    //DiscordEmoji xEmoji = DiscordEmoji.FromName(ctx.Client, ":x:");

                    DiscordButtonComponent left, right, x;
                    if (index < items.Count - 1)
                    {
                        //await info.CreateReactionAsync(right);
                        right = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnRight", "➡️");
                    }
                    else
                    {
                        right = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnRight", "➡️", true);
                    }

                    x = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnX", "❌");

                    if (index >= 21)
                    {
                        //await info.CreateReactionAsync(left);
                        left = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnLeft", "⬅️");
                    }
                    else
                    {
                        left = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "btnLeft", "⬅️", true);
                    }
                    var message = await new DiscordMessageBuilder()
                    .WithEmbed(embed)
                    .AddComponents(new DiscordComponent[] {
                        left, x, right
                    }).SendAsync(ctx.Channel);



                    var result = await interactivity.WaitForButtonAsync(message, TimeSpan.FromMinutes(2)).ConfigureAwait(false);

                    if (result.Result == null)
                    {
                        //TODO add timeout embed

                        await ctx.Channel.DeleteMessageAsync(message).ConfigureAwait(false);
                        return;
                    }

                    if (result.Result.Id == "btnRight")
                    {
                        //Too tired to fix it right now, but the user could possibly add the reaction themself and mess stuff up

                        await ctx.Channel.DeleteMessageAsync(message).ConfigureAwait(false);
                        continue;
                    }

                    if (result.Result.Id == "btnLeft")
                    {
                        index = index - (index % 20) - 20;

                        await ctx.Channel.DeleteMessageAsync(message).ConfigureAwait(false);
                        continue;
                    }
                    if (result.Result.Id == "btnX")
                    {

                        await ctx.Channel.DeleteMessageAsync(message).ConfigureAwait(false);
                        return;
                    }




                }


            }


        }

        [Command("equip")]
        public async Task Equip(CommandContext ctx, [Description("The item's id that you want to equip")] int itemId)
        {
            /*
                Check if the item exists or they own it
                Update the item in the database to equipped
                Get the Item type
                Update the user in the database depending on what type the item is
            */


            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                //Checking if the bot can connect to the server
                try
                {
                    con.Open();
                }
                catch
                {
                    var failedEmbed = CreateEmbed("Could not connect to server", "Sorry, I couldn't connect to the database, please contact Shivum", DiscordColor.Red);
                    await ctx.Channel.SendMessageAsync(embed: failedEmbed).ConfigureAwait(false);
                    return;
                }


                //Gets the item row
                MySqlDataAdapter adapter = new MySqlDataAdapter($"SELECT * FROM items WHERE Owner = '{ctx.User.Id}' AND ItemID = {itemId}", con);
                DataSet item = new DataSet();
                adapter.Fill(item);

                DataSet user = new DataSet();
                var userAdapter = new MySqlDataAdapter($"SELECT * FROM users WHERE id = '{ctx.User.Id}'", con);
                userAdapter.Fill(user);

                //Checks if the item exists and/or the user owns the item essentially
                if (item.Tables[0].Rows.Count == 0)
                {
                    var itemDoesNotExist = CreateEmbed("Failed to Retrieve Item!", "Unable to retrieve item with the id given, please check the id and if you own the item", DiscordColor.Red);
                    await ctx.Channel.SendMessageAsync(embed: itemDoesNotExist).ConfigureAwait(false);
                    return;
                }

                //Unequip the previous item
                //TODO: Figure out something better
                var itemType = item.Tables[0].Rows[0]["ItemType"];
                int? overwriteItemId = null;
                if (itemType.Equals("Sword"))
                {
                    if (DBNull.Value != user.Tables[0].Rows[0]["equippedWeapon"])
                    {
                        overwriteItemId = Convert.ToInt32(user.Tables[0].Rows[0]["equippedWeapon"]);
                    }

                }
                else if (itemType.Equals("Chestplate"))
                {
                    if (DBNull.Value != user.Tables[0].Rows[0]["equippedChestplate"])
                    {
                        overwriteItemId = Convert.ToInt32(user.Tables[0].Rows[0]["equippedChestplate"]);
                    }
                }
                else if (itemType.Equals("Leggings"))
                {
                    if (DBNull.Value != user.Tables[0].Rows[0]["equippedLeggings"])
                    {
                        overwriteItemId = Convert.ToInt32(user.Tables[0].Rows[0]["equippedLeggings"]);
                    }
                }

                if (overwriteItemId != null)
                {
                    MySqlCommand unequipCommand = new MySqlCommand($"Update items Set Equipped = 0 Where ItemID = {overwriteItemId}", con);
                    unequipCommand.ExecuteNonQuery();
                }

                item.Tables[0].Rows[0]["Equipped"] = 1;





                //Correctly sets the values for the user;
                if (itemType.Equals("Sword"))
                {
                    user.Tables[0].Rows[0]["equippedWeapon"] = itemId;
                }
                else if (itemType.Equals("Chestplate"))
                {
                    user.Tables[0].Rows[0]["equippedChestplate"] = itemId;
                }
                else if (itemType.Equals("Leggings"))
                {
                    user.Tables[0].Rows[0]["equippedLeggings"] = itemId;
                }


                MySqlCommandBuilder builder = new MySqlCommandBuilder(userAdapter);
                MySqlCommandBuilder itemBuilder = new MySqlCommandBuilder(adapter);

                builder.GetUpdateCommand();
                itemBuilder.GetUpdateCommand();

                adapter.Update(item);
                userAdapter.Update(user);

                //Creates and returns embed

                var successEmbed = CreateEmbed("Equipped Item!", $"You have equipped:\n{Item.GetItemFromId(itemId).GetItemStatString()}", DiscordColor.Green);
                await ctx.Channel.SendMessageAsync(embed: successEmbed).ConfigureAwait(false);
            }




        }

    }
}
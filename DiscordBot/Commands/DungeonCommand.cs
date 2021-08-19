using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DiscordBot.Adventures;
using DiscordBot.Adventures.Dungeons;
using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MySql.Data.MySqlClient;
using static DiscordBot.GlobalItems;

namespace DiscordBot.Commands
{
    public class DungeonCommand : BaseCommandModule
    {
         [Command("dungeon")]
        public async Task Dungeon(CommandContext ctx)
        {
            //I will just create a list of all available dungeons
            //*Can make this dynamic if we wish to
            List<AdventureInterface> dungeons = new List<AdventureInterface>();
            dungeons.Add(new TestDungeon());
            int index = 0;
            DataSet user = new DataSet();
            //Adds the user to the database
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter($"Select * from users Where id = {ctx.User.Id}",ConString);
                adapter.Fill(user);
            }
            catch
            {
                var failedEmbed = CreateEmbed("Failed", "There was an issue connecting to the server", DiscordColor.Red);
                var failed = await ctx.Channel.SendMessageAsync(embed: failedEmbed);
            }

            //Checking if the player already has a dungeon and if the have completed it
            if(user.Tables[0].Rows[0]["dungeonStartTime"] != DBNull.Value) 
            {
                var currentDungeon = dungeons.Find(x => x.ID == Convert.ToInt32(user.Tables[0].Rows[0]["currentDungeon"]));
                if(DateTime.Now > Convert.ToDateTime(user.Tables[0].Rows[0]["dungeonStartTime"]) + currentDungeon.dungeonTime)
                {
                    List<Item> loot;
                    using(MySqlConnection con = new MySqlConnection(ConString))
                    {
                        con.Open();
                        loot = currentDungeon.Finish(con, ctx.User.Id);
                        if(loot == null)
                        {
                            var failedDungeonEmbed = CreateEmbed("You Failed the Dungeon", "You barely escaped the dungeon with your life", DiscordColor.Red);
                            MySqlCommand clearDungeon = new MySqlCommand($"Update users Set currentDungeon = NULL, dungeonStartTime = NULL Where id = '{ctx.User.Id}'",con);
                            clearDungeon.ExecuteNonQuery();
                            await ctx.Channel.SendMessageAsync(embed: failedDungeonEmbed).ConfigureAwait(false);
                            return;
                        }
                        foreach (var item in loot)
                        {
                            var stat1 = item.Stat1;
                            var stat2 = item.Stat2;
                            var stat3 = item.Stat3;
                            var stat4 = item.Stat4;
                            string commandText = $"Insert into items (ItemType, Stat1, Stat2, Stat3, Stat4, Owner, Name) Values ('{item.ItemType}', ";

                            if(stat1 == null)
                                commandText += "NULL, ";
                            else
                                commandText += $"{stat1}, ";
                            
                            if(stat2 == null)
                                commandText += "NULL, ";
                            else
                                commandText += $"{stat2}, ";

                            if(stat3 == null)
                                commandText += "NULL, ";
                            else
                                commandText += $"{stat3}, ";

                            if(stat4 == null)
                                commandText += "NULL, ";
                            else
                                commandText += $"{stat4}, ";

                            commandText += $"'{ctx.User.Id}', '{item.Name}')";

                            MySqlCommand cmd = new MySqlCommand(commandText, con);
                            cmd.ExecuteNonQuery();
                        }
                        MySqlCommand resetCmd = new MySqlCommand($"Update users Set currentDungeon = NULL, dungeonStartTime = NULL Where id = '{ctx.User.Id}'",con);
                        resetCmd.ExecuteNonQuery();
                    }

                    var finishedDungeonEmbed = CreateEmbed("You returned!", "You arrived from the dungeon with loot!", DiscordColor.Green);
                    foreach (var item in loot)
                    {
                        finishedDungeonEmbed.AddField(item.Name, item.GetItemStatString());
                    }

                    var finishedDungeon = await ctx.Channel.SendMessageAsync(embed: finishedDungeonEmbed).ConfigureAwait(false);
                    return;

                }
                else
                {
                    //If the time elapsed hasn't passed
                    TimeSpan timeRemaining = (Convert.ToDateTime(user.Tables[0].Rows[0]["dungeonStartTime"]) + currentDungeon.dungeonTime) - DateTime.Now;
                    var notCompletedEmbed = CreateEmbed("You haven't completed the dungeon yet!", $"You have {timeRemaining.ToString()} left", DiscordColor.Red);
                    var notCompleted = ctx.Channel.SendMessageAsync(embed: notCompletedEmbed).ConfigureAwait(false);
                    return;
                }
            }
            while(true)
            {
                DiscordEmbedBuilder startEmbed = new DiscordEmbedBuilder
                {
                    Title = "Choose a dungeon"
                };

                var interactivity = ctx.Client.GetInteractivity();
                if(index == 0)
                {
                    startEmbed.AddField($"{dungeons[0].name}", $"Drops between {dungeons[0].minDrops}-{dungeons[0].maxDrops} items");
                }
                var start = await ctx.Channel.SendMessageAsync(embed: startEmbed).ConfigureAwait(false);

                //Add Emoji reactions and stuff later...
                DiscordEmoji yes = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
                DiscordEmoji left = DiscordEmoji.FromName(ctx.Client,":arrow_left:");
                DiscordEmoji right = DiscordEmoji.FromName(ctx.Client, ":arrow_right:");

                //So we don't get a left emote on the first dungeon
                if(index != 0)
                {
                    await start.CreateReactionAsync(left);
                }

                
                await start.CreateReactionAsync(yes);

                //So we can't go past the amount of dungeons we have
                if(index != dungeons.Count-1)
                {
                    await start.CreateReactionAsync(right);
                }

                var result = await interactivity.WaitForReactionAsync( (x) => x.Emoji == left || x.Emoji == right || x.Emoji == yes, start, ctx.User, new TimeSpan(0,1,0)).ConfigureAwait(false);

                if(result.TimedOut)
                {
                    var timedOutEmbed = new DiscordEmbedBuilder
                    {
                        Title="Timed Out...",
                        Description="Took too long for a response..",
                        Color = DiscordColor.Red
                    };
                    await ctx.Channel.DeleteMessageAsync(start);
                    var timedOut = await ctx.Channel.SendMessageAsync(embed: timedOutEmbed).ConfigureAwait(false);
                    return;
                }

                if(result.Result.Emoji == left)
                {
                    if(index == 0)
                    {
                        var invalidEmbed = new DiscordEmbedBuilder
                        {
                            Title="Invalid",
                            Description="Stop adding emotes...\nIf you got this message naturally, something went wrong...",
                            Color = DiscordColor.Red
                        };
                        await ctx.Channel.DeleteMessageAsync(start);
                        var invalid = await ctx.Channel.SendMessageAsync(embed: invalidEmbed).ConfigureAwait(false);
                        return;
                    }

                    await ctx.Channel.DeleteMessageAsync(start);
                    index -= 1;
                    continue;
                }

                if(result.Result.Emoji == right)
                {
                    if(index == dungeons.Count-1)
                    {
                        var invalidEmbed = new DiscordEmbedBuilder
                        {
                            Title="Invalid",
                            Description="Stop adding emotes...\nIf you got this message naturally, something went wrong...",
                            Color = DiscordColor.Red
                        };
                        await ctx.Channel.DeleteMessageAsync(start);
                        var invalid = await ctx.Channel.SendMessageAsync(embed: invalidEmbed).ConfigureAwait(false);
                        return;
                    }
                    await ctx.Channel.DeleteMessageAsync(start);
                    index++;
                    continue;
                }

                if(result.Result.Emoji == yes)
                {
                    //TODO: Do a level check when that is relevant
                    using(MySqlConnection con = new MySqlConnection(ConString))
                    {
                        try
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand($"Update users Set currentDungeon = {dungeons[index].ID}, dungeonStartTime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}' Where id = '{ctx.User.Id}'", con);
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            var failedEmbed = CreateEmbed("Failed to start dungeon", "There was an issue connecting to the server", DiscordColor.Red);
                            await ctx.Channel.DeleteMessageAsync(start).ConfigureAwait(false);
                            var failed = await ctx.Channel.SendMessageAsync(embed: failedEmbed);
                        } 

                        var completeEmbed = CreateEmbed("You head out into the dungeon",
                            $"You went into the {dungeons[index].name}, you should be out in {dungeons[index].dungeonTime.ToString()}", 
                            DiscordColor.None);
                        
                        await ctx.Channel.DeleteMessageAsync(start).ConfigureAwait(false);
                        var complete = await ctx.Channel.SendMessageAsync(embed:completeEmbed).ConfigureAwait(false);

                    }
                    return;
                }
                break;
            }


        }

    }
}
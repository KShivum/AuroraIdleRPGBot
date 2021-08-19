using System;
using System.Data;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MySql.Data.MySqlClient;
using static DiscordBot.GlobalItems;

namespace DiscordBot.Commands
{
    class CreateUserCommand : BaseCommandModule
    {
        [Command("createuser")]
        [Description("Used to create a character")]
        public async Task CreateUser(CommandContext ctx, [Description("[Optional] Used to set a custom name for your character, if left blank, uses your current username")] string name = "")
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Create a User",
                Description = "Creates a new character!"
            };

            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter($"Select * from users Where id = '{ctx.User.Id}'", con);
                DataSet possibleUsers = new DataSet();
                adapter.Fill(possibleUsers);

                

                if (possibleUsers.Tables[0].Rows.Count != 0)
                {
                    embedBuilder.Color = DiscordColor.Red;
                    embedBuilder.AddField("Error!", "You already have a character!");
                    var response = await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
                    return;
                }
            }

            string finalizedName;
            if (string.IsNullOrEmpty(name))
            {
                embedBuilder.AddField("Name:", ctx.Member.DisplayName);
                finalizedName = ctx.Member.DisplayName;

            }
            else
            {
                embedBuilder.AddField("Name:", name);
                finalizedName = name;
            }


            var embed = await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

            var accept = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
            var decline = DiscordEmoji.FromName(ctx.Client, ":x:");

            await embed.CreateReactionAsync(accept).ConfigureAwait(false);
            await embed.CreateReactionAsync(decline).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();

            var result = await interactivity.WaitForReactionAsync(x => x.Emoji == accept || x.Emoji == decline, embed, ctx.User).ConfigureAwait(false);

            if(result.Result == null)
            {
                DiscordEmbedBuilder timeoutEmbed = new DiscordEmbedBuilder
                {
                    Title = "Timedout",
                    Description = "You took too long, canceling",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.DeleteMessageAsync(embed).ConfigureAwait(false);
                var timeout = await ctx.Channel.SendMessageAsync(embed: timeoutEmbed).ConfigureAwait(false);
                return;
            }

            if (result.Result.Emoji == accept)
            {
                await ctx.Channel.DeleteMessageAsync(embed).ConfigureAwait(false);

                using (MySqlConnection con = new MySqlConnection(ConString))
                {
                    con.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter($"Select * from users Where id = '{ctx.User.Id}'", con);
                    DataSet possibleUsers = new DataSet();
                    adapter.Fill(possibleUsers);



                    if (possibleUsers.Tables[0].Rows.Count != 0)
                    {
                        embedBuilder.Color = DiscordColor.Red;
                        embedBuilder.AddField("Error!", "You already have a character!");
                        var response = await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
                        return;
                    }
                }

                try
                {
                    using (MySqlConnection con = new MySqlConnection(ConString))
                    {
                        con.Open();
                        string query = $"Insert into users Values ('{ctx.User.Id}', 0, '{finalizedName}', 100)";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch(Exception e)
                {
                    DiscordEmbedBuilder failedEmbed = CreateEmbed("Failed to Create User", "Something went wrong in the backend, Contact Shivum");
                    var failedMessage = await ctx.Channel.SendMessageAsync(embed: failedEmbed).ConfigureAwait(false);
                    Console.WriteLine(e.ToString());
                    return;
                }

                DiscordEmbedBuilder successEmbed = CreateEmbed("Success!", "Your Character has been created!", DiscordColor.Green);

                successEmbed.AddField("Name:", finalizedName);
                successEmbed.AddField("Starting Money:", "100");

                var success = await ctx.Channel.SendMessageAsync(embed: successEmbed).ConfigureAwait(false);
            }
            else
            {
                DiscordEmbedBuilder cancelEmbed = CreateEmbed("Canceled", "Canceled creating a character", DiscordColor.Red);
                await ctx.Channel.DeleteMessageAsync(embed).ConfigureAwait(false);
                var cancel = await ctx.Channel.SendMessageAsync(embed: cancelEmbed).ConfigureAwait(false);
            }

        }

    }
}
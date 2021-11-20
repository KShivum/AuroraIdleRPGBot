using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace RPGBotMain.Commands
{
    public class CreatePlayer : BaseCommandModule
    {
        public SqlConnection con {private get; set; }
        [Command("Start")]
        public async Task Start(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();


            SqlDataAdapter adapter = new SqlDataAdapter($"Select * from Users WHERE Id = '{ctx.User.Id}'",con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if(dt.Rows.Count != 0)
            {
                var message = await ctx.RespondAsync("You already have a character!");
                await Task.Delay(5000);
                await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage> { message , ctx.Message});
                return;
            }


            var intro = await ctx.Channel.SendMessageAsync("Hello, " + ctx.User.Username + "! Welcome to the RPG Bot!");
            await Task.Delay(1000);
            var intro2 = await ctx.Channel.SendMessageAsync("Please enter your desired username.");

            var username = await ctx.Message.GetNextMessageAsync();

            //TODO do some type of profanity check or something
            if(username.TimedOut)
            {
                return;
            }
            SqlCommand cmd  = new SqlCommand("Insert into Users (Id, PlayerName, Xp) Values(@id, @username, 0);", con);

            SqlParameter idParam = new SqlParameter("@id",System.Data.SqlDbType.VarChar, 255);
            SqlParameter usernameParam = new SqlParameter("@username",System.Data.SqlDbType.VarChar, 255);

            idParam.Value = ctx.User.Id.ToString();
            usernameParam.Value = username.Result.Content.ToString();

            cmd.Parameters.Add(idParam);
            cmd.Parameters.Add(usernameParam);

            cmd.Prepare();
            cmd.ExecuteNonQuery();

            var intro3 = await ctx.Channel.SendMessageAsync("Welcome, " + username.Result.Content.ToString() + "!");
            await Task.Delay(6000);

            await ctx.Channel.DeleteMessagesAsync(new List<DiscordMessage>() { intro, intro2, intro3, ctx.Message, username.Result });


        }
    }
}

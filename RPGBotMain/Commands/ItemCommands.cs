using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RPGBotMain.Models;

namespace RPGBotMain.Commands
{
    public class ItemCommands : BaseCommandModule
    {
        public SqlConnection con { private get; set; }

        [Command("item")]
        public async Task CreateItem(CommandContext ctx)
        {
            Item item = new Item("Wooden Sword", 1, 0, null, null, false, ctx.User.Id.ToString());
            item.Create(con);
            await ctx.RespondAsync($"{ctx.User.Mention} created an item!");
        }
    }
}

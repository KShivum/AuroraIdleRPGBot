using AuroraLibrary.DatabaseModels;
using DSharpPlus.CommandsNext;

namespace AuroraDiscordBot;

public class GlobalFunctions
{
    public async Task CheckIfUserExists(CommandContext ctx, Player? player)
    {
        if(player == null)
        {
            var failed = await ctx.RespondAsync("You do not have a player, create one with !createplayer");
            await Task.Delay(2000);
            await ctx.Channel.DeleteMessageAsync(failed);
            await ctx.Channel.DeleteMessageAsync(ctx.Message);
            return;
        }
    }
}

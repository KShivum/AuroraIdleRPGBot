using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace AuroraDiscordBot.Commands;

public class Test : BaseCommandModule
{
    [Command("test")]
    public async Task TestCommand(CommandContext ctx)
    {
        await ctx.RespondAsync("Test");
    }
}

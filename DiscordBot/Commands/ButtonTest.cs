using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.Commands
{
    public class ButtonTest : BaseCommandModule
    {
        [Command("ButtonTest")]
        public async Task Test(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var messageBuilder = new DiscordMessageBuilder().WithContent("Message with button!");
            var myButton = new DiscordButtonComponent(ButtonStyle.Primary, "my_custom_id", null, false, new DiscordComponentEmoji(506897708749946900));
            messageBuilder.AddComponents(myButton);
            var message = await messageBuilder.SendAsync(ctx.Channel);
            ctx.Client.ComponentInteractionCreated += async (s, e) =>
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            };
            var result = await interactivity.WaitForButtonAsync(message, ctx.User, TimeSpan.FromMinutes(2));
            if(result.Result != null)
            {
               await new DiscordMessageBuilder().WithContent(":middle_finger:").SendAsync(ctx.Channel);
            } 

        }
    }
}
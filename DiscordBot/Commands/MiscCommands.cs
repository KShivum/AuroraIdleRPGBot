using DiscordBot.Handlers.Dialogue;
using DiscordBot.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    class MiscCommands :BaseCommandModule
    {
        [Command("ping")]
        [Description("Pongs")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers")]
        public async Task Add(CommandContext ctx, [Description("First Number")] int numberOne, [Description("Second Number")] int numberTwo)
        {
            await ctx.Channel.SendMessageAsync($"{numberOne} + {numberTwo} = {numberOne + numberTwo}").ConfigureAwait(false);
        }

        [Command("Prune")]
        public async Task Prune(CommandContext ctx, int amount)
        {
            var messages = await ctx.Channel.GetMessagesAsync(amount + 1);
            await ctx.Channel.DeleteMessagesAsync(messages).ConfigureAwait(false);
                
        }

        [Command("respondmessage")]
        public async Task RespondMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            StringBuilder newMessage = new StringBuilder(message.Result.Content);

            for(int i = 0; i < message.Result.Content.Length; i+=2)
            {
                newMessage[i] = char.ToUpper(newMessage[i]);
            }
            await ctx.Channel.SendMessageAsync(newMessage.ToString());
        }

        [Command("respondreaction")]
        public async Task RespondReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        [Command("poll")]
        public async Task Poll(CommandContext ctx, TimeSpan duration, params DiscordEmoji[] emojiOptions)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };

            var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            foreach(var option in emojiOptions)
            {
                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var result = await interactivity.CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);

            var results = result.Select(x => $"{x.Emoji}: {x.Total}");

            await ctx.Channel.SendMessageAsync(string.Join("\n", results)).ConfigureAwait(false);
        }

        [Command("dialogue")]
        public async Task Dialogue(CommandContext ctx)
        {
            string input = string.Empty;

            var inputStep = new TextStep("Enter something interesting!", null);

            inputStep.OnValidResult += (result) => input = result;

            var inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, inputStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);
        }

        [Command("emojidialogue")]
        public async Task EmojiDialogue(CommandContext ctx)
        {
            var yesStep = new TextStep("You chose yes", null);
            var noStep = new TextStep("You chose no", null);

            var emojiStep = new ReactionStep("Yes or No?", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                {DiscordEmoji.FromName(ctx.Client,":thumbsup:"), new ReactionStepData{ Content = "This means yes", NextStep = yesStep}  },
                {DiscordEmoji.FromName(ctx.Client,":thumbsdown:"), new ReactionStepData{ Content = "This means no", NextStep = noStep}  }
            });

            //var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, emojiStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if(!succeeded) { return; }
        }
    }
}

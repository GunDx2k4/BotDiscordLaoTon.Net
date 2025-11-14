using System;
using BotDiscordLaoTon.Net.Extensions;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules;

public abstract class InteractionModuleLogging(ILogger<InteractionModuleLogging> logger) : InteractionModuleBase<IInteractionContext>
{
    private Task LogInteractionAsync()
    {
        logger.LogInformation("{User} used {Command} in {Guild}/{Channel}", Context.User.ToStringDebug(), Context.Interaction.ToStringDebug(), Context.Guild.ToStringDebug(), Context.Channel.ToStringDebug());
        return Task.CompletedTask;
    }

    protected override async Task DeferAsync(bool ephemeral = false, RequestOptions? options = null)
    {
        await base.DeferAsync(ephemeral, options);
    }

    protected override async Task RespondAsync(string? text = null, Embed[]? embeds = null, bool isTTS = false, bool ephemeral = false,
            AllowedMentions? allowedMentions = null, RequestOptions? options = null, MessageComponent? components = null, Embed? embed = null, PollProperties? poll = null, MessageFlags flags = MessageFlags.None)
    {
        await LogInteractionAsync();
        await base.RespondAsync(text, embeds, isTTS, ephemeral, allowedMentions, options, components, embed, poll, flags);
    }

    protected override async Task<IUserMessage> ReplyAsync(string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None, PollProperties? poll = null)
    {
        await LogInteractionAsync();
        return await base.ReplyAsync(text, isTTS, embed, options, allowedMentions, messageReference, components, stickers, embeds, flags, poll);
    }

    protected override async Task<IUserMessage> FollowupAsync(string? text = null, Embed[]? embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions? allowedMentions = null, RequestOptions? options = null, MessageComponent? components = null, Embed? embed = null, PollProperties? poll = null, MessageFlags flags = MessageFlags.None)
    {
        await LogInteractionAsync();
        return await base.FollowupAsync(text, embeds, isTTS, ephemeral, allowedMentions, options, components, embed, poll, flags);
    }

}

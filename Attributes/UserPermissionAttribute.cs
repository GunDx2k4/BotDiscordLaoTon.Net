using System;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Attributes;

public class UserPermissionAttribute : PreconditionAttribute
{
    public GuildPermission? GuildPermission { get; }
    public ChannelPermission? ChannelPermission { get; }

    public UserPermissionAttribute(GuildPermission guildPermission)
    {
        GuildPermission = guildPermission;
    }

    public UserPermissionAttribute(ChannelPermission channelPermission)
    {
        ChannelPermission = channelPermission;
    }

    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        IGuildUser? guildUser = context.User as IGuildUser;
        if (GuildPermission.HasValue)
        {
            if (guildUser == null)
            {
                return Task.FromResult(PreconditionResult.FromError($"Command must be used in {Format.Bold(Format.Code("a guild channel."))}"));
            }

            if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
            {
                return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? $"User requires guild permission {Format.Bold(Format.Code(GuildPermission.Value.ToString()))}"));
            }
        }

        if (ChannelPermission.HasValue && !((!(context.Channel is IGuildChannel channel)) ? ChannelPermissions.All(context.Channel) : guildUser!.GetPermissions(channel)).Has(ChannelPermission.Value))
        {
            return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? $"User requires channel permission {Format.Bold(Format.Code(ChannelPermission.Value.ToString()))}"));
        }

        return Task.FromResult(PreconditionResult.FromSuccess());
    }
}


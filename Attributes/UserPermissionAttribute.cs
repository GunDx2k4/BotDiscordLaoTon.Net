using System;
using BotDiscordLaoTon.Net.Guilds;
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
        var guildConfiguration = GuildManager.GuildConfigurations[0];
        if (GuildPermission.HasValue)
        {
            guildConfiguration = GuildManager.GuildConfigurations[context.Guild.Id];
            if (guildUser == null)
            {
                return Task.FromResult(PreconditionResult.FromError(guildConfiguration.LocalizedStrings.ErrorCommandMustBeInGuildChannel));
            }

            if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
            {
                return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? string.Format(guildConfiguration.LocalizedStrings.ErrorUserRequiresGuildPermission, $"{Format.Bold(Format.Code(GuildPermission.Value.ToString()))}"))); 
            }
        }

        if (ChannelPermission.HasValue && !((!(context.Channel is IGuildChannel channel)) ? ChannelPermissions.All(context.Channel) : guildUser!.GetPermissions(channel)).Has(ChannelPermission.Value))
        {
            return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? string.Format(guildConfiguration.LocalizedStrings.ErrorBotRequiresChannelPermission , $"{Format.Bold(Format.Code(ChannelPermission.Value.ToString()))}")));
        }

        return Task.FromResult(PreconditionResult.FromSuccess());
    }
}


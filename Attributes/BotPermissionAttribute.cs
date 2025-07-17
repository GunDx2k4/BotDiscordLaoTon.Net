using System;
using BotDiscordLaoTon.Net.Guilds;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Attributes;

public class BotPermissionAttribute : PreconditionAttribute
{   
     public GuildPermission? GuildPermission { get; }
    public ChannelPermission? ChannelPermission { get; }

    public BotPermissionAttribute(GuildPermission guildPermission)
    {
        GuildPermission = guildPermission;
    }

    public BotPermissionAttribute(ChannelPermission channelPermission)
    {
        ChannelPermission = channelPermission;
    }

    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        IGuildUser? guildUser = null;
        var guildConfiguration = GuildManager.GuildConfigurations[0];
        if (context.Guild != null)
        {
            guildUser = await context.Guild.GetCurrentUserAsync().ConfigureAwait(continueOnCapturedContext: false);
            guildConfiguration = GuildManager.GuildConfigurations[context.Guild.Id];
        }

        if (GuildPermission.HasValue)
        {
            if (guildUser == null)
            {
                return PreconditionResult.FromError(guildConfiguration.LocalizedStrings.ErrorCommandMustBeInGuildChannel);
            }

            if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
            {
                return PreconditionResult.FromError(ErrorMessage ?? string.Format(guildConfiguration.LocalizedStrings.ErrorBotRequiresGuildPermission, $"{Format.Bold(Format.Code(GuildPermission.Value.ToString()))}")); 
            }
        }

        if (ChannelPermission.HasValue && !((!(context.Channel is IGuildChannel channel)) ? ChannelPermissions.All(context.Channel) : guildUser!.GetPermissions(channel)).Has(ChannelPermission.Value))
        {
            return PreconditionResult.FromError(ErrorMessage ?? string.Format(guildConfiguration.LocalizedStrings.ErrorUserRequiresChannelPermission , $"{Format.Bold(Format.Code(ChannelPermission.Value.ToString()))}"));
        }

        return PreconditionResult.FromSuccess();
    }
}

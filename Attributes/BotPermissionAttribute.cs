using System;
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
        if (context.Guild != null)
        {
            guildUser = await context.Guild.GetCurrentUserAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        if (GuildPermission.HasValue)
        {
            if (guildUser == null)
            {
                return PreconditionResult.FromError($"Command must be used in {Format.Bold(Format.Code("a guild channel."))}");
            }

            if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
            {
                return PreconditionResult.FromError(ErrorMessage ?? $"Bot requires guild permission {Format.Bold(Format.Code(GuildPermission.Value.ToString()))}"); 
            }
        }

        if (ChannelPermission.HasValue && !((!(context.Channel is IGuildChannel channel)) ? ChannelPermissions.All(context.Channel) : guildUser!.GetPermissions(channel)).Has(ChannelPermission.Value))
        {
            return PreconditionResult.FromError(ErrorMessage ?? $"Bot requires channel permission {Format.Bold(Format.Code(ChannelPermission.Value.ToString()))}");
        }

        return PreconditionResult.FromSuccess();
    }
}

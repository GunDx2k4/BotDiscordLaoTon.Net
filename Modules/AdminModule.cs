using BotDiscordLaoTon.Net.Helpers;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules;

public class AdminModule(ILogger<AdminModule> logger, EmbedHelper embedHelper) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("test", "Test Command")]
    public async Task TestCommandAsync()
    {
        logger.LogDebug("Test command");
        await RespondAsync(embed: embedHelper.BasicEmbedBuilder("Test thôi mà cu").Build());
    }
}

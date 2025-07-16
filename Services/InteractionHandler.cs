using System.Reflection;
using System.Text;
using BotDiscordLaoTon.Net.Converters;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Helpers;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotDiscordLaoTon.Net.Services;

public class InteractionHandler(DiscordSocketClient client, InteractionService interactionService, IServiceProvider services, ILogger<InteractionHandler> logger, EmbedHelper embedHelper) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        interactionService.AddTypeConverter<Color>(new ColorConverter());
        await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);

        client.InteractionCreated += HandleInteraction;
        interactionService.InteractionExecuted += HandleInteractionExecuted;
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(client, interaction);

            var result = await interactionService.ExecuteCommandAsync(context, services);

            if (!result.IsSuccess)
                _ = Task.Run(() => HandleInteractionExecutionResult(interaction, result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }

    private Task HandleInteractionExecuted(ICommandInfo command, IInteractionContext context, IResult result)
    {
        if (!result.IsSuccess)
            _ = Task.Run(() => HandleInteractionExecutionResult(context.Interaction, result));
        return Task.CompletedTask;
    }

    private async Task HandleInteractionExecutionResult(IDiscordInteraction interaction, IResult result)
    {
        StringBuilder interactionName = new();
        if (interaction is IApplicationCommandInteraction commandInteraction)
        {
            interactionName.Append($"{commandInteraction.Data.Name}");
        }
        IGuild? guild = null;
        IChannel? channel = null;
        if (interaction.GuildId != null)
            guild = client.GetGuild((ulong)interaction.GuildId);
        if (interaction.ChannelId != null)
            channel = await client.GetChannelAsync((ulong)interaction.ChannelId);
        logger.LogError("{User} use [{interactionName}] {channel} {guild} => {Error} [{ErrorReason}]", interaction.User.ToStringDebug(), interactionName, channel.ToStringDebug(), guild?.ToStringDebug() ?? "DM", result.Error, result.ErrorReason);
        if (!interaction.HasResponded)
        {
            await interaction.RespondAsync(embed: embedHelper.ErrorEmbedBuilder($"{Format.Bold("Error")} [{result.ErrorReason}]").Build());
        }
        else
        {
            await interaction.FollowupAsync(embed: embedHelper.ErrorEmbedBuilder($"{Format.Bold("Error")} [{result.ErrorReason}]").Build());
        }
    }
}

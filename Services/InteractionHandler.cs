using System.Reflection;
using BotDiscordLaoTon.Net.Converters;
using BotDiscordLaoTon.Net.Extensions;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotDiscordLaoTon.Net.Services;

public class InteractionHandler(DiscordSocketClient client, InteractionService interactionService, IServiceProvider services, ILogger<InteractionHandler> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        interactionService.AddGenericTypeConverter<Enum>(typeof(EnumConverter<>));

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
        IGuild? guild = null;
        if (interaction.GuildId != null)
            guild = client.GetGuild(interaction.GuildId ?? 0);

        IChannel? channel = null;
        if (interaction.ChannelId != null)
            channel = await client.GetChannelAsync((ulong)interaction.ChannelId);

        logger.LogError("{User} used [{interactionName}] {channel} {guild} => {Error} [{ErrorReason}]", interaction.User.ToStringDebug(), interaction.ToStringDebug(), channel.ToStringDebug(), guild?.ToStringDebug() ?? "DM", result.Error, result.ErrorReason);
       
        if (!interaction.HasResponded)
        {
            await interaction.RespondAsync($"{Format.Bold($"{Constants.ErrorMessagePrefix}")} {result.ErrorReason}", ephemeral: true);
        }
        else
        {
            await interaction.FollowupAsync($"{Format.Bold($"{Constants.ErrorMessagePrefix}")} {result.ErrorReason}", ephemeral: true);
        }
    }
}

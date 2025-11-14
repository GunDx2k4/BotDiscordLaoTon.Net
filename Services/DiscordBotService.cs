using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Options;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace BotDiscordLaoTon.Net.Services;

public class DiscordBotService(DiscordSocketClient client, InteractionService interactions, ILogger<DiscordBotService> logger, IOptions<DiscordBotOptions> options) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            TokenUtils.ValidateToken(TokenType.Bot, options.Value.Token);
        }
        catch (ArgumentNullException e)
        {
            logger.LogError("Token is null, empty, or contains only whitespace. {Error}", e);
        }
        catch (ArgumentException e)
        {
            logger.LogError("Token value is invalid. {Error}", e);
        }


        client.Ready += ClientReady;

        client.Log += LogAsync;
        interactions.Log += LogAsync;

        return client.LoginAsync(TokenType.Bot, options.Value.Token)
            .ContinueWith(t => client.StartAsync(), cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        if (ExecuteTask is null)
            return Task.CompletedTask;

        base.StopAsync(cancellationToken);
        return client.StopAsync();
    }

    private async Task ClientReady()
    {
        logger.LogInformation("Logged as {User}", client.CurrentUser);

        var commands = await interactions.RegisterCommandsGloballyAsync();

        await client.SetActivityAsync(new Game(options.Value.Status));

        var slashCommands = commands.Where(c => c.Type == ApplicationCommandType.Slash);
        var userCommands = commands.Where(c => c.Type == ApplicationCommandType.User);
        var messageCommands = commands.Where(c => c.Type == ApplicationCommandType.Message);
        
        logger.LogInformation("Registered {Count} commands:", commands.Count);
        logger.LogInformation("  Slash Commands: {SlashCount} ({SlashNames})",
            slashCommands.Count(),
            string.Join(", ", slashCommands.Select(c => c.GetCommand())));
        logger.LogInformation("  User Commands: {UserCount} ({UserNames})",
            userCommands.Count(),
            string.Join(", ", userCommands.Select(c => c.GetCommand())));
        logger.LogInformation("  Message Commands: {MessageCount} ({MessageNames})",
            messageCommands.Count(),
            string.Join(", ", messageCommands.Select(c => c.GetCommand())));
        
    }

    public Task LogAsync(LogMessage msg)
    {
        var severity = msg.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };

        logger.Log(severity, msg.Exception, msg.Message);

        return Task.CompletedTask;
    }
}
using System;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Guilds;
using Discord.WebSocket;

namespace BotDiscordLaoTon.Net.Services;

public class GuildService(DiscordSocketClient client, ILogger<GuildService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var guilds = await client.Rest.GetGuildsAsync();

        foreach (var guild in guilds)
        {
            GuildManager.GuildConfigurations.Add(guild.Id, new GuildConfiguration());
        }
        GuildManager.GuildConfigurations.Add(0, new GuildConfiguration());
        logger.LogInformation("Loaded {countGuild} guild", GuildManager.GuildConfigurations.Count - 1);
        client.JoinedGuild += BotJoinNewGuid;
        client.LeftGuild += BotLeftGuild;
    }

    private Task BotJoinNewGuid(SocketGuild newGuild)
    {
        logger.LogInformation("Bot join new guild {Guild}", newGuild.ToStringDebug());
        GuildManager.GuildConfigurations.Add(newGuild.Id, new GuildConfiguration());
        return Task.CompletedTask;
    }
    private Task BotLeftGuild(SocketGuild guild)
    {
        logger.LogInformation("Bot left guild {Guild}", guild.ToStringDebug());
        GuildManager.GuildConfigurations.Remove(guild.Id);
        return Task.CompletedTask;
    }
}

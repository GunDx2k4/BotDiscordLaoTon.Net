using System;
using Discord;

namespace BotDiscordLaoTon.Net.Extensions;

public static class DebugExtension
{
    public static string? ToStringDebug(this ISnowflakeEntity? entity)
    {
        if (entity == null)
        {
            return string.Empty;
        }
        else if (entity is IGuild guild)
        {
            return $"{guild.Name}[{guild.Id}]";
        }
        else if (entity is IUser user)
        {
            return $"{user.GlobalName ?? user.Username}[{user.Id}]";
        }
        else if (entity is IRole role)
        {
            return $"{role.Name}[{role.Id}]";
        }
        else if (entity is IChannel channel)
        {
            return $"{channel.Name}[{channel.Id}]";
        }
        else
        {
            return $"{entity.GetType().Name}[{entity.Id}]";
        }
    }
}

using System;
using Discord;

namespace BotDiscordLaoTon.Net.Extensions;

public static class DebugExtensions
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
        else if (entity is IAttachment attachment)
        {
            return $"{attachment.Filename}[{attachment.Id}]";
        }
        else if (entity is IUserCommandInteraction userCommandInteraction)
        {
            return $"UserCommand:{userCommandInteraction.Data.Name}[{userCommandInteraction.Id}]";
        }
        else if (entity is IMessageCommandInteraction messageCommandInteraction)
        {
            return $"MessageCommand:{messageCommandInteraction.Data.Name}[{messageCommandInteraction.Id}]";
        }
        else if (entity is ISlashCommandInteraction slashCommandInteraction)
        {
            return $"SlashCommand:{slashCommandInteraction.GetCommand()}[{slashCommandInteraction.Id}]";
        }
        else if (entity is IComponentInteraction componentInteraction)
        {
            return $"Component:{componentInteraction.Data.CustomId}[{componentInteraction.Id}]";
        }
        else if (entity is IModalInteraction modalInteraction)
        {
            return $"Modal:{modalInteraction.Data.CustomId}[{modalInteraction.Id}]";
        }
        else if (entity is IEmote emote)
        {
            return $"{emote.Name}";
        }
        else if (entity is IStickerItem sticker)
        {
            return $"{sticker.Name}[{sticker.Id}]";
        }
        else if (entity is IMessage message)
        {
            return $"Message[{message.Id}]";
        }
        else
        {
            return $"{entity.GetType().Name}[{entity.Id}]";
        }
    }

    public static string GetCommand(this ISlashCommandInteraction interaction)
    {
        var option = interaction.Data.Options.Where(c => c.Type == ApplicationCommandOptionType.SubCommand || c.Type == ApplicationCommandOptionType.SubCommandGroup);
        var optionStr = option != null && option.Any() ? $"[{string.Join(", ", option.Select(o => o.Name))}]" : "";
        return $"{interaction.Data.Name} {optionStr}".Trim();
    } 

    public static string GetCommand(this IApplicationCommand interaction)
    {
        var option = interaction.Options.Where(c => c.Type == ApplicationCommandOptionType.SubCommand || c.Type == ApplicationCommandOptionType.SubCommandGroup);
        var optionStr = option != null && option.Any() ? $"[{string.Join(", ", option.Select(o => o.Name))}]" : "";
        return $"{interaction.Name} {optionStr}".Trim();
    } 
}

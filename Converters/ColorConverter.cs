using System;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Converters;

public class ColorConverter : TypeConverter<Color>
{
    public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
    {
        if (Color.TryParse((string)option.Value, out Color color))
            return Task.FromResult(TypeConverterResult.FromSuccess(color));
        else
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Value {Format.Bold(option.Value.ToString())} cannot be converted to {Format.Bold("Color")}"));
    }
}

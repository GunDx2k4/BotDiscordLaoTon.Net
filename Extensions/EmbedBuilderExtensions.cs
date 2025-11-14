using System;
using Discord;

namespace BotDiscordLaoTon.Net.Extensions;

public static class EmbedBuilderExtensions
{
    public static Embed BuildInfoEmbed(this EmbedBuilder builder)
    {
        builder.WithColor(Color.Blue);
        return builder.Build();
    }

    public static Embed BuildErrorEmbed(this EmbedBuilder builder)
    {
        builder.WithColor(Color.Red)
        .WithAuthor(new EmbedAuthorBuilder
        {
            Name = Constants.ErrorMessagePrefix,
            IconUrl = Constants.CrossIconUrl
        });
        return builder.Build();
    }

    public static Embed BuildSuccessEmbed(this EmbedBuilder builder)
    {
        builder.WithColor(Color.Green)
        .WithAuthor(new EmbedAuthorBuilder
        {
            Name = Constants.SuccessMessagePrefix,
            IconUrl = Constants.CheckIconUrl
        });
        return builder.Build();
    }
}

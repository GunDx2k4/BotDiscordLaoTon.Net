using System;

namespace BotDiscordLaoTon.Net.Extensions;

public static class ButtonExtensions
{
    public static string ToButtonId(this string id) => $"{Constants.ButtonCustomIdPrefix}_{id}";
}

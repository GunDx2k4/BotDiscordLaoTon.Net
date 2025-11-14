using System;

namespace BotDiscordLaoTon.Net.Extensions;

public static class ModalExtensions
{
    public static string ToModalId(this string customId) => $"{Constants.ModalCustomIdPrefix}_{customId}";
}

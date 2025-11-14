using System;
using Discord;

namespace BotDiscordLaoTon.Net.Extensions;

public static class SelectMenuBuilderExtensions
{
    public static SelectMenuBuilder AddOption<T>(this SelectMenuBuilder menu) where T : Enum
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            menu.AddOption(value.GetLabelString(), value.GetValueString(), value.GetDescriptionString());
        }
        return menu;
    }

    public static string ToSelectMenuId(this string id) => $"{Constants.SelectMenuCustomIdPrefix}_{id}";
}

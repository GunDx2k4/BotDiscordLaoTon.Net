using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BotDiscordLaoTon.Net.Extensions;

public static class EnumExtensions
{
    public static string GetDescriptionString<T>(this T value) where T : Enum
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static string GetLabelString<T>(this T value) where T : Enum
    {
        return Regex.Replace(value.ToString(),"([A-Z])"," $1").Trim();
    }

    public static string GetValueString(this Enum value)
    {
        return Regex.Replace(value.ToString(),"(?<!^)([A-Z])","_$1").Trim().ToLower();
    }
}

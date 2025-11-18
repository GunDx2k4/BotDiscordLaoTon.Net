using System;
using System.Threading.Tasks;
using Discord;

namespace BotDiscordLaoTon.Net.Extensions;

public static class GuildExtensions
{
    public static async Task<IRole?> GetRoleByNameAsync(this IGuild guild, string roleName)
    {
        var role = guild.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        return role;
    }

    public static async Task<IRole> CreateRoleByNameAsync(this IGuild guild, string roleName)
    {
        var role = await guild.CreateRoleAsync(roleName, permissions: GuildPermissions.None, isMentionable: true, isHoisted: true, color: Color.Blue);
        return role;
    }

    public static async Task<IRole> CreateOrGetRoleAsync(this IGuild guild, string roleName)
    {
        var role = await guild.GetRoleByNameAsync(roleName);

        if (role == null)
        {
            role = await guild.CreateRoleByNameAsync(roleName);
        }
        return role;
    }

    public static async Task<ICategoryChannel?> GetCategoryByNameAsync(this IGuild guild, string categoryName)
    {
        var category = (await guild.GetCategoriesAsync())
            .FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
        return category;
    }

    public static async Task<ICategoryChannel> CreateCategoryByNameAsync(this IGuild guild, string categoryName)
    {
        var category = await guild.CreateCategoryAsync(categoryName.ToUpper(), prop =>
        {
            prop.Position = 0;
        });
        return category;
    }

    public static async Task<ICategoryChannel> CreateOrGetCategoryAsync(this IGuild guild, string categoryName)
    {
        var category = await guild.GetCategoryByNameAsync(categoryName);

        if (category == null)
        {
            category = await guild.CreateCategoryByNameAsync(categoryName);
        }
        return category;
    }

    public static async Task<T?> GetChannelByNameAsync<T>(this ICategoryChannel category, string channelName) where T : INestedChannel
    {
        var guild = category.Guild;
        var channel = (await guild.GetChannelsAsync())
            .OfType<T>()
            .FirstOrDefault(c => c.CategoryId == category.Id && c.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
        return channel;
    }

    public static async Task<T> CreateChannelByNameAsync<T>(this ICategoryChannel category, string channelName) where T : INestedChannel
    {
        var guild = category.Guild;
        T channel;

        switch (typeof(T))
        {
            case Type t when t == typeof(ITextChannel):
                channel = (T)await guild.CreateTextChannelAsync(channelName, prop =>
                {
                    prop.CategoryId = category.Id;
                });
                break;
            case Type t when t == typeof(IVoiceChannel):
                channel = (T)await guild.CreateVoiceChannelAsync(channelName, prop =>
                {
                    prop.CategoryId = category.Id;
                    prop.UserLimit = 99;
                });
                break;
            case Type t when t == typeof(IStageChannel):
                channel = (T)await guild.CreateStageChannelAsync(channelName, prop =>
                {
                    prop.CategoryId = category.Id;
                });
                break;
            case Type t when t == typeof(INewsChannel):
                channel = (T)await guild.CreateNewsChannelAsync(channelName, prop =>
                {
                    prop.CategoryId = category.Id;
                });
                break;
            case Type t when t == typeof(IForumChannel):
                channel = (T)await guild.CreateForumChannelAsync(channelName, prop =>
                {
                    prop.CategoryId = category.Id;
                });
                break;
            case Type t when t == typeof(IStageChannel):
                channel = (T)await guild.CreateStageChannelAsync(channelName, prop =>
                {
                    prop.CategoryId = category.Id;
                });
                break;
            default:
                throw new NotSupportedException($"Channel type '{typeof(T).Name}' is not supported.");
        }
        return channel;
    }

    public static async Task<T> CreateOrGetChannelAsync<T>(this ICategoryChannel category, string channelName) where T : INestedChannel
    {
        var guild = category.Guild;
        var channel = await category.GetChannelByNameAsync<T>(channelName);

        if (channel == null)
        {
            channel = await category.CreateChannelByNameAsync<T>(channelName);
        }
        return channel;
    }

    public static async Task<IEnumerable<IGuildChannel>> GetChannelsInCategoryAsync(this IGuild guild, ICategoryChannel category)
    {
        var channels = (await guild.GetChannelsAsync())
            .Where(c => (c as INestedChannel)?.CategoryId == category.Id);
        return channels;
    }

    public static async Task DeleteCategoryAndChannelsAsync(this IGuild guild, ICategoryChannel category)
    {
        var channels = await guild.GetChannelsInCategoryAsync(category);
        foreach (var channel in channels)
        {
            await channel.DeleteAsync();
        }
        await category.DeleteAsync();
    }
}

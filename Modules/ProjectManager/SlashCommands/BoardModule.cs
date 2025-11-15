using System;
using BotDiscordLaoTon.Net.Extensions;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.ProjectManager.SlashCommands;

[Group("board", "Commands for managing boards")]
public class BoardModule(ILogger<BoardModule> logger) : ProjectManagerRequire(logger)
{
    [SlashCommand("create", "Create a new board")]
    public async Task CreateBoardAsync(string name)
    {
        await DeferAsync();

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
        {
            await FollowupAsync("Board name must be between 1 and 100 characters.");
            return;
        }
        
        var memberRole = await Context.Guild.CreateOrGetRoleAsync($"{name} Member");

        var category = await Context.Guild.CreateOrGetCategoryAsync(name);
        await category.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
        await category.AddPermissionOverwriteAsync(memberRole, new OverwritePermissions(viewChannel: PermValue.Allow));

        var infoChannel = await category.CreateOrGetChannelAsync<ITextChannel>("info");
        await infoChannel.ModifyAsync(prop => prop.Topic = $"Information channel for the '{name}'.");
        await infoChannel.DeleteMessagesAsync(await infoChannel.GetMessagesAsync(100).FlattenAsync());
        await infoChannel.SendMessageAsync($"Welcome to the **{name}** board! This channel contains information about the board.");

        var chatChannel = await category.CreateOrGetChannelAsync<ITextChannel>("chat");
        await chatChannel.ModifyAsync(prop => prop.Topic = $"General chat channel for the '{name}'.");

        var voiceChannel = await category.CreateOrGetChannelAsync<IVoiceChannel>("Voice");
        await voiceChannel.ModifyAsync(prop => prop.UserLimit = 99);

        await FollowupAsync($"Board '{name}' has been created with role '{memberRole.Name}' and chat '{chatChannel.Mention}' or voice {voiceChannel.Mention}.");
    }

    [SlashCommand("delete", "Delete an existing board")]
    public async Task DeleteBoardAsync(string name)
    {
        await DeferAsync();

        var category = await Context.Guild.GetCategoryByNameAsync(name);
        if (category != null)
        {
            foreach (var channel in await Context.Guild.GetChannelsInCategoryAsync(category))
            {
                await channel.DeleteAsync();
            }
            await category.DeleteAsync();
            var role = await Context.Guild.GetRoleByNameAsync($"{name} Member");
            if (role != null)
            {
                await role.DeleteAsync();
            }
            await FollowupAsync($"Board '{name}' and its associated channels and role have been deleted.");
            return;
        }

        await FollowupAsync($"Board '{name}' does not exist.");
    }
}

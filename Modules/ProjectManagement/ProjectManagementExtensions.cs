using System;
using BotDiscordLaoTon.Net.Extensions;
using Discord;

namespace BotDiscordLaoTon.Net.Modules.ProjectManagement;

public static class ProjectManagementExtensions
{
    private static readonly OverwritePermissions MemberRolePermissions = new OverwritePermissions(
        viewChannel: PermValue.Allow,
        sendMessages: PermValue.Allow,
        readMessageHistory: PermValue.Allow,

        addReactions: PermValue.Allow,
        embedLinks: PermValue.Allow,
        attachFiles: PermValue.Allow,
        useApplicationCommands: PermValue.Allow,

        connect: PermValue.Allow,
        speak: PermValue.Allow,

        createPublicThreads: PermValue.Allow,
        sendMessagesInThreads: PermValue.Allow
    );

    private static readonly OverwritePermissions OwnerPermissions = MemberRolePermissions.Modify(
        manageChannel: PermValue.Allow,
        manageMessages: PermValue.Allow,
        manageRoles: PermValue.Allow,
        manageWebhooks: PermValue.Allow,
        manageThreads: PermValue.Allow,
        mentionEveryone: PermValue.Allow,

        muteMembers: PermValue.Allow,
        deafenMembers: PermValue.Allow,

        createPrivateThreads: PermValue.Allow
    );


    public static async Task<(IRole memberRole, ICategoryChannel categoryChannel, ITextChannel infoChannel, ITextChannel boardChannel)> CreateBoardResourcesAsync(this IInteractionContext context, string boardName)
    {
        var guild = context.Guild;

        var memberRole = await guild.CreateRoleByNameAsync($"{boardName} Member");

        var category = await guild.CreateCategoryByNameAsync(boardName);
        await category.AddPermissionOverwriteAsync(guild.EveryoneRole, OverwritePermissions.DenyAll(category));
        await category.AddPermissionOverwriteAsync(memberRole, MemberRolePermissions);
        await category.AddPermissionOverwriteAsync(context.User, OwnerPermissions);

        var boardChannel = await category.CreateChannelByNameAsync<ITextChannel>("board");
        await boardChannel.ModifyAsync(prop => prop.Topic = $"Board channel for the '{boardName}'.");
        await boardChannel.AddPermissionOverwriteAsync(memberRole, MemberRolePermissions.Modify(sendMessages: PermValue.Deny));

        var infoChannel = await category.CreateChannelByNameAsync<ITextChannel>("info");
        await infoChannel.ModifyAsync(prop => prop.Topic = $"Information channel for the '{boardName}'.");

        var chatChannel = await category.CreateChannelByNameAsync<ITextChannel>("chat");
        await chatChannel.ModifyAsync(prop => prop.Topic = $"Chat channel for the '{boardName}'.");
        var voiceChannel = await category.CreateChannelByNameAsync<IVoiceChannel>("voice");

        return (memberRole, category, infoChannel, boardChannel);
    }

    public static async Task<(IRole memberRole, ICategoryChannel categoryChannel, ITextChannel infoChannel, ITextChannel boardChannel)> GetOrCreateBoardResourcesAsync(this IInteractionContext context, string boardName)
    {
        var guild = context.Guild;

        var memberRole = await guild.CreateOrGetRoleAsync($"{boardName} Member");

        var category = await guild.CreateOrGetCategoryAsync(boardName);
        await category.AddPermissionOverwriteAsync(guild.EveryoneRole, OverwritePermissions.DenyAll(category));
        await category.AddPermissionOverwriteAsync(memberRole, MemberRolePermissions);
        await category.AddPermissionOverwriteAsync(context.User, OwnerPermissions);

        var boardChannel = await category.CreateOrGetChannelAsync<ITextChannel>("board");
        await boardChannel.ModifyAsync(prop => prop.Topic = $"Board channel for the '{boardName}'.");
        await boardChannel.AddPermissionOverwriteAsync(memberRole, MemberRolePermissions.Modify(sendMessages: PermValue.Deny));

        var infoChannel = await category.CreateOrGetChannelAsync<ITextChannel>("info");
        await infoChannel.ModifyAsync(prop => prop.Topic = $"Information channel for the '{boardName}'.");

        var chatChannel = await category.CreateOrGetChannelAsync<ITextChannel>("chat");
        await chatChannel.ModifyAsync(prop => prop.Topic = $"Chat channel for the '{boardName}'.");
        var voiceChannel = await category.CreateOrGetChannelAsync<IVoiceChannel>("voice");
        await voiceChannel.ModifyAsync(prop => prop.UserLimit = 99);

        return (memberRole, category, infoChannel, boardChannel);
    }
}

using System;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Models;
using BotDiscordLaoTon.Net.Repositories;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.ProjectManagement.SlashCommands;

[Group("board", "Commands for managing boards")]
public class BoardModule(ILogger<BoardModule> logger, IBoardRepository boardRepository) : ProjectManagementRequire(logger)
{
    [SlashCommand("create", "Create a new board")]
    public async Task CreateBoardAsync([Summary(description: "The name of the board")] string name, [Summary(description: "Override existing board if it exists")] bool isOverride = false)
    {
        await DeferAsync(ephemeral: false);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
        {
            await FollowupAsync("Board name must be between 1 and 100 characters.");
            return;
        }

        if (isOverride)
        {
            var (memberRole, category, infoChannel, boardChannel) = await Context.GetOrCreateBoardResourcesAsync(name);

            var oldBoard = await boardRepository.GetBoardByCategoryChannelIdAsync(category.Id);
            if (oldBoard != null)
            {
                oldBoard.Name = name;
                oldBoard.MemberRoleId = memberRole.Id;
                oldBoard.InfoChannelId = infoChannel.Id;
                oldBoard.BoardChannelId = boardChannel.Id;

                if (!await boardRepository.UpdateBoardAsync(oldBoard))
                {
                    await boardRepository.AddBoardAsync(new Board
                    {
                        Name = name,
                        CategoryChannelId = category.Id,
                        GuildId = Context.Guild.Id,
                        OwnerId = Context.User.Id,
                        MemberRoleId = memberRole.Id,
                        InfoChannelId = infoChannel.Id,
                        BoardChannelId = boardChannel.Id
                    });
                }
                await FollowupAsync($"Board '{name}' already exists. Resources have been updated.");
                return;
            }

            await boardRepository.AddBoardAsync(new Board
            {
                Name = name,
                CategoryChannelId = category.Id,
                GuildId = Context.Guild.Id,
                OwnerId = Context.User.Id,
                MemberRoleId = memberRole.Id,
                InfoChannelId = infoChannel.Id,
                BoardChannelId = boardChannel.Id
            });
            await FollowupAsync($"Board '{name}' has been created successfully.");
        }
        else
        {
            var (memberRole, category, infoChannel, boardChannel) = await Context.CreateBoardResourcesAsync(name);
            await FollowupAsync($"Board '{name}' has been created or updated successfully.");

            var board = new Board
            {
                Name = name,
                CategoryChannelId = category.Id,
                GuildId = Context.Guild.Id,
                OwnerId = Context.User.Id,
                MemberRoleId = memberRole.Id,
                InfoChannelId = infoChannel.Id,
                BoardChannelId = boardChannel.Id
            };

            await boardRepository.AddBoardAsync(board);
        }


    }

    [SlashCommand("list", "List all existing boards")]
    public async Task ListBoardsAsync()
    {
        await DeferAsync(ephemeral: true);

        var boards = await boardRepository.GetBoardsByGuildIdAsync(Context.Guild.Id);
        if (boards.Count == 0)
        {
            await FollowupAsync("No boards found in this server.");
            return;
        }

        foreach (var board in boards)
        {
            var owner = await Context.Guild.GetUserAsync(board.OwnerId);
            var memberRole = await Context.Guild.GetRoleAsync(board.MemberRoleId);
            var boardChannel = await Context.Guild.GetChannelAsync(board.BoardChannelId) as ITextChannel;
            var embedBuilder = new EmbedBuilder
            {
                Title = board.Name,
                Description = $"Board ID: {board.Id}\n" +
                              $"Owner ID: {owner.Mention}\n" +
                              $"Member Role ID: {memberRole.Mention}\n" +
                              $"Board Channel ID: {boardChannel?.Mention}"
            };
            await FollowupAsync(embed: embedBuilder.BuildInfoEmbed());
        }

    }
}

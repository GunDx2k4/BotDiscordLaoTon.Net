using System;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Repositories;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.ProjectManagement.Buttons;

public class ProjectManagermentButtonModule(ILogger<ProjectManagermentButtonModule> logger, IBoardRepository boardRepository) : ProjectManagementRequire(logger)
{
    
    [ComponentInteraction($"{Constants.ButtonCustomIdPrefix}_delete_board_*")]
    public async Task HandleTestingButtonAsync(long boardId)
    {
        await DeferAsync(ephemeral: true);

        var board = await boardRepository.GetBoardByIdAsync(boardId);
        if (board == null)
        {
            await FollowupAsync($"Board with ID {boardId} does not exist.");
            return;
        }

        if (!await boardRepository.DeleteBoardAsync(boardId))
        {
            await FollowupAsync($"Failed to delete board with ID {boardId}. It may not exist.");
            return;
        }

        var roleMember = await Context.Guild.GetRoleAsync(board.MemberRoleId);
        if (roleMember != null)
        {
            await roleMember.DeleteAsync();
        }
        var categoryChannel = await Context.Guild.GetChannelAsync(board.CategoryChannelId) as ICategoryChannel;

        if (categoryChannel != null)
        {
            await Context.Guild.DeleteCategoryAndChannelsAsync(categoryChannel);
        }



        await FollowupAsync($"Board with ID {boardId} has been deleted.");
    }
}

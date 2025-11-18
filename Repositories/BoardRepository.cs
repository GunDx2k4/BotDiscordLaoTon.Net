using System;
using BotDiscordLaoTon.Net.Data;
using BotDiscordLaoTon.Net.Models;
using Microsoft.EntityFrameworkCore;

namespace BotDiscordLaoTon.Net.Repositories;

public class BoardRepository(ApplicationDbContext context) : IBoardRepository
{
    private readonly DbSet<Board> _boards = context.Boards;

    public async Task AddBoardAsync(Board board)
    {
        await _boards.AddAsync(board);
        await context.SaveChangesAsync();
    }

    public async Task<Board?> GetBoardByIdAsync(long id)
    {
        return await _boards.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Board?> GetBoardByCategoryChannelIdAsync(ulong categoryChannelId)
    {
        return await _boards.FirstOrDefaultAsync(b => b.CategoryChannelId == categoryChannelId);
    }

    public async Task<List<Board>> GetBoardsByGuildIdAsync(ulong guildId)
    {
        return await _boards.Where(b => b.GuildId == guildId).ToListAsync();
    }

    public async Task<bool> DeleteBoardAsync(long id)
    {
        var board = await GetBoardByIdAsync(id);
        if (board == null)
            return false;

        _boards.Remove(board);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateBoardAsync(Board board)
    {
        var existingBoard = await GetBoardByIdAsync(board.Id);
        if (existingBoard == null)
            return false;

        context.Entry(existingBoard).CurrentValues.SetValues(board);
        return await context.SaveChangesAsync() > 0;
    }
}
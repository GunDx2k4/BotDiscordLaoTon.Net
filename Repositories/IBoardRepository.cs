using System;
using BotDiscordLaoTon.Net.Models;

namespace BotDiscordLaoTon.Net.Repositories;

public interface IBoardRepository
{
    public Task AddBoardAsync(Board board);

    public Task<Board?> GetBoardByIdAsync(long id);

    public Task<Board?> GetBoardByCategoryChannelIdAsync(ulong categoryChannelId);

    public Task<List<Board>> GetBoardsByGuildIdAsync(ulong guildId);

    public Task<bool> DeleteBoardAsync(long id);

    public Task<bool> UpdateBoardAsync(Board board);
}

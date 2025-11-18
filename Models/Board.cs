using System;

namespace BotDiscordLaoTon.Net.Models;

public class Board
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required ulong GuildId { get; set; }
    public required ulong CategoryChannelId { get; set; }
    public required ulong OwnerId { get; set; }
    public required ulong MemberRoleId { get; set; }
    public required ulong InfoChannelId { get; set; }
    public required ulong BoardChannelId { get; set; }
}

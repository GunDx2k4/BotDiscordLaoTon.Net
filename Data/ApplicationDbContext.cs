using System;
using BotDiscordLaoTon.Net.Models;
using Microsoft.EntityFrameworkCore;

namespace BotDiscordLaoTon.Net.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Board> Boards { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

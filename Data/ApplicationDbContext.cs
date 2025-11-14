using System;
using Microsoft.EntityFrameworkCore;

namespace BotDiscordLaoTon.Net.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

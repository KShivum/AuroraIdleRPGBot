using AuroraLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AuroraAPi;

public class AuroraContext : DbContext
{
    public AuroraContext(DbContextOptions<AuroraContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null;
    public DbSet<Item> Items { get; set; } = null;
}
using AuroraLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AuroraAPi;

public class AuroraContext : DbContext
{
    public AuroraContext(DbContextOptions<AuroraContext> options) : base(options)
    {
    }
    
    public AuroraContext()
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Item> Items { get; set; }
}
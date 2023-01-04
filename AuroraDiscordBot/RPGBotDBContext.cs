using System.Configuration;
using AuroraLibrary.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


public class RPGBotDBContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Item> Items { get; set; }

    public RPGBotDBContext()
    {
    }
    public RPGBotDBContext(DbContextOptions<RPGBotDBContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        
        string conString = configuration.GetConnectionString("RPGBot");
        optionsBuilder.UseSqlServer(conString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Player>().Property(p => p.Id).HasConversion<string>();
    }


}


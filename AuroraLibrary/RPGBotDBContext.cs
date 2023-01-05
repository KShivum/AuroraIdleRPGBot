using System.Configuration;
using AuroraLibrary.Config;
using AuroraLibrary.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


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
        Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"))!;
        optionsBuilder.UseSqlServer(config.ConnectionSettings.ToString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Player>().Property(p => p.Id).HasConversion<string>();
    }


}


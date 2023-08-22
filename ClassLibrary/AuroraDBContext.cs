using ClassLibrary.Configuration;
using ClassLibrary.Models;
using Config.Net;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary;

public class AuroraDBContext : DbContext
{
    public AuroraDBContext()
    {
    }

    public AuroraDBContext(DbContextOptions<AuroraDBContext> options): base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Check if config file exists
        if (!File.Exists("./config.ini"))
        {
            Console.WriteLine("Config file does not exist, creating....");
            ConfigWriter.WriteConfigFile();
            return;
        }
        IConfig config = new ConfigurationBuilder<IConfig>().UseIniFile("./config.ini").Build();
        string _connectionString =
            $@"Host={config.DatabaseHost};Username={config.DatabaseUser};Password={config.DatabasePassword};Database={config.DatabaseName}";
        optionsBuilder.UseNpgsql(_connectionString);
    }

    public DbSet<User> Users { get; set; }
}
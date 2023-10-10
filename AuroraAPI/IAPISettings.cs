namespace AuroraAPi;
using Config.Net;

public interface IAPISettings
{
    [Option(Alias = "Database.DatabaseHost")]
    string DatabaseHost { get; set; }
    [Option(Alias = "Database.DatabaseUser")]
    string DatabaseUser { get; set; }
    [Option(Alias = "Database.DatabasePassword")]
    string DatabasePassword { get; set; }
    [Option(Alias = "Database.DatabaseName")]
    string DatabaseName { get; set; }
}
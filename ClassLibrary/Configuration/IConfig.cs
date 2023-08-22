using Config.Net;

namespace ClassLibrary.Configuration;

public interface IConfig
{
    [Option(Alias = "BotConfiguration.BotToken")]
    public string BotToken { get;}
    [Option(Alias = "BotConfiguration.BotPrefix", DefaultValue = "!")]
    public string BotPrefix { get; set; }
    
    [Option(Alias = "DatabaseConfiguration.DatabaseUser")]
    public string DatabaseUser { get; }
    [Option(Alias = "DatabaseConfiguration.DatabasePassword")]
    public string DatabasePassword { get; }
    [Option(Alias = "DatabaseConfiguration.DatabaseName")]
    public string DatabaseName { get; }
    [Option(Alias = "DatabaseConfiguration.DatabaseHost")]
    public string DatabaseHost { get; }
}
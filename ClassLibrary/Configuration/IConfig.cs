using Config.Net;

namespace ClassLibrary.Configuration;

public interface IConfig
{
    [Option(Alias = "BotConfiguration.BotToken")]
    public string BotToken { get;}
    [Option(Alias = "BotConfiguration.BotPrefix", DefaultValue = "!")]
    public string BotPrefix { get; set; }
}
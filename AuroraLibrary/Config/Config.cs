namespace AuroraLibrary.Config;

public class Config
{
    public Connection ConnectionSettings { get; set; } = new();
    public Bot BotSettings { get; set; } = new();
    public Game GameSettings { get; set; } = new();
}
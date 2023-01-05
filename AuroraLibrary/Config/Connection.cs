namespace AuroraLibrary.Config;

public class Connection
{
    public string ServerAddress { get; set; } = "";
    public string Database { get; set; } = "";
    public string DBUserName { get; set; } = "";
    public string DBPassword { get; set; } = "";

    public override string ToString()
        => $"Server={ServerAddress};Database={Database};User Id={DBUserName};Password={DBPassword};";
}
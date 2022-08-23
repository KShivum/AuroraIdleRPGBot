namespace AuroraLibrary.ConfigManager;

public static class ConfigManager
{
    public static Dictionary<string, string> Config { get; private set; }

    public static void Initialize()
    {
        Config = new Dictionary<string, string>();

        if(!File.Exists("config.cfg"))
        {
            File.Create("config.cfg");
        }

        string[] lines = System.IO.File.ReadAllLines(@"config.cfg");
        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                continue;
            }
            string[] split = line.Split(':');
            if (split.Length != 2)
            {
                continue;
            }
            if(Config.ContainsKey(split[0]))
            {
                continue;
            }
            Config.Add(split[0], split[1]);
        }
    }

    public static bool AddToConfigAndError(string key)
    {
        if (Config.ContainsKey(key) && !string.IsNullOrWhiteSpace(Config[key]))
        {
            Console.WriteLine($"{key} exists in config.cfg");
            return false;
        }
        if(Config.ContainsKey(key) && string.IsNullOrWhiteSpace(Config[key]))
        {
            Console.WriteLine($"{key} is empty");
            return true;
        }
        System.IO.File.AppendAllText(@"config.cfg", $"{key}:\n");
        return true;
    }
}

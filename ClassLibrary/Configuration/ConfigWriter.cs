namespace ClassLibrary.Configuration;

public static class ConfigWriter
{
    public static void WriteConfigFile()
    {
        File.WriteAllText("./config.ini",ConfigWrite);
    }

    private static string ConfigWrite =$@"[BotConfiguration]
BotToken=
BotPrefix=";



}
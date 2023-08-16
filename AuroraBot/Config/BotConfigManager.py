import configparser
import os


def LoadConfig():
    if not os.path.exists("./BotConfig.ini"):
        config = configparser.ConfigParser()
        config["BotConfig"] = {
            "BotToken": "",
            "BotPrefix": ""
        }
        config["DatabaseConfig"] = {
            "DBUser": "",
            "DBPass": "",
            "Database": "",
            "DBHost": "",
        }
        with open("./BotConfig.ini", "w") as configfile:
            config.write(configfile)
            quit()
    config = configparser.ConfigParser()
    config.sections()
    config.read("./BotConfig.ini")
    return config
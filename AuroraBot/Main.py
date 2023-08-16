import asyncio

from discord.ext import commands
from Commands.UserManagement.CreateUser import CreateUserCog

import Config.BotConfigManager
import discord


async def main():
    botConfig = Config.BotConfigManager.LoadConfig()

    intents = discord.Intents.default()
    intents.message_content = True
    bot = commands.Bot(botConfig["BotConfig"]["BotPrefix"], intents=intents, help_command=None)

    @bot.event
    async def on_ready():
        print(f'We have logged in as {bot.user}')

    
    await bot.add_cog(CreateUserCog(bot, botConfig["DatabaseConfig"]))
    await bot.start(botConfig["BotConfig"]["BotToken"])

asyncio.run(main())